using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightFire : MonoBehaviour
{
    [SerializeField] float _minIntensity = 0;
    [SerializeField] float _maxIntensity = 0;
    [SerializeField] float _updateSeconds = 0.3f;
    Light _lightSource = null;
    Coroutine _routine;

    private void Awake()
    {
        _lightSource = GetComponent<Light>();
    }

    private void Start()
    {
        _routine = StartCoroutine(LightRoutine());
    }

    IEnumerator LightRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(_updateSeconds);
            _lightSource.intensity = Random.Range(_minIntensity, _maxIntensity);
        }
    }
}
