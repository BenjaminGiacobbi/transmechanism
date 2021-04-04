using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObject : MonoBehaviour
{
    [SerializeField] protected AudioClip _activationAudio = null;
    [SerializeField] protected AudioClip _deactivationAudio = null;

    public virtual void Activate()
    {
        if (_activationAudio != null)
            AudioHelper.PlayClip3D(_activationAudio, 0.5f, transform);
    }

    public virtual void Deactivate()
    {
        if (_deactivationAudio != null)
            AudioHelper.PlayClip3D(_deactivationAudio, 0.5f, transform);
    }
}
