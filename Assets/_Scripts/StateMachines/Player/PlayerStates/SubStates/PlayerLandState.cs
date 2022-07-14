using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerGroundedState
{
    public PlayerLandState(Player player, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(0f);
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (InputManager.Instance.MovementInput != 0 && (stateMachine.CurrentState != player.JumpState))
        {
            stateMachine.ChangeState(player.MoveState);
        }
        else if (isAnimationFinished)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        player.ApplyRotation(player.CurrentFloatEulerAngles);

        player.ApplyVelocity();
    }
}
