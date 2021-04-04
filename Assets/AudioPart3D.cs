using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPart3D : MonoBehaviour
{
    string _objectTag = "";
    AudioSource _audio3D = null;
    ObjectPooler _pooler = null;

    private void Awake()
    {
        _audio3D = GetComponent<AudioSource>();
    }

    public void Init(ObjectPooler pooler, float lifeTime, string tag)
    {
        _audio3D.Play();
        _pooler = pooler;
        _objectTag = tag;
        Invoke("ReturnToPool", lifeTime);
    }

    void ReturnToPool()
    {
        _pooler.ReturnToPool(_objectTag, gameObject);
    }
}
