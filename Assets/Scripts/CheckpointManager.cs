using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] PlayerHealth _player = null;
    [SerializeField] BasicTrigger[] _checkpoints = null;
    Vector3 _respawnPosition = Vector3.zero;

    private void OnEnable()
    {
        foreach(BasicTrigger point in _checkpoints)
        {
            point.Activated += SetRespawnPoint;
        }
        _player.Died += ResetPlayer;
    }

    private void OnDisable()
    {
        foreach (BasicTrigger point in _checkpoints)
        {
            point.Activated -= SetRespawnPoint;
        }
        _player.Died -= ResetPlayer;
    }

    // Start is called before the first frame update
    void Start()
    {
        _respawnPosition = _player.transform.position;
    }

    void SetRespawnPoint(Collider collider)
    {
        _respawnPosition = collider.transform.position;
        Debug.Log("Respawn Position: " + collider.transform.position);
    }

    public void ResetPlayer()
    {
        CharacterController controller = _player.GetComponent<CharacterController>();
        if(controller != null)
        {
            controller.enabled = false;
            _player.transform.position = _respawnPosition;
            controller.enabled = true;
        }
    }
}
