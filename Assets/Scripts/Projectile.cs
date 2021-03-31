using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] LayerMask _impactLayers;
    [SerializeField] int _damage = 5;
    [SerializeField] float _projectileSpeed = 0.5f;
    [SerializeField] float _lifetime = 3f;
    [SerializeField] float _recoilSpeed = 5f;
    public float ProjectileSpeed
    { get { return _projectileSpeed; } private set { _projectileSpeed = value; } }

    Rigidbody _rb = null;
    ObjectPooler _pooler = null;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Init(ObjectPooler pooler)
    {
        _pooler = pooler;
        Invoke("ReturnPool", _lifetime);
        _rb.velocity = transform.forward * ProjectileSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        IRecoil recoil = other.gameObject.GetComponent<IRecoil>();
        IDamageable<int> damageable = other.gameObject.GetComponent<IDamageable<int>>();
        if (recoil != null)
        {
            Vector3 origin = new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y + 1, other.gameObject.transform.position.z);
            Vector3 direction = origin - transform.position;
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, Mathf.Infinity))
                recoil.ApplyRecoil(hit.normal, _recoilSpeed);
        }

        if (damageable != null)
        {
            damageable.Damage(_damage);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //TODO object pooling
            ReturnPool();
        }
    }

    void ReturnPool()
    {
        if(gameObject.activeSelf)
            _pooler.ReturnToPool("Projectile", gameObject);
    }
}
