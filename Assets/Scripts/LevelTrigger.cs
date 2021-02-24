using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelTrigger : LevelActivator
{
    public event Action Sent = delegate { };
    [SerializeField] LayerMask _activationLayers;
    bool _activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!_activated && (_activationLayers == (_activationLayers | 1 << other.gameObject.layer)))
        {
            SendActivation();
            Sent?.Invoke();
            _activated = true;
        }
    }
}
