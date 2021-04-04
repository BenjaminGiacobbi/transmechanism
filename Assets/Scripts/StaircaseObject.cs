using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaircaseObject : LevelObject
{
    [SerializeField] float _zRotation = 25f;
    [SerializeField] Transform _pivotPoint = null;
    private float _storedRotation = 0f;

    public override void Activate()
    {
        base.Activate();
        _storedRotation = transform.rotation.eulerAngles.z + _zRotation;
    }

    public override void Deactivate()
    {
        base.Deactivate();
        _storedRotation = transform.rotation.eulerAngles.z - _zRotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        _storedRotation = transform.rotation.eulerAngles.z;

    }

    // Update is called once per frame
    void Update()
    {
        RotateStaircase();
    }

    void RotateStaircase()
    {
        if (transform.rotation.eulerAngles.z < _storedRotation)
        {
            transform.RotateAround(_pivotPoint.transform.position, -_pivotPoint.transform.forward, 20 * Time.deltaTime);
        }
    }
}
