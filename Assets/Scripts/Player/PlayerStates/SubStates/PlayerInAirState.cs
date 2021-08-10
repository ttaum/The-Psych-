using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    private int input;
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
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isGrounded && player.LocalRbVelocity().y < 0.01f)
        {
            stateMachine.ChangeState(player.LandState);
        }
        else
        {
            player.Anim.SetFloat("yVelocity", player.LocalRbVelocity().y);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        player.CheckYarn();
    }
}
