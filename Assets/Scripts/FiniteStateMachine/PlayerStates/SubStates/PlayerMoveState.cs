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

        player.SetVelocity(movementInput * playerData.movementVelocity);

        player.CheckIfFlip(movementInput);
       
        if (movementInput == 0) // Условие перехода в состояние бездействия
        {
            stateMachine.ChangeState(player.IdleState);
        }
        else if (crouchInput == 1)
        {
            stateMachine.ChangeState(player.CrouchMoveState);
        }
        else if (ShiftInput && isGrounded)
        {
            stateMachine.ChangeState(player.SpiritState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        player.ApplyVelocity();

        player.CheckYarn();
    }
}