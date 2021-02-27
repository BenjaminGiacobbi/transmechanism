using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Generator : LevelActivator
{
    public event Action<float> TimerUpdated = delegate { };
    [SerializeField] GeneratorSwitch[] _switches = null;
    [SerializeField] float _timeLimit = 10f;
    int _generatorCount = 0;
    int _generatorMax = 0;
    float _timer = 0;

    private void OnEnable()
    {
        foreach(GeneratorSwitch sw in _switches)
        {
            sw.Possessed += IterateGenerator;
        }
    }

    private void OnDisable()
    {
        foreach (GeneratorSwitch sw in _switches)
        {
            sw.Possessed -= IterateGenerator;
        }
    }

    private void Start()
    {
        _generatorMax = _switches.Length;
    }

    void IterateGenerator()
    {
        _generatorCount++;
        if(_generatorCount < _generatorMax)
        {
            _timer = _timeLimit;
        }
        else
        {
            _timer = 0;
            SendActivation();
        }
    }

    private void Update()
    {
        ActivationTimer();
    }

    void ActivationTimer()
    {
        if(_timer > 0)
        {
            TimerUpdated?.Invoke(_timer);
            _timer -= Time.deltaTime;
            if(_timer <= 0)
            {
                _timer = 0;
                _generatorCount = 0;
                foreach (GeneratorSwitch sw in _switches)
                {
                    sw.Unpossess();
                }
                TimerUpdated.Invoke(_timer);
            }
        }
    }
}
