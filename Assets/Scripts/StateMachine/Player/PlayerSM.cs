using UnityEngine;

public class PlayerSM : StateMachine
{
    [SerializeField] InputController _input = null;
    public InputController Input => _input;
    [SerializeField] PlayerController _player = null;
    public PlayerController Player => _player;
    [SerializeField] Animator _animator = null;
    public Animator Animator => _animator;

    void Start()
    {
        ChangeState<PlayerGroundState>();
    }
}
