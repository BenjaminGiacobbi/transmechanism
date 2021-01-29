using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSM : StateMachine
{
    [SerializeField] PlayerController _player = null;
    public PlayerController Player => _player;
    [SerializeField] InputController _input = null;
    public InputController Input => _input;
    [SerializeField] Animator _animator = null;
    public Animator Animator => _animator;

    void Start()
    {
        ChangeState<MoveNoneState>();
    }
}
