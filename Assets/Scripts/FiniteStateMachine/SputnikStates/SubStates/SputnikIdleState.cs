using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SputnikIdleState : SputnikBindState
{
    public SputnikIdleState(Sputnik sputnik, SputnikStateMachine sputnikStateMachine, SputnikData sputnikData, string animBoolName) : base(sputnik, sputnikStateMachine, sputnikData, animBoolName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (AttackInput == 1)
        {
            sputnikStateMachine.ChangeState(sputnik.AttackState);
        }
        else if (DefenseInput == 1)
        {
            sputnikStateMachine.ChangeState(sputnik.DefenseState);
        }
    }
}
