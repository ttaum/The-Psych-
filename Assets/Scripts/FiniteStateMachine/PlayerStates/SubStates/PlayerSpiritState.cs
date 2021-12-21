using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpiritState : State
{
    public PlayerSpiritState(Player player, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    private bool ShiftInput;

    private bool isGrounded;

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = player.CheckIfGrounded();
    }

    public override void Enter()
    {
        base.Enter();

        player.particleToSputnik.Play();

        player.SetVelocity(0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        ShiftInput = player.InputHandler.ShiftInput;

        if (!ShiftInput)
        {
            stateMachine.ChangeState(player.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        player.ApplyVelocity();

        player.SetRotation(player.CurrentFloatEulerAngles);
    }
}
