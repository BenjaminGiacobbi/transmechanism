using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerController))]
public class PlayerCharacterAnimator : MonoBehaviour
{
    [Header("Running/Sprinting Feedback")]
    [SerializeField] AudioClip[] _footstepSounds = null;
    [SerializeField] ParticleSystem _movementParticles = null;
    [SerializeField] float _footstepTime = 0.3f;
    [SerializeField] float _sprintStepModifier = 2;

    [Header("Jumping/Landing Feedback")]
    [SerializeField] AudioClip _landSound = null;
    [SerializeField] ParticleSystem _jumpParticles = null;
    [SerializeField] ParticleSystem _trailParticles = null;

    [Header("Damage/Recoil Feedback")]
    [SerializeField] ParticleBase _damageParticle = null;
    [SerializeField] SkinnedMeshRenderer _bodyRenderer = null;
    [SerializeField] Material _damageMaterial = null;
    [SerializeField] float _flashTime = 1f;
    [SerializeField] AudioClip _damageSound = null;
    [SerializeField] AudioClip _deathSound = null;

    // these names are the same as the animation nodes in Mecanim
    const string IdleState = "Idle";
    const string RunState = "Run";
    const string JumpState = "Jumping";
    const string FallState = "Falling";
    const string LandState = "Land";
    const string SprintState = "Sprint";
    const string AbilityState = "Ability";
    const string RecoilState = "Recoil";
    const string DeathState = "Death";

    private float _sprintStepTime = 0f;

    // animator field
    Animator _animator = null;
    PlayerController _movementScript = null;

    Coroutine _damageRoutine = null;
    private Coroutine _footstepRoutine = null;
    private bool _stepRoutineRunning = false;

    int _lastIdx = 0;
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _movementScript = GetComponent<PlayerController>(); // some workflows might have the animator as a child object, so 
        Debug.Log(_footstepSounds.Length);
        foreach(AudioClip sound in _footstepSounds)
        {
            Debug.Log(sound.name);
        }
    }

    #region subscriptions
    private void OnEnable()
    {
        _movementScript.Idle += OnIdle;
        _movementScript.StartRunning += OnStartRunning;
        _movementScript.StartFall += OnStartFalling;
        _movementScript.Land += OnLand;
        _movementScript.StartSprint += OnSprint;
        _movementScript.StartRecoil += OnRecoil;
        _movementScript.Death += OnDeath;
    }

    private void OnDisable()
    {
        _movementScript.Idle -= OnIdle;
        _movementScript.StartRunning -= OnStartRunning;
        _movementScript.StartFall -= OnStartFalling;
        _movementScript.Land -= OnLand;
        _movementScript.StartSprint -= OnSprint;
        _movementScript.StartRecoil -= OnRecoil;
        _movementScript.Death -= OnDeath;
    }
    #endregion


    private void Start()
    {
        _sprintStepTime = _footstepTime / _sprintStepModifier;
    }


    private void OnIdle()
    {
        ClearFeedback();
        _animator.CrossFadeInFixedTime(IdleState, .2f);
    }


    private void OnStartRunning()
    {
        _animator.CrossFadeInFixedTime(RunState, .2f);

        ClearFeedback();
        _footstepRoutine = StartCoroutine(StepRoutine(_footstepTime));

    }


    private void OnSprint()
    {
        _animator.CrossFadeInFixedTime(SprintState, .2f);

        ClearFeedback();
        _footstepRoutine = StartCoroutine(StepRoutine(_sprintStepTime));
    }

    private void OnLand()
    {
        _animator.Play(LandState);
        _jumpParticles.Play();
        _trailParticles.Stop();
        AudioHelper.PlayClip2D(_landSound, 0.35f);
    }


    private void OnStartFalling()
    {
        _animator.CrossFadeInFixedTime(FallState, .2f);
    }

    private void OnRecoil()
    {
        _animator.Play(RecoilState);
        ClearFeedback();
        _trailParticles.Play();

        if (_damageRoutine == null)
        {
            _damageRoutine = StartCoroutine(FlashRoutine());
            if (_damageParticle != null)
                _damageParticle.PlayComponents();
            if (_damageSound != null)
            AudioHelper.PlayClip2D(_damageSound, 0.75f);
        }
    }


    private void OnDeath()
    {
        _animator.CrossFadeInFixedTime(DeathState, .2f);
        if (_deathSound != null)
            AudioHelper.PlayClip2D(_deathSound, 0.5f);
        ClearFeedback();
    }


    private void ClearFeedback()
    {
        if (_footstepRoutine != null)
            StopCoroutine(_footstepRoutine);
        _trailParticles.Stop();
    }


    IEnumerator StepRoutine(float stepDelay)
    {
        _lastIdx = 0;
        while (true)
        {
            yield return new WaitForSeconds(stepDelay);

            _movementParticles.Play();
            int rand = 0;
            do
            {
                rand = Random.Range(0, _footstepSounds.Length);
            }
            while (rand == _lastIdx);
            AudioHelper.PlayClip2D(_footstepSounds[rand], 0.01f);
            _lastIdx = rand;
            
        }
    }


    // simple flash stuff
    IEnumerator FlashRoutine()
    {
        Material tempMaterial = _bodyRenderer.material;
        _bodyRenderer.material = _damageMaterial;

        yield return new WaitForSeconds(_flashTime);

        _bodyRenderer.material = tempMaterial;
        _damageRoutine = null;
    }
}
