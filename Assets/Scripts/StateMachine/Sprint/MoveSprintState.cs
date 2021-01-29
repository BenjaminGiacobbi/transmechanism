using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSprintState : MoveState
{
    public override void Enter()
    {
        MoveStates.Player.ApplySprint();
        Debug.Log("Enter Sprint");
        MoveStates.Input.Move += OnMove;
        MoveStates.Input.Sprint += ReturnSprint;
    }

    private void OnMove(Vector3 direction)
    {
        if(direction == Vector3.zero)
        {
            MoveStates.ChangeState<MoveNoneState>();
        }
        MoveStates.Player.ApplyMovement(direction);
    }

    private void ReturnSprint(float sprintAxis)
    {
        if (sprintAxis == 0)
        {
            PlayerLandState landState = PlayerStates.CurrentState as PlayerLandState;
            if (landState == null)
                MoveStates.ChangeState<MoveRunState>();
        }  
    }

    public override void Exit()
    {
        Debug.Log("Exit Sprint");
        MoveStates.Input.Move -= OnMove;
        MoveStates.Input.Sprint -= ReturnSprint;
    }
}
