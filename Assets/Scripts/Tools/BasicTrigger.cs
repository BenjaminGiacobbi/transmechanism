using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public class BasicTrigger : MonoBehaviour
{
    public event Action<Collider> Activated = delegate { };
    [SerializeField] protected LayerMask _activationLayers;
    [SerializeField] bool _singleUse = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!enabled) return;
        ActivateTrigger(other);
    }

    protected virtual void ActivateTrigger(Collider other)
    {
        if (_activationLayers == (_activationLayers | (1 << other.gameObject.layer)))
        {
            Activated?.Invoke(other);
            if (_singleUse)
            {
                enabled = false;
            }
        }
    }
}