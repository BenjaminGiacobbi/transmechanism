using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(CharacterController))]
// [RequireComponent(typeof(Health))]
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
    [SerializeField] float _jumpSpeed = 10f;
    [SerializeField] float _recoilDecel = 4f;
    [SerializeField] float _fallGravityMultiplier = 1.02f;
    [SerializeField] float _turnSmoothTime = 0.1f;
    [SerializeField] float _landAnimationTime = 0.467f;

    // fields for physics calculation
    private float _turnSmoothVelocity;
    private float _moveAngle;
    private float _verticalVelocity;
    private float _defaultSpeed = 0;
    private float _targetSpeed = 0;
    private float _currentRecoil = 0;
    private Vector3 _recoilDirection;

    // references
    CharacterController _controller;
    Transform _camTransform;

    // caching
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _camTransform = Camera.main.transform;
        _defaultSpeed = _speed;
    }

    private void Update()
    {
        VerticalMovement();
        CalculateRecoil();
        CalculateSpeed();
    }

    public void SetIdle()
    {
        Idle?.Invoke();
        _speed = 0;
        _targetSpeed = 0;
    }

    // calculates player movement and accesses controller
    public void ApplyMovement(Vector3 direction)
    {
        if (direction.magnitude >= 0.1f)
        {
            // Atan is tangent of angle between the x axis and vector starting at 0 and terminating at x, y (in radians by default)
            // passing in x, then z adjusts for the forward direction being positive z here
            _moveAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camTransform.eulerAngles.y;


            // SmoothDampAngle adjusts and smooths the turn
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _moveAngle, ref _turnSmoothVelocity, _turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);


            // adjusts the direction of movement by applying the forward direction to the player's quaternion rotation of targetAngle
            Vector3 moveDirection = Quaternion.Euler(0f, _moveAngle, 0f) * Vector3.forward;
            _controller.Move(moveDirection.normalized * _speed * Time.deltaTime);
        }
    }

    // accepts the raw input axis for jump (can be either 1 or 0)
    public void ApplyJump(float jumpAxis)
    {
        _verticalVelocity = _jumpSpeed;
    }

    private void VerticalMovement()
    {
        Vector3 playerMovement = new Vector3(0, _verticalVelocity, 0);
        _controller.Move(playerMovement * Time.deltaTime);
    }

    public void ApplyAirGravity()
    {
        _verticalVelocity += Physics.gravity.y * Time.deltaTime * (_verticalVelocity < 0 ? _fallGravityMultiplier : 1);
    }


    // applies the sprint flag, sets regardless of grounding state so the animation can be updated on landing
    public void ApplySprint()
    {
        _targetSpeed = _sprintSpeed;
    }


    // cancels sprint by setting back flag, and resumes running if still grounded
    public void CancelSprint()
    {
        _targetSpeed = _defaultSpeed;
    }

    // adjusts speed by ramping up or down towards a target based on current sprint flag
    private void CalculateSpeed()
    {
        if (_speed != _targetSpeed)
            _speed = BasicCounter.TowardsTarget(_speed, _targetSpeed, _sprintAcceleration);
        //Debug.Log("Speed: " + _speed);
    }

    public void ApplyRecoil(Vector3 recoilOrigin, float recoilSpeed)
    {
        transform.LookAt(new Vector3(recoilOrigin.x, transform.position.y, recoilOrigin.z));

        // clamps recoil direction to get a minimum XZ recoil, prevents player from getting stuck atop an enemy
        _recoilDirection = new Vector3(transform.position.x, transform.position.y + (_controller.height / 2), transform.position.z) - recoilOrigin;
        float clampX = Mathf.Clamp(Mathf.Abs(_recoilDirection.x), 0.25f, 1f);
        float clampZ = Mathf.Clamp(Mathf.Abs(_recoilDirection.z), 0.25f, 1f);
        _recoilDirection = new Vector3(clampX * (_recoilDirection.x > 0 ? 1f : -1f), _recoilDirection.y, clampZ * (_recoilDirection.z > 0 ? 1f : -1f));

        _verticalVelocity = 0;
        _currentRecoil = recoilSpeed;
        StartRecoil?.Invoke();
    }

    // applies recoil with a basic timer system according to designer-determined deceleration
    private void CalculateRecoil()
    {
        if (_currentRecoil > 0)
        {
            _controller.Move(new Vector3
                (_recoilDirection.x * _currentRecoil, Physics.gravity.y * Time.deltaTime, _recoilDirection.z * _currentRecoil) * Time.deltaTime);

            // _currentRecoil = BasicCounter.TowardsTarget(_currentRecoil, 0, _recoilDecel);
            if (_currentRecoil == 0)
            {
                /*
                if (IsDead)
                    return;
                else
                {
                */
                    _recoilDirection = Vector3.zero;
                    StopRecoil?.Invoke();
                //}
            }
        }            
    }


    // tests for ground using spherecasts
    public bool Grounded()
    {
        if (Physics.SphereCast(transform.position + _controller.center, _controller.height / 6, -transform.up, out RaycastHit hit, 0.83f))
        {
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
}