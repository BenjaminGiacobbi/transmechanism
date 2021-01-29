using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public class BasicTrigger : MonoBehaviour
{
    public event Action<Collider> Activated = delegate { };
    [SerializeField] protected LayerMask _activationLayers;

    private void OnTriggerEnter(Collider other)
    {
        ActivateTrigger(other);
    }

    protected virtual void ActivateTrigger(Collider other)
    {
        if (_activationLayers == (_activationLayers | (1 << other.gameObject.layer)))
        {
            Debug.Log("activated");
            Activated?.Invoke(other);
        }
    }
}