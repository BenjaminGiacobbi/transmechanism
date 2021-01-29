using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOffset : MonoBehaviour
{
    [SerializeField] GameObject _player;     // might be better to find a way to search for this instead
    Vector3 cameraOffset;


    // establishes offset between player and camera based on its position in the level view
    private void Start()
    {
        cameraOffset = transform.position - _player.transform.position;
    }


    // applies offset each frame to keep camera position stable
    private void LateUpdate()
    {
        transform.position = _player.transform.position + cameraOffset;
    }
}
