using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillVolume : BasicTrigger
{
    private void OnTriggerEnter(Collider other)
    {
        ActivateTrigger(other);
    }

    protected override void ActivateTrigger(Collider other)
    {
        if (_activationLayers == (_activationLayers | (1 << other.gameObject.layer)))
        {
            Ghost ghost = other.gameObject.GetComponent<Ghost>();
            if(ghost != null)
            {
                ghost.ResetGhost(true);
            }
        }
    }
}
