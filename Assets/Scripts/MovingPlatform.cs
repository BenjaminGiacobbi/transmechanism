using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : LevelObject
{
    [SerializeField] MoveDirection _direction;
    [SerializeField] float _moveLength = 5f;
    [SerializeField] float _cycleTime = 3f;
    private float _startCoord = 0;
    private float _endCoord = 0;
    private float _timer = 0;
    private Coroutine _timerRoutine = null;

    public override void Activate()
    {
        if (_timerRoutine == null)
            _timerRoutine = StartCoroutine(TimerRoutine());
    }

    public override void Deactivate()
    {
        if (_timerRoutine != null)
        {
            StopCoroutine(_timerRoutine);
            _timerRoutine = null;
        }   
    }

    // Start is called before the first frame update
    void Start()
    {
        switch (_direction)
        {
            case MoveDirection.PosX: case MoveDirection.NegX:
                _startCoord = transform.position.x;
                break;
            case MoveDirection.PosY: case MoveDirection.NegY:
                _startCoord = transform.position.y;
                break;
            case MoveDirection.PosZ: case MoveDirection.NegZ:
                _startCoord = transform.position.z;
                break;
            default:
                _startCoord = transform.position.x;
                break;
        }

        int sign = 1;
        if (_direction == MoveDirection.NegX || _direction == MoveDirection.NegY || _direction == MoveDirection.NegZ)
            sign = -1;

        _endCoord = _startCoord + sign * _moveLength;
    }

    // Update is called once per frame
    void Update()
    {
        Osscilate();
    }

    private void Osscilate()
    {
        float lerp = Mathf.Lerp(_startCoord, _endCoord, _timer/_cycleTime);
        ApplyAxisPosition(lerp);
    }

    private void ApplyAxisPosition(float value)
    {
        switch (_direction)
        {
            case MoveDirection.PosX: case MoveDirection.NegX:
                transform.position = new Vector3(value, transform.position.y, transform.position.z);
                break;
            case MoveDirection.PosY: case MoveDirection.NegY:
                transform.position = new Vector3(transform.position.x, value, transform.position.z);
                break;
            case MoveDirection.PosZ: case MoveDirection.NegZ:
                transform.position = new Vector3(transform.position.x, transform.position.y, value);
                break;
            default:
                transform.position = new Vector3(value, transform.position.y, transform.position.z);
                break;
        }
    }

    IEnumerator TimerRoutine()
    {
        while(true)
        {
            while (_timer < _cycleTime)
            {
                Debug.Log(_timer);
                _timer += Time.deltaTime;
                if (_timer >= _cycleTime)
                    _timer = _cycleTime;
                yield return null;
            }

            // maybe add a pause here?

            while (_timer > 0)
            {
                Debug.Log(_timer);
                _timer -= Time.deltaTime;
                if (_timer <= 0)
                    _timer = 0;
                yield return null;
            }
        }
    }
}

public enum MoveDirection
{
    PosX, NegX, PosY, NegY, PosZ, NegZ
}
