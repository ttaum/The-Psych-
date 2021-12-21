using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    // private float movementInput; ��� �������� ������ � �������
    private bool isGrounded;
    private bool jumpInput;
    private bool jumpInputStop;
    private bool isJumping;
    
    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = player.CheckIfGrounded();
    }

    public override void Enter()
    {
        base.Enter();

        // player.isAirForceAllowed = true; ��� �������� ������ � �������
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // movementInput = player.InputHandler.MovementInput; ��� �������� ������ � �������
        jumpInput = player.InputHandler.JumpInput;
        jumpInputStop = player.InputHandler.JumpInputStop;

        CheckJumpMultiplier();

        if (isGrounded && player.LocalRbVelocity().y < 0.01f) // ������� �������� � ��������� ����������� = �� ����� + �������� ������ 0.01f
        {
            stateMachine.ChangeState(player.LandState);
        }
        else if (jumpInput && player.JumpState.CanJump())
        {
            player.InputHandler.UseJumpInput();
            stateMachine.ChangeState(player.JumpState);
        }
        else
        {
            player.Anim.SetFloat("yVelocity", player.LocalRbVelocity().y); // �������� �������� ������������ �������� ���������

            // �������� ������ � �������

            /*if (player.isAirForceAllowed && movementInput != 0)
            {
                player.SetAirForce(movementInput);
                player.isAirForceAllowed = false;
            }*/
        }
    }

    private void CheckJumpMultiplier() // ��������� ��������� � ������ ���� ����� ������
    {
        if (isJumping)
        {
            if (jumpInputStop)
            {
                player.SetJump(player.LocalRbVelocity().y * playerData.variableJumpHeightMultiplier);
                isJumping = false;
            }
            else if (player.LocalRbVelocity().y <= 0f)
            {
                isJumping = false;
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        player.CheckYarn();
    }

    public void SetIsJumping() => isJumping = true;
}
