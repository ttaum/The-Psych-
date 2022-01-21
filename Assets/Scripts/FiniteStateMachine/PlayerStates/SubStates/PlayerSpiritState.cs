    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpiritState : State
{
    public PlayerSpiritState(Player player, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    private bool isGrounded;

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = player.CheckIfGrounded();
    }

    public override void Enter()
    {
        DoChecks();
        player.Anim.speed = 0f;
        player.particleToSputnik.Play();
        player.sputnikParticles.Play();
    }

    public override void Exit()
    {
        player.sputnikParticles.Stop();
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        float distance = (player.sputnikPosition.position -
            player.transform.position).magnitude;

        if (ShiftInput && distance < player.SpiritEnterRadius) // Выходим из состояния духа если находимся близко
        {
            player.InputHandler.UseShiftInput();

            player.Anim.speed = 1f; // Продолжаем анимации

            player.RB.constraints = RigidbodyConstraints2D.None;
            player.RB.constraints = RigidbodyConstraints2D.FreezeRotation;

            stateMachine.ChangeState(previousState);
        }
        else
        {
            player.InputHandler.UseShiftInput();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        player.CheckYarn();
    }
}
