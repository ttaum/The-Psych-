using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (movementInput != 0) // Условие перехода в состояние движения
        {
            stateMachine.ChangeState(player.MoveState);
        }
        else if (crouchInput == 1)
        {
            stateMachine.ChangeState(player.CrouchIdleState);
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

        player.SetRotation(player.CurrentFloatEulerAngles);      
    }
}

