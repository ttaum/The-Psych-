using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{

    public PlayerMoveState(Player player, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
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

        player.SetVelocity(InputManager.Instance.MovementInput * playerData.movementVelocity);

        player.CheckIfFlip(InputManager.Instance.MovementInput);
       
        if (InputManager.Instance.MovementInput == 0)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        player.ApplyVelocity();
    }
}