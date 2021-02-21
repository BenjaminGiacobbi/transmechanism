using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelActivator : MonoBehaviour
{
    [SerializeField] protected LevelObject _targetObject = null;

    public virtual void SendActivation()
    {
        if (_targetObject != null)
            _targetObject.Activate();
    }
}
