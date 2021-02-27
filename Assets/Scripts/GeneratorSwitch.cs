using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GeneratorSwitch : MonoBehaviour, IPossessable
{
    public event Action Possessed = delegate { };
    private Ghost _possessor = null;

    public bool Possess(Ghost possessor)
    {
        Debug.Log("Possess attempt");
        if(_possessor == null)
        {
            _possessor = possessor;
            Possessed?.Invoke();
            return true;
        }
        return false;
    }

    public bool Unpossess()
    {
        Debug.Log(gameObject.name);
        if(_possessor)
        {
            _possessor.ActiveGhost(true);
            _possessor.ResetGhost(true);
            _possessor = null;
            return true;
        }
        return false;
    }
}
