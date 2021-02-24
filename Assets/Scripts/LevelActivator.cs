using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelActivator : MonoBehaviour
{
    [SerializeField] protected LevelObject[] _targetObjects = null;

    public virtual void SendActivation()
    {
        if (_targetObjects.Length > 0)
        {
            foreach(LevelObject lvlObj in _targetObjects)
            {
                lvlObj.Activate();
            }
        }
    }
}
