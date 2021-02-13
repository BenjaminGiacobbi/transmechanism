using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCube : MonoBehaviour
{
    [SerializeField] Material _active = null;
    MeshRenderer _mRenderer = null;

    private void Awake()
    {
        _mRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Projectile"))
        {
            _mRenderer.material = _active;
        }
    }
}
