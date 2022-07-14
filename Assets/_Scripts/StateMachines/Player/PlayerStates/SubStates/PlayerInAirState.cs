using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : State
{
    private bool isGrounded;

    private bool isJumping;

    public PlayerInAirState(Player player, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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
    }

    public override void Exit()
    {
        base.Exit();

        player.RB.gravityScale = 3f;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        player.CheckIfFlip(InputManager.Instance.MovementInput);

        HeightJumpMultiplier();

        if (player.LocalRbVelocity().y < 0f)
        {
            player.RB.gravityScale = playerData.falloffGravityScale;
        }

        if (isGrounded && player.LocalRbVelocity().y < 0.01f)
        {
            stateMachine.ChangeState(player.LandState);
        }
        else
        {
            player.Anim.SetFloat("yVelocity", player.LocalRbVelocity().y); 
        }
    }

    private void HeightJumpMultiplier() // Adjustable Jump Height
    {
        if (isJumping)
        {
            if (InputManager.Instance.JumpInputStop)
            {
                player.ApplyJump(player.LocalRbVelocity().y * playerData.variableJumpHeightMultiplier);
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

        player.SetYarn();
    }

    public void SetIsJumping() => isJumping = true;
}
