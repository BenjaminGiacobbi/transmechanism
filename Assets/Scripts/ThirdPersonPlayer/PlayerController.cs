using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerHealth))]
public class PlayerController : MonoBehaviour, IRecoil
{
    public event Action Active = delegate { };
    public event Action Inactive = delegate { };
    public event Action Idle = delegate { };
    public event Action StartRunning = delegate { };
    public event Action StartJump = delegate { };
    public event Action StartFall = delegate { };
    public event Action StartSprint = delegate { };
    public event Action Land = delegate { };
    public event Action Ability = delegate { };
    public event Action StartRecoil = delegate { };
    public event Action StopRecoil = delegate { };
    public event Action Death = delegate { };

    [SerializeField] float _speed = 6f;
    [SerializeField] float _slowSpeed = 2f;
    [SerializeField] float _sprintSpeed = 12f;
    [SerializeField] float _sprintAcceleration = 2f;
    [SerializeField] float _recoilDecel = 4f;
    [SerializeField] float _fallGravityMultiplier = 1.02f;
    [SerializeField] float _turnSmoothTime = 0.1f;
    [SerializeField] float _landAnimationTime = 0.467f;

    // fields for physics calculation
    private float _turnSmoothVelocity;
    private float _verticalVelocity;
    private float _defaultSpeed = 0;
    private float _currentRecoil = 0;
    private Vector3 _recoilDirection;
    private bool _movedOnLast = false;

    private BaseState _bs = BaseState.Stand;
    private AirState _as = AirState.Grounded;
    private int _lastBase = (int)BaseState.Stand;
    private int _lastAir = (int)AirState.Grounded;

    // references
    InputController _input;
    CharacterController _controller;
    PlayerHealth _health;
    Transform _camTransform;

    // coroutine
    Coroutine _landRoutine = null;
    Coroutine _deathRoutine = null;

    // caching
    private void Awake()
    {
        _input = GetComponent<InputController>();
        _controller = GetComponent<CharacterController>();
        _health = GetComponent<PlayerHealth>();
        _camTransform = Camera.main.transform;
    }

    // start is called the frame after awake
    private void Start()
    {
        // this definitely shouldn't go here
        Application.targetFrameRate = 30;
        _bs = BaseState.Stand;
        _defaultSpeed = _speed;
    }

    private void OnEnable()
    {
        _input.Move += ApplyMovement;
        _input.Sprint += ApplySprint;
    }

    private void OnDisable()
    {
        _input.Move -= ApplyMovement;
        _input.Sprint -= ApplySprint;
    }

    private void Update()
    {
        ApplyAir();
        CalculateRecoil();
        CalculateSpeed();
    }

    private void LateUpdate()
    {
        CheckPlayerState();
    }


    // calculates player movement and accesses controller
    private void ApplyMovement(Vector3 direction)
    {
        if (direction.magnitude >= 0.1f)
        {
            CheckIfStartedMoving();
            if ((int)_bs > 2)
            {
                // Atan is tangent of angle between the x axis and vector starting at 0 and terminating at x, y (in radians by default)
                // passing in x, then z adjusts for the forward direction being positive z here
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camTransform.eulerAngles.y;


                // SmoothDampAngle adjusts and smooths the turn
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);


                // adjusts the direction of movement by applying the forward direction to the player's quaternion rotation of targetAngle
                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                _controller.Move(moveDirection.normalized * _speed * Time.deltaTime);
                _movedOnLast = true;
            }
        }
        else
        {
            CheckIfStoppedMoving();
            _movedOnLast = false;
        }
    }


    // accepts the raw input axis for jump (can be either 1 or 0)
    private void ApplyAir()
    {
        if (Grounded())
        {
            CheckIfLanded();
        }
        else
        {
            if (_as != AirState.Jump || _as != AirState.Fall)
                _as = AirState.Jump;
        }
            
            
        if (_as == AirState.Grounded)
            _verticalVelocity = Physics.gravity.y;
        else
        {
            if (_verticalVelocity < Physics.gravity.y && _as != AirState.Fall)
                _as = AirState.Fall;
            // applies gravity a multiplier on downwards fall - deltaTime is applied twice due to a bug
            _verticalVelocity += Physics.gravity.y * Time.deltaTime;
            CheckIfJumping();
        }

        // puts movement into a Vector3 to use .Move
        Vector3 playerMovement = new Vector3(0, _verticalVelocity, 0);
        _controller.Move(playerMovement * Time.deltaTime);
    }


    // applies the sprint flag, sets regardless of grounding state so the animation can be updated on landing
    private void ApplySprint(float SprintAxis)
    {
        if ((int)_bs > 2)
        {
            if (SprintAxis == 1 && _bs != BaseState.Sprint && _movedOnLast)
                _bs = BaseState.Sprint;
            else if (SprintAxis == 0 && _bs == BaseState.Sprint)
                _bs = BaseState.Stand;
        }
    }

    // adjusts speed by ramping up or down towards a target based on current sprint flag
    private void CalculateSpeed()
    {
        if (_as == AirState.Grounded && (int)_bs > 2)     // speed stays static in mid-air, forces the player to be more mindful
        {
            if (_speed < _sprintSpeed && _bs == BaseState.Sprint)
                _speed = BasicCounter.TowardsTarget(_speed, _sprintSpeed, _sprintAcceleration);

            if (_speed > _defaultSpeed && _bs != BaseState.Sprint)
                _speed = BasicCounter.TowardsTarget(_speed, _defaultSpeed, _sprintAcceleration);
        }
    }


    // this coroutine is used to avoid two-way reference with the animator, runs according to designer control
    IEnumerator LandRoutine()
    {
        _bs = BaseState.Land;
        _speed = 0;
        yield return new WaitForSeconds(_landAnimationTime);
        _bs = BaseState.Stand;
        _speed = _defaultSpeed;

        _landRoutine = null;
        yield break;
    }

    public void ApplyRecoil(Vector3 recoilDirection, float recoilSpeed)
    {
        _bs = BaseState.Recoil;
        transform.LookAt(recoilDirection);

        // clamps recoil direction to get a minimum XZ recoil, prevents player from getting stuck atop an enemy
        _recoilDirection = recoilDirection;
        float clampX = Mathf.Clamp(Mathf.Abs(_recoilDirection.x), 0.15f, 1f);
        float clampZ = Mathf.Clamp(Mathf.Abs(_recoilDirection.z), 0.15f, 1f);
        _recoilDirection = new Vector3(clampX * (_recoilDirection.x > 0 ? 1f : -1f), _recoilDirection.y, clampZ * (_recoilDirection.z > 0 ? 1f : -1f));

        _verticalVelocity = 0;
        _currentRecoil = recoilSpeed;
        _speed = _defaultSpeed;
    }

    // applies recoil with a basic timer system according to designer-determined deceleration
    private void CalculateRecoil()
    {
        if (_currentRecoil > 0)
        {
            _controller.Move(new Vector3
                (_recoilDirection.x * _currentRecoil, Physics.gravity.y * Time.deltaTime, _recoilDirection.z * _currentRecoil) * Time.deltaTime);

            _currentRecoil = BasicCounter.TowardsTarget(_currentRecoil, 0, _recoilDecel);
            if (_currentRecoil == 0)
            {
                if (_bs == BaseState.Dead)
                    return;
                else
                {
                    _bs = BaseState.Stand;
                    _recoilDirection = Vector3.zero;
                }
            }
        }
    }


    private void OnDeath()
    {
        if (_deathRoutine == null)
            _deathRoutine = StartCoroutine(DieRoutine());
    }


    // tests for ground using spherecasts
    bool Grounded()
    {
        if (Physics.SphereCast(transform.position + _controller.center, _controller.height / 6, -transform.up, out RaycastHit hit, 0.9f))
        {
            int count = 0;
            int index = 0;
            Collider[] colliders = Physics.OverlapSphere
                (new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z), _controller.height / 6);
            for(int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].GetComponent<MovingPlatform>() == true)
                {
                    count++;
                    index = i;
                } 
            }

            switch(count)
            {
                case 1:
                    transform.parent = colliders[index].transform;
                    break;
                default:
                    transform.parent = null;
                    break;
            }

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                // very simple ground clamping
                Vector3 equivalentXZ = new Vector3(hit.normal.x, 0, hit.normal.y);
                if (Vector3.Angle(equivalentXZ, hit.normal) > 60 && Vector3.Angle(equivalentXZ, hit.normal) < 85)
                {
                    _controller.Move(new Vector3(0, -20, 0));
                }
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }


    // death can't start until the player is grounded
    IEnumerator DieRoutine()
    {
        yield return new WaitForEndOfFrame();

        while (true)
        {
            if (!Grounded() && _currentRecoil != 0)
            {
                yield return null;
            }
            else
            {
                Death?.Invoke();
                yield break;
            }
        }
    }


    // sets flag for player movement
    private void CheckIfStartedMoving()
    {
        // prevents overlap with jump animations in the animation controller
        if (_bs != BaseState.Run && (int)_bs > 2)
            _bs = BaseState.Run;

    }

    // reverts flag for player movement
    private void CheckIfStoppedMoving()
    {
        if (_bs != BaseState.Stand && (int)_bs > 2)
            _bs = BaseState.Stand;

    }


    // checks for player jump, and tests if their velocity is negative to also apply falling state
    private void CheckIfJumping()
    {
        if (_as != AirState.Jump)
            _as = AirState.Jump;
    }


    // checks for landing and sets jumping flags to false
    private void CheckIfLanded()
    {
        if (_as != AirState.Grounded)
        {
            _as = AirState.Grounded;
            if (_verticalVelocity < -11 && _landRoutine == null)
                StartCoroutine(LandRoutine());
            else
            {
                if (_movedOnLast)
                    _bs = BaseState.Run;
                else
                    _bs = BaseState.Stand;
            }
                
        }
    }


    // returns the player to next state based on their current flags, the order here is based on logical exclusions
    private void CheckPlayerState()
    {
        if(_bs != BaseState.Land)
        {
            if (_lastAir != (int)_as)
            {
                if (_as == AirState.Fall)
                {
                    //Debug.Log("Invoke Fall");
                    StartFall?.Invoke();
                }
                else if (_as == AirState.Jump)
                {
                    //Debug.Log("Invoke Jump");
                    StartJump?.Invoke();
                }
            }

            if (_lastBase != (int)_bs && _as != AirState.Jump)
            {
                if (_bs == BaseState.Sprint)
                {
                    //Debug.Log("Invoke Sprint");
                    StartSprint?.Invoke();
                }
                else if (_bs == BaseState.Run)
                {
                    //Debug.Log("Invoke Run");
                    StartRunning?.Invoke();
                }
                else if (_bs == BaseState.Recoil)
                {
                    //Debug.Log("Invoke Recoil");
                    StartRecoil?.Invoke();
                }
                else if (_bs == BaseState.Dead)
                {
                    //Debug.Log("Invoke Death");
                    Death?.Invoke();
                }
                else
                {
                    //Debug.Log("Invoke Idle");
                    Idle?.Invoke();
                }
            }
        }
        else if (_lastBase != (int)_bs)
        {
            //Debug.Log("Invoke Land");
            Land?.Invoke();
        }

        _lastAir = (int)_as;
        _lastBase = (int)_bs;
    }
}

enum BaseState
{
    Land = 0,
    Recoil = 1,
    Dead = 2,
    Stand = 3,
    Run = 4,
    Sprint = 5
}

enum AirState
{
    Grounded = 0,
    Jump = 1,
    Fall = 2
}