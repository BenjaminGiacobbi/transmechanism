using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GeneratorSwitch : MonoBehaviour, IPossessable
{
    public event Action Possessed = delegate { };

    public bool Possess(Ghost possessor)
    {
        throw new System.NotImplementedException();
    }

    public bool Unpossess()
    {
        throw new System.NotImplementedException();
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
