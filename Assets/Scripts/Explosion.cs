using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KillVolume))]
public class Explosion : MonoBehaviour
{
    [SerializeField] float _lifetime = 0.2f;
    private ObjectPooler _pooler = null;

    public void Init(ObjectPooler pooler)
    {
        _pooler = pooler;
        Invoke("ReturnPool", _lifetime);
    }

    void ReturnPool()
    {
        _pooler.ReturnToPool("LightExplosion", gameObject);
    }
}
