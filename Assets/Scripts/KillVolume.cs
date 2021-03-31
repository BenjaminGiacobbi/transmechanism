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
            IKillable kill = other.gameObject.GetComponent<IKillable>();
            if(kill != null)
            {
                kill.Kill();
            }
        }
    }
}
