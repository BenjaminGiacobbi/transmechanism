using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : LevelActivator, IPossessable
{
    private bool _possessed = false;
    private Ghost _possessor = null;

    public bool Possess(Ghost possessor)
    {
        if(!_possessed)
        {
            SendActivation();
            _possessed = true;
            _possessor = possessor;
            // code for visual feedback
            return true;
        }
        return false;
    }

    public bool Unpossess()
    {
        if(_possessed)
        {
            _possessor.ActiveGhost(true);
            _possessed = false;
            return true;
        }
        return false;
    }
}
