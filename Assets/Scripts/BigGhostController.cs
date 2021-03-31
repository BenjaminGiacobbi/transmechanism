using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BigGhostController : LevelObject
{
    public int BGFlag { get; private set; } = 0;
    [SerializeField] PlayerController _player = null;
    [SerializeField] Ghost _bigGhost = null;
    [SerializeField] BasicTrigger _trig1 = null;
    [SerializeField] BasicTrigger _trig2 = null;
    [SerializeField] Transform _point1 = null;
    [SerializeField] Transform _point2 = null;
    [SerializeField] Transform _point3 = null;

    private void OnEnable()
    {
        _trig1.Activated += IterateFlag;
        _trig2.Activated += IterateFlag;
    }

    private void OnDisable()
    {
        _trig1.Activated -= IterateFlag;
        _trig2.Activated -= IterateFlag;
    }

    public override void Activate()
    {
        BGFlag = 2;
        IterateFlag(null);
    }

    public override void Deactivate()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        _bigGhost.GetComponent<NavMeshAgent>().enabled = false;
        BGFlag = 0;
        CheckGhostState();
    }

    void CheckGhostState()
    {
        switch (BGFlag)
        {
            case 1:
                _bigGhost.transform.position = _point1.position;
                _bigGhost.transform.rotation = _point1.rotation;
                LeanTween.move(_bigGhost.gameObject, _bigGhost.transform.position + _bigGhost.transform.forward * 20, 3f);
                break;
            case 2:
                _bigGhost.transform.position = _point2.position;
                _bigGhost.transform.rotation = _point2.rotation;
                LeanTween.move(_bigGhost.gameObject, _bigGhost.transform.position + _bigGhost.transform.forward * 30, 20f);
                break;
            case 3:
                _bigGhost.transform.position = _point3.position;
                _bigGhost.transform.rotation = _point3.rotation;
                _bigGhost.GetComponent<NavMeshAgent>().enabled = true;
                _bigGhost.DetectRange = 999f;
                _bigGhost.SetTarget(_player.transform);
                break;
        }
    }


    void IterateFlag(Collider collider)
    {
        BGFlag++;
        Debug.Log(BGFlag);
        CheckGhostState();
    }
}
