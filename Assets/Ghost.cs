using UnityEngine;
using UnityEngine.AI;

public class Ghost : MonoBehaviour, IKillable
{
    [SerializeField] float _detectRange = 5f, _pathUpdateSec = 0.2f;
    [SerializeField] BasicTrigger _trigger = null;
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

    public void Kill()
    {
        transform.position = _startPos;
        _agent.SetDestination(_startPos);
        _agent.enabled = false;
        gameObject.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Entered");
        Debug.Log(other.gameObject);
        IPossessable possessable = other.gameObject.GetComponent<IPossessable>();
        Debug.Log(possessable);
        if (possessable != null)
        {
            if (possessable.Possess())
            {
                transform.position = _startPos;
                _agent.SetDestination(_startPos);
                _target = null;
                // TODO this is an easy bug where the ghost won't be able to find the player if they're in its radius upon return to home
            }
                
            else
                return;
        }
    }

    private void OnActive(Collider other)
    {
        _target = other.transform;
        _agent.SetDestination(_target.position);
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
                    if (distance > _detectRange)
                    {
                        _agent.SetDestination(_startPos);
                        _target = null;
                    }
                    else
                    {
                        _agent.SetDestination(_target.position);
                    }
                }
            }
        }
    }
}
