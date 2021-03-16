using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrap : MonoBehaviour
{
    [SerializeField] string _poolTag = "";
    public string PoolTag { get { return _poolTag; } private set { _poolTag = value; } }

    [SerializeField] BasicTrigger _trigger = null;
    private ObjectPooler _pooler = null;

    // TODO this has no lifetime, so technically it would spawn these infinitely until the game crashed
    public void Init(ObjectPooler pooler)
    {
        _pooler = pooler;
    }

    private void OnEnable()
    {
        _trigger.Activated += Impact;
    }

    private void OnDisable()
    {
        _trigger.Activated -= Impact;
    }

    void Impact(Collider collider)
    {
        GameObject explosion = _pooler.SpawnObject("LightExplosion", null, transform.position, Quaternion.identity);
        explosion.GetComponent<Explosion>().Init(_pooler);
        _pooler.ReturnToPool(_poolTag, gameObject);
    }
}
