using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truck : LevelObject, IPossessable
{
    [SerializeField] BasicTrigger _frontTrigger = null;
    [SerializeField] BasicTrigger _backTrigger = null;
    [SerializeField] float _moveSpeed = 10.5f;
    [SerializeField] float _pauseTime = 2f;

    private float _timer = 0.5f;
    private float _moveDirection = 0f;
    private float _speed = 0f;
    private Ghost _possessor = null;

    public override void Activate()
    {
        Debug.Log("Activated");
        _frontTrigger.Activated += FlipDirection;
        _backTrigger.Activated += FlipDirection;
        _frontTrigger.gameObject.SetActive(true);
        _moveDirection = 1f;
    }

    public override void Deactivate()
    {
        Debug.Log("Deactivated");
        _frontTrigger.Activated -= FlipDirection;
        _backTrigger.Activated -= FlipDirection;
        _frontTrigger.gameObject.SetActive(false);
        _backTrigger.gameObject.SetActive(false);
        _moveDirection = 0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        _speed = _moveSpeed;
        Deactivate();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            Activate();
        if (Input.GetKeyDown(KeyCode.X))
            Deactivate();
        PauseTimer();
        MoveTruck();
    }

    void PauseTimer()
    {
        if (_timer > 0)
        {
            _speed = 0;
            _timer -= Time.deltaTime;
            if(_timer <= 0)
            {
                _timer = 0;
                _speed = _moveSpeed;
            }
        }
    }

    public bool Possess(Ghost possessor)
    {
        if(_possessor == null)
        {
            Activate();
            _possessor = possessor;
            Debug.Log("Possess");
            return true;
        }
        return false;
    }

    public bool Unpossess()
    {
        _possessor.ResetGhost();
        _possessor = null;
        return true;
    }

    void FlipDirection(Collider collider)
    {
        Debug.Log("Flipped Direction");
        _moveDirection = -_moveDirection;
        if (_moveDirection > 0)
        {
            _frontTrigger.gameObject.SetActive(true);
            _backTrigger.gameObject.SetActive(false);
        }
        else if (_moveDirection < 0)
        {
            _frontTrigger.gameObject.SetActive(false);
            _backTrigger.gameObject.SetActive(true);
        }

        _timer = _pauseTime;
        Debug.Log("Timer Started");
    }

    void MoveTruck()
    {
        transform.Translate(Vector3.forward * _speed * _moveDirection * Time.deltaTime);
    }
}
