using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockHazard : LevelObject
{
    [SerializeField] GameObject[] _hazardObjects = null;
    [SerializeField] Material _inactiveMaterial = null;
    [SerializeField] Material _activeMaterial = null;

    public override void Activate()
    {
        foreach (GameObject obj in _hazardObjects)
        {
            Damage damage = obj.GetComponent<Damage>();
            if (damage != null)
                damage.enabled = true;
            MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
            if (renderer != null)
                renderer.material = _activeMaterial;
            Collider collider = obj.GetComponent<Collider>();
            if (collider != null)
                collider.isTrigger = true;
        }
    }

    public override void Deactivate()
    {
        foreach(GameObject obj in _hazardObjects)
        {
            Damage damage = obj.GetComponent<Damage>();
            if (damage != null)
                damage.enabled = false;
            MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
            if (renderer != null)
                renderer.material = _inactiveMaterial;
            Collider collider = obj.GetComponent<Collider>();
            if (collider != null)
                collider.isTrigger = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Deactivate();
    }
}
