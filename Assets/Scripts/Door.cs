using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : LevelObject
{
    [SerializeField] bool _startOpen = false;
    [SerializeField] float _moveSpeed = 5f;
    [SerializeField] Transform _closedPoint = null;
    [SerializeField] Transform _openPoint = null;
    Vector3 _targetPos = Vector3.zero;
    Vector3 _activePos = Vector3.zero;
    Vector3 _inactivePos = Vector3.zero;

    public override void Activate()
    {
        base.Activate();
        _targetPos = _activePos;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        _targetPos = _inactivePos;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!_startOpen)
        {
            _activePos = _openPoint.position;
            _inactivePos = _closedPoint.position;
        }
        else
        {
            _activePos = _closedPoint.position;
            _inactivePos = _openPoint.position;
        }
        _targetPos = _inactivePos;
        transform.position = _targetPos;
    }

    // Update is called once per frame
    void Update()
    {
        MoveDoor();
    }

    void MoveDoor()
    {
        if (transform.position != _targetPos)
        {
            Vector3 dir = _targetPos - transform.position;
            transform.Translate(dir * Time.deltaTime * _moveSpeed);
        }
    }
}
