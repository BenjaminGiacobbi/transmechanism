using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    /*
    float frameCounter;

    public override void Enter()
    {
        Debug.Log("Enter Ground");
        PlayerStates.Input.Jump += OnJump;
        frameCounter = 0;
    }

    public override void Tick()
    {
        frameCounter++;
        if (frameCounter == 2 && !PlayerStates.Player.Grounded())
        {
            PlayerStates.ChangeState<PlayerAirState>();
            frameCounter = 0;
        }
    }

    private void OnJump(float jumpValue)
    {
        if(jumpValue > 0)
        {
            PlayerStates.Player.ApplyJump(jumpValue);
            PlayerStates.ChangeState<PlayerAirState>();
        }
    }

    public override void Exit()
    {
        Debug.Log("Exit Ground");
        PlayerStates.Input.Jump -= OnJump;
    }
    */
}
