using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    private float input;
    private bool isGrounded;
    
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

        player.isAirForceAllowed = true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        input = player.InputHandler.MovementInput;

        if (isGrounded && player.LocalRbVelocity().y < 0.01f)
        {
            stateMachine.ChangeState(player.LandState);
        }
        else
        {
            player.Anim.SetFloat("yVelocity", player.LocalRbVelocity().y);

            if (player.isAirForceAllowed && input != 0)
            {
                player.SetAirForce(input);
                player.isAirForceAllowed = false;
            }
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        player.CheckYarn();
    }
}
