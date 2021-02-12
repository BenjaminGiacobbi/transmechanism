using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public override void Enter()
    {
        Debug.Log("Enter Jump");
    }

    /*
    public override void Tick()
    {
        PlayerStates.Player.ApplyAirGravity();
        if (PlayerStates.Player.Grounded())
            PlayerStates.ChangeState<PlayerLandState>();
    }
    */

    public override void Exit()
    {
        Debug.Log("Exit Jump");
    }
}
