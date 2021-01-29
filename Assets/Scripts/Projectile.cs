using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] LayerMask _impactLayers;
    [SerializeField] float _projectileSpeed = 0.5f;
    [SerializeField] float _lifetime = 3f;
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
