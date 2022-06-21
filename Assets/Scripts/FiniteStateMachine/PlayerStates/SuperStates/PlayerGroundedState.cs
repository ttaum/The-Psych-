using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : State
{
    protected bool isGrounded; 

    protected bool isTouchingCeiling;

    protected float movementInput; 

    private bool jumpInput;

    protected int crouchInput; 

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

        movementInput = player.InputHandler.MovementInput; 

        jumpInput = player.InputHandler.JumpInput;

        crouchInput = player.InputHandler.CrouchInput;

        if (jumpInput)
        {
            player.InputHandler.UseJumpInput();
            stateMachine.ChangeState(player.JumpState);
        }

        if (!isGrounded)
        {
            stateMachine.ChangeState(player.InAirState);
        }

        if (crouchInput == 1 && movementInput == 0)
        {
            stateMachine.ChangeState(player.CrouchIdleState);
        }
        else if (crouchInput == 1 && movementInput != 0)
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
