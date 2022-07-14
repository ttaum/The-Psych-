using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SputnikFreeState : SputnikState
{
    public SputnikFreeState(Sputnik sputnik, SputnikStateMachine sputnikStateMachine, SputnikData sputnikData, string animBoolName) :
        base(sputnik, sputnikStateMachine, sputnikData, animBoolName)
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

        sputnik.MousePosition();
      
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        sputnik.FreeMovement();
    }
}
