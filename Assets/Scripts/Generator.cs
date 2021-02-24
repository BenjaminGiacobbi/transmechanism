using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : LevelActivator
{
    [SerializeField] GeneratorSwitch[] _switches = null;

    private void OnEnable()
    {
        foreach(GeneratorSwitch sw in _switches)
        {

        }
    }

    private void OnDisable()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
