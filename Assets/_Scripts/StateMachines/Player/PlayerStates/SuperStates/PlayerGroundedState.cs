using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : State
{
    protected bool isGrounded; 

    protected bool isTouchingCeiling;

    public PlayerGroundedState(Player player, StateMachine stateMachine, PlayerData playerData, string animBoolName) :
        base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = player.CheckIfGrounded();
        isTouchingCeiling = player.CheckForCeiling();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (InputManager.Instance.JumpInput)
        {
            InputManager.Instance.UseJumpInput();
            stateMachine.ChangeState(player.JumpState);
        }

        if (!isGrounded)
        {
            stateMachine.ChangeState(player.InAirState);
        }

        if (InputManager.Instance.CrouchInput == 1 && InputManager.Instance.MovementInput == 0)
        {
            stateMachine.ChangeState(player.CrouchIdleState);
        }
        else if (InputManager.Instance.CrouchInput == 1 && InputManager.Instance.MovementInput != 0)
        {
            stateMachine.ChangeState(player.CrouchMoveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        player.SetYarn();
    }
}
