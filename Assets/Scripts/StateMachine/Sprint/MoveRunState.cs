using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRunState : MoveState
{
    /*
    public override void Enter()
    {
        MoveStates.Player.CancelSprint();
        Debug.Log("Enter Run");
        MoveStates.Input.Move += OnMove;
        MoveStates.Input.Sprint += OnSprint;
    }

    private void OnMove(Vector3 direction)
    {
        if (direction == Vector3.zero)
        {
            MoveStates.ChangeState<MoveNoneState>();
            return;
        }
        MoveStates.Player.ApplyMovement(direction);
    }

    private void OnSprint(float sprintAxis)
    {
        if (sprintAxis > 0)
            MoveStates.ChangeState<MoveSprintState>();
    }

    public override void Exit()
    {
        Debug.Log("Exit Run");
        MoveStates.Input.Move -= OnMove;
        MoveStates.Input.Sprint -= OnSprint;
    }
    */
}
