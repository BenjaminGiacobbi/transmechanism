using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour, IPossessable
{
    [SerializeField] GameObject _gunSpin = null;
    [SerializeField] float _targetRadius = 10f;
    [SerializeField] float _rotateSpeed = 2f;
    [SerializeField] LayerMask _targetMasks;
    [SerializeField] GameObject _projectile = null;
    [SerializeField] Transform _firePosition = null;
    [SerializeField] bool _possessed = false;
    [SerializeField] ObjectPooler _pooler = null;
    [SerializeField] ParticleBase _dischargeParticle = null;
    public bool Possessed
    { get { return _possessed; } private set { _possessed = value; } }

    private Transform _target = null;
    private float _timer = 0;
    private bool _firedOnLast = false;

    //
    public bool Possess(Ghost possessor)
    {
        if (!Possessed)
        {
            Possessed = true;
            return Possessed;
        }
        return false;
    }

    private void Start()
    {
        if(_dischargeParticle != null)
        {
            _dischargeParticle.transform.position = _firePosition.position;
            _dischargeParticle.transform.rotation = _firePosition.rotation;
        }
    }

    // no unpossess behaviour currently because it's only used once with a straight use case
    public bool Unpossess()
    {
        throw new System.NotImplementedException();
    }


    // Update is called once per frame
    void Update()
    {
        IterateTimer();
        TargetObject();
        FireProjectile();
    }

    private void IterateTimer()
    {
        _timer += Time.deltaTime;
    }

    private void FireProjectile()
    {
        if (Mathf.Round(_timer * 10) / 10 % 1 == 0 && !_firedOnLast)
        {
            GameObject newProjectile = _pooler.SpawnObject("Projectile", null, _firePosition.position, _firePosition.rotation);
            newProjectile.GetComponent<Projectile>().Init(_pooler);
            _firedOnLast = true;
            _timer += 0.1f;
            if (_dischargeParticle != null)
                _dischargeParticle.PlayComponents();
        }
        else
        {
            _firedOnLast = false;
        }
        _gunSpin.transform.Rotate(
            new Vector3(_gunSpin.transform.localEulerAngles.x, _gunSpin.transform.localEulerAngles.y, _gunSpin.transform.localEulerAngles.z+1));
    }

    private void TargetObject()
    {
        if (Possessed)
        {
            if (_target)
            {
                float distance = Vector2.Distance(new Vector2(_target.position.x, _target.position.z), new Vector2(transform.position.x, transform.position.z));
                if (distance > _targetRadius)
                {
                    _target = null;
                    return;
                }

                // gain target bearing and rotate toards
                Vector3 direction = new Vector3
                    (_target.position.x - transform.position.x, transform.position.y, _target.position.z - transform.position.z);
                float step = _rotateSpeed * Time.deltaTime;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, step, 0.0f);
                transform.rotation = Quaternion.LookRotation(newDirection);
            }
            else
            {
                if ((Mathf.Round(_timer * 10) / 10) % 0.5 == 0)
                {
                    Collider[] colliderSearch = Physics.OverlapSphere(transform.position, _targetRadius, 1 << 9);

                    // TODO this could technically mess up if the mask has multiple layers or there are multiple player colliders
                    if (colliderSearch.Length > 0)
                    {
                        _target = colliderSearch[0].gameObject.transform;
                    }
                }
            }
        }
    }
}
