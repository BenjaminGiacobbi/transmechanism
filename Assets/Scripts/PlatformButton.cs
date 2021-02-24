using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LevelTrigger))]
public class PlatformButton : MonoBehaviour
{
    [SerializeField] Material _activeMaterial;
    [SerializeField] MeshRenderer _renderer = null;
    LevelTrigger _trigger = null;

    private void Awake()
    {
        _trigger = GetComponent<LevelTrigger>();
    }

    private void OnEnable()
    {
        _trigger.Sent += ActiveFeedback;
    }

    private void OnDisable()
    {
        _trigger.Sent -= ActiveFeedback;
    }

    void ActiveFeedback()
    {
        _renderer.material = _activeMaterial;
    }

}
