using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : State
{
    protected float movementInput; // ������ ��� ����� ��������

    protected bool isTouchingCeiling;

    private bool jumpInput; // ������ ��� ����� ������

    protected int crouchInput; 

    protected bool isGrounded;

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

        player.JumpState.ResetAmountOfJumpsLeft(); // ��������� ���������� �������
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        movementInput = player.InputHandler.MovementInput; // ������ ���� �������� �� Player

        jumpInput = player.InputHandler.JumpInput;

        crouchInput = player.InputHandler.CrouchInput;

        if (jumpInput && player.JumpState.CanJump())
        {
            player.InputHandler.UseJumpInput();
            stateMachine.ChangeState(player.JumpState);
        }

        if (!isGrounded)
        {
            player.JumpState.DecreaseAmountOfJumpsLeft();
            stateMachine.ChangeState(player.InAirState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        player.CheckYarn();
    }
}
