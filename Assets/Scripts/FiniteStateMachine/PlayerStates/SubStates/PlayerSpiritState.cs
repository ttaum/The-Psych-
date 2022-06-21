    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpiritState : State
{
    public PlayerSpiritState(Player player, StateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
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


        if (ShiftInput && player.SputnikDistance() < player.SpiritEnterRadius) 
        {
            player.InputHandler.UseShiftInput();

            player.RB.constraints = RigidbodyConstraints2D.None;
            player.RB.constraints = RigidbodyConstraints2D.FreezeRotation;

            stateMachine.ChangeState(player.IdleState);
        }
        else
        {
            player.InputHandler.UseShiftInput();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        player.SetYarn();
    }
}
