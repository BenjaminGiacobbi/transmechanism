﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerController))]
public class PlayerCharacterAnimator : MonoBehaviour
{
    [Header("Running/Sprinting Feedback")]
    [SerializeField] AudioClip _footstepSound = null;
    [SerializeField] ParticleSystem _movementParticles = null;
    [SerializeField] float _footstepTime = 0.3f;
    [SerializeField] float _sprintStepModifier = 2;

    [Header("Jumping/Landing Feedback")]
    [SerializeField] AudioClip _jumpSound = null;
    [SerializeField] AudioClip _landSound = null;
    [SerializeField] ParticleSystem _jumpParticles = null;
    [SerializeField] ParticleSystem _trailParticles = null;

    [Header("Damage/Recoil Feedback")]
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
    // AbilityLoadout _abilityScript = null;

    Coroutine _damageRoutine = null;


    private Coroutine _footstepRoutine = null;
    private bool _stepRoutineRunning = false;
    /*
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _movementScript = GetComponent<PlayerController>(); // some workflows might have the animator as a child object, so 
        // _abilityScript = GetComponent<AbilityLoadout>();       // keep in mind that this might be filled as an inspector reference
    }

    #region subscriptions
    private void OnEnable()
    {
        _movementScript.Idle += OnIdle;
        _movementScript.StartRunning += OnStartRunning;
        _movementScript.StartJump += OnStartJump;
        _movementScript.StartFall += OnStartFalling;
        _movementScript.Land += OnLand;
        _movementScript.StartSprint += OnSprint;
        _movementScript.Ability += OnAbility;
        _movementScript.StartRecoil += OnRecoil;
        _movementScript.Death += OnDeath;
    }

    private void OnDisable()
    {
        _movementScript.Idle -= OnIdle;
        _movementScript.StartRunning -= OnStartRunning;
        _movementScript.StartJump -= OnStartJump;
        _movementScript.StartFall -= OnStartFalling;
        _movementScript.Land -= OnLand;
        _movementScript.StartSprint -= OnSprint;
        _movementScript.Ability -= OnAbility;
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


    private void OnStartJump()
    {
        ClearFeedback();

        _animator.Play(JumpState);
        
        _jumpParticles.transform.localEulerAngles = new Vector3
            (0, _jumpParticles.transform.localEulerAngles.y, _jumpParticles.transform.localEulerAngles.z);
        _jumpParticles.Play();
        _trailParticles.Play();
        // AudioHelper.PlayClip2D(_jumpSound, 0.45f);
    }


    private void OnLand()
    {
        _animator.Play(LandState);

        _jumpParticles.transform.localEulerAngles = new Vector3
            (180, _jumpParticles.transform.localEulerAngles.y, _jumpParticles.transform.localEulerAngles.z);
        _jumpParticles.Play();
        _trailParticles.Stop();
        // AudioHelper.PlayClip2D(_landSound, 0.35f);
    }


    private void OnStartFalling()
    {
        _animator.CrossFadeInFixedTime(FallState, .2f);
    }


    private void OnAbility()
    {
        _animator.CrossFadeInFixedTime(AbilityState, .2f);
        ClearFeedback();
    }


    private void OnRecoil()
    {
        _animator.Play(RecoilState);
        ClearFeedback();
        _trailParticles.Play();

        if (_damageRoutine == null)
        {
            _damageRoutine = StartCoroutine(FlashRoutine());
            if (_damageSound != null)
            {

            }
                // AudioHelper.PlayClip2D(_damageSound, 0.75f);
        }
            
    }


    private void OnDeath()
    {
        _animator.CrossFadeInFixedTime(DeathState, .2f);
        if (_deathSound != null)
            // AudioHelper.PlayClip2D(_deathSound, 0.5f);
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
        while(true)
        {
            yield return new WaitForSeconds(stepDelay);

            _movementParticles.Play();
            // AudioHelper.PlayClip2D(_footstepSound, 0.4f);
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
    */
}
