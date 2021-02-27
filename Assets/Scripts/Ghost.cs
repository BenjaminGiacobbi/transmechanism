using UnityEngine;
using UnityEngine.AI;

public class Ghost : MonoBehaviour
{
    [SerializeField] float _detectRange = 5f, _pathUpdateSec = 0.2f;
    [SerializeField] BasicTrigger _trigger = null;
    [SerializeField] int _damage = 5;
    [SerializeField] int _recoilSpeed = 10;
    NavMeshAgent _agent = null;
    Transform _target = null;

    float _timer = 0;
    bool playerInRange = false;
    Vector3 _startPos = Vector3.zero;

    // caching for components
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        _trigger.Activated += OnActive;
    }

    private void OnDisable()
    {
        _trigger.Activated -= OnActive;
    }


    // begins enemy pathing
    private void Start()
    {
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
            if (Physics.Raycast(transform.position, other.transform.position - transform.position, out RaycastHit hit, Mathf.Infinity))
            {
                recoil.ApplyRecoil(hit.point, _recoilSpeed);
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

        if (Mathf.Abs(other.transform.position.y - transform.position.y) <= 2)
        {
            Vector3 targetPos = new Vector3(other.transform.position.x, other.transform.position.y + 1, other.transform.position.z);
            if(Physics.Raycast(transform.position, targetPos - transform.position, Mathf.Infinity))
            {
               _target = other.transform;
                _agent.SetDestination(_target.position);
            }
        }
    }

    // updates timer and restarts when it hits zero
    private void Update()
    {
        UpdateTarget();
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
                    if (distance > _detectRange || Mathf.Abs(_target.transform.position.y - transform.position.y) > 2)
                    {
                        ResetGhost(true);
                    }
                    else
                    {
                        _agent.SetDestination(_target.position);
                    }
                }
            }
        }
    }

    public void ResetGhost(bool instant = false)
    {
        if(instant)
            transform.position = _startPos;
        _agent.SetDestination(_startPos);
        _target = null;
    }

    public void ActiveGhost(bool state)
    {
        _agent.enabled = state;
        gameObject.SetActive(state);
    }
}
