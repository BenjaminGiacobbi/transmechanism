using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerState
{
    [SerializeField] float _landTime;
    float timer = 0;

    public override void Enter()
    {
        Debug.Log("Enter Land");
        timer = _landTime;
    }

    public override void Tick()
    {
        if (timer > 0)
            BasicCounter.TowardsTarget(timer, 0, 1);
        if (timer == 0)
            PlayerStates.ChangeState<PlayerGroundState>();
    }

    public override void Exit()
    {
        Debug.Log("Enter Ground");
    }
}
