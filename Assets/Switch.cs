using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : LevelActivator, IPossessable
{
    private bool _possessed = false;

    public bool Possess()
    {
        if(!_possessed)
        {
            SendActivation();
            _possessed = true;
            // code for visual feedback
            return true;
        }
        return false;
    }

    public override void SendActivation()
    {
        base.SendActivation();
    }
}
