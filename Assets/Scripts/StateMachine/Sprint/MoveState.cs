using UnityEngine;

[RequireComponent(typeof(PlayerSM))]
[RequireComponent(typeof(MoveSM))]
public class MoveState : State
{
    protected PlayerSM PlayerStates { get; private set; }
    protected MoveSM MoveStates { get; private set; }

    private void Awake()
    {
        PlayerStates = GetComponent<PlayerSM>();
        MoveStates = GetComponent<MoveSM>();
    }
}
