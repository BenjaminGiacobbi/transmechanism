using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour, IPossessable
{
    [SerializeField] float _targetRadius = 10f;
    [SerializeField] LayerMask _targetMasks;
    [SerializeField] GameObject _projectile = null;
    [SerializeField] Transform _firePosition = null;
    [SerializeField] bool _possessed = false;
    [SerializeField] ObjectPooler _pooler = null;
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
        }
        else
        {
            _firedOnLast = false;
        }
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
                transform.LookAt(new Vector3(_target.position.x, transform.position.y, _target.position.z));
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
                        Debug.Log(_target);
                    }
                }
            }
        }
    }
}
