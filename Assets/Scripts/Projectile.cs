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

    Rigidbody rb = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        OnStart();
        rb.velocity = transform.forward * ProjectileSpeed;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        IRecoil recoil = other.gameObject.GetComponent<IRecoil>();
        IDamageable<int> damageable = other.gameObject.GetComponent<IDamageable<int>>();
        if (recoil != null)
        {
            if (Physics.Raycast(transform.position, other.transform.position - transform.position, out RaycastHit hit, Mathf.Infinity))
                recoil.ApplyRecoil(hit.point, _recoilSpeed);
        }

        if (damageable != null)
        {
            damageable.Damage(_damage);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //TODO object pooling
            Destroy(gameObject, 0.02f);
        }
    }

    protected virtual void OnStart()
    {
        Destroy(gameObject, _lifetime);
        // particle effect on spawn
    }

    protected virtual void OnUpdate()
    {
        
    }
}
