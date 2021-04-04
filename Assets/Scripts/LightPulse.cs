using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightPulse : MonoBehaviour
{
    [SerializeField] float _negAdjustment = -0.5f;
    [SerializeField] float _posAdjustment = 0.5f;
    [SerializeField] float _speedModifier = 0.5f;
    float _startVal;

    Light _setLight = null;

    private void Awake()
    {
        _setLight = GetComponent<Light>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _startVal = _setLight.intensity;
        _negAdjustment = _startVal - Mathf.Abs(_negAdjustment);
        _posAdjustment = _startVal + Mathf.Abs(_posAdjustment);
        _startVal = _posAdjustment - _negAdjustment;
    }

    // Update is called once per frame
    void Update()
    {
        float _currentVal = Mathf.PingPong(Time.time*_speedModifier, _startVal);
        _setLight.intensity = _negAdjustment + _currentVal;
    }
}
