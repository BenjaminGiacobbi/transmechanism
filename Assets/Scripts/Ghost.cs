using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class Ghost : MonoBehaviour
{
    [SerializeField] float _detectRange = 10f;
    public float DetectRange
    { get { return _detectRange; } set { _detectRange = value; } }

    [SerializeField] float _pathUpdateSec = 0.2f;
    [SerializeField] BasicTrigger _trigger = null;
    [SerializeField] int _damage = 5;
    [SerializeField] int _recoilSpeed = 10;
    NavMeshAgent _agent = null;
    Transform _target = null;

    float _timer = 0;
    Vector3 _startPos = Vector3.zero;

    [SerializeField] int _positions = 4;
    Vector3[] _pos = null;

    Coroutine _behaviour = null;

    // caching for components
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        if(_trigger != null)
            _trigger.Activated += OnActive;
    }

    private void OnDisable()
    {
        if (_trigger != null)
            _trigger.Activated -= OnActive;
    }


    // begins enemy pathing
    private void Start()
    {
        /*
        _pos = new Vector3[_positions];
        foreach(Vector3 pos in _pos)
        {
            NavMeshUtil.GetRandomPoint(transform.position, 2);
        }
        */
        _startPos = transform.position;
    }


    private void OnTriggerEnter(Collider other)
    {
        IPossessable possessable = other.gameObject.GetComponent<IPossessable>();
        IRecoil recoil = other.gameObject.GetComponent<IRecoil>();
        IDamageable<int> damageable = other.gameObject.GetComponent<IDamageable<int>>();
        if (possessable != null)
        {
            if (possessable.Possess(this))
            {
                ResetGhost(true);
                ActiveGhost(false);
                // TODO this is an easy bug where the ghost won't be able to find the player if they're in its radius upon return to home
            }  
            else
                return;
        }

        if (recoil != null)
        {
            Vector3 origin = new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y + 1, other.gameObject.transform.position.z);
            Vector3 direction = origin - transform.position;
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, Mathf.Infinity))
            {
                // recoil.ApplyRecoil(hit.point, _recoilSpeed);
            }
        }

        if (damageable != null)
        {
            damageable.Damage(_damage);
            transform.position = _startPos;
            ResetGhost(true);
        }
    }

    private void OnActive(Collider other)
    {

        if (Mathf.Abs(other.transform.position.y - transform.position.y) <= 3)
        {
            Vector3 targetPos = new Vector3(other.transform.position.x, other.transform.position.y + 1, other.transform.position.z);
            if(Physics.Raycast(transform.position, targetPos - transform.position, Mathf.Infinity))
            {
               _target = other.transform;
               _agent.SetDestination(_target.position);
               //EndCoroutine();
                    
            }
        }
    }

    // updates timer and restarts when it hits zero
    private void Update()
    {
        UpdateTarget();
    }

    public void SetTarget(Transform target)
    {
        _target = target;
        _agent.SetDestination(_target.position);
        //EndCoroutine();
    }

    void UpdateTarget()
    {

        if (_timer == 0)
        {
            _timer = _pathUpdateSec;
        }

        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                _timer = 0;

                if(_target)
                {
                    // detects whether object is in range and adjusts pathing accordingly
                    float distance = Vector2.Distance(
                        new Vector2(transform.position.x, transform.position.z), new Vector2(_target.position.x, _target.position.z));
                    if (distance > DetectRange )//|| Mathf.Abs(_target.transform.position.y - transform.position.y) > 3)
                    {
                        ResetGhost(false);
                    }
                    else
                    {
                        _agent.SetDestination(_target.position);
                    }
                }
                else
                {
                    /*
                    if(_idle == null)
                        _idle = StartCoroutine(IdleRoutine());
                        */
                }
            }
        }
    }

    public void ResetGhost(bool kill = false)
    {
        if(kill)
        {
            
            
            if(_behaviour != null)
            {
                StopCoroutine(_behaviour);
                _behaviour = null;
            }
            _behaviour = StartCoroutine(DieRoutine());
        }
        _agent.SetDestination(_startPos);
        _target = null;
    }

    public void ActiveGhost(bool state)
    {
        _agent.enabled = state;
        gameObject.SetActive(state);
    }

    private void EndCoroutine()
    {
        if (_behaviour != null)
        {
            StopCoroutine(_behaviour);
            _behaviour = null;
        }
    }

    IEnumerator DieRoutine()
    {
        _agent.enabled = false;
        transform.position = new Vector3(2000, 2000, 2000);
        yield return new WaitForSeconds(2.5f);
        _agent.enabled = true;
        transform.position = _startPos;
    }

    /*
    IEnumerator IdleRoutine()
    {
        while(true)
        {
            int idx = Random.Range(0, _pos.Length - 1);
            _agent.SetDestination(_pos[idx]);
            while (Vector2.Distance(new Vector2(_agent.destination.x, _agent.destination.z), new Vector2(transform.position.x, transform.position.z)) >= 0.25f)
            {
                if (Vector2.Distance(new Vector2(_agent.destination.x, _agent.destination.z), new Vector2(transform.position.x, transform.position.z)) < 0.25f)
                    break;
                yield return null;
            }

            yield return new WaitForSeconds(2f);
        }
    }
    */
}
