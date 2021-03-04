using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    [SerializeField] Transform _pivotPoint = null;
    [SerializeField] float _swingRotation = 60f;
    [SerializeField] float _swingSpeed = 5f;
    private float _baseXRot = 0f;

    // Start is called before the first frame update
    // TODO use quaternions, gimble lock (doesn't interpret values past 360 degrees)
    void Start()
    {
        _baseXRot = transform.eulerAngles.x - _swingRotation / 2;
        transform.RotateAround(_pivotPoint.position, Vector3.right, transform.eulerAngles.x - _baseXRot);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SwingObject();
    }

    void SwingObject()
    {
        float pong = Mathf.PingPong(Time.time * _swingSpeed, Mathf.Abs(_swingRotation));
        float rot = _baseXRot + pong;
        transform.RotateAround(_pivotPoint.position, Vector3.right, rot - transform.eulerAngles.x);
    }
}
