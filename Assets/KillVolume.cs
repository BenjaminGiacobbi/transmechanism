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
            IKillable killable = other.gameObject.GetComponent<IKillable>();
            if(killable != null)
            {
                killable.Kill();
            }
        }
    }
}
