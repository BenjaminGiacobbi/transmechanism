using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNoneState : MoveState
{
    bool sprint = false;

    public override void Enter()
    {
        
        MoveStates.Player.SetIdle();
        MoveStates.Input.Move += StartMove;
        MoveStates.Input.Sprint += HandleSprint;
    }

    private void StartMove(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            PlayerLandState landState = PlayerStates.CurrentState as PlayerLandState;
            if(landState == null)
            {
                if(sprint)
                    MoveStates.ChangeState<MoveSprintState>();
                else
                    MoveStates.ChangeState<MoveRunState>();
            }
        }          
    }

    private void HandleSprint(float sprintAxis)
    {
        sprint = (sprintAxis > 0 ? true : false);
    }

    public override void Exit()
    {
        Debug.Log("Exit Idle");
        MoveStates.Input.Move -= StartMove;
        MoveStates.Input.Sprint -= HandleSprint;
    }
}
