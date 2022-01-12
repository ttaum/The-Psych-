using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SputnikBindState : SputnikState
{
    protected int AttackInput;

    protected int DefenseInput;

    public SputnikBindState(Sputnik sputnik, SputnikStateMachine sputnikStateMachine, SputnikData sputnikData, string animBoolName) : base(sputnik, sputnikStateMachine, sputnikData, animBoolName)
    {
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
 
        sputnik.MousePosition();

        AttackInput = sputnik.InputHandler.AttackInput;

        DefenseInput = sputnik.InputHandler.DefenseInput;

        if (sputnik.player.StateMachine.CurrentState == sputnik.player.SpiritState)
        {
            sputnikStateMachine.ChangeState(sputnik.FreeState);

            sputnik.playerVcam.SetActive(false);
            sputnik.sputnikVcam.SetActive(true);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        sputnik.BindMovement();
    }
}
