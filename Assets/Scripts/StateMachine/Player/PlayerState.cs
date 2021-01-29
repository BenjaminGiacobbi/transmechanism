using UnityEngine;

// each state needs to attach to an object with a state machine,
// therefore we can search with GetComponent in Awake
[RequireComponent(typeof(PlayerSM))]
public class PlayerState : State
{
    protected PlayerSM PlayerStates { get; private set; }

    private void Awake()
    {
        PlayerStates = GetComponent<PlayerSM>();
    }
}
