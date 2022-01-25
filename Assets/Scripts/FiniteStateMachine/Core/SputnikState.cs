using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SputnikState // Базовый класс для состояний спутника
{
    protected Sputnik sputnik;

    protected SputnikStateMachine sputnikStateMachine;

    protected SputnikData sputnikData;

    private string animBoolName;

    public SputnikState(Sputnik sputnik, SputnikStateMachine sputnikStateMachine, SputnikData sputnikData, string animBoolName)
    {
        this.sputnik = sputnik;
        this.sputnikStateMachine = sputnikStateMachine;
        this.sputnikData = sputnikData;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter() // Функция выполняется при вхождении в состояние
    {
        DoChecks();
        sputnik.Anim.SetBool(animBoolName, true); 
      //  Debug.Log("Sputnik " + animBoolName);
    }
    public virtual void Exit() // Функция выполняется при выходе из состояние
    {
        sputnik.Anim.SetBool(animBoolName, false);
    }

    public virtual void LogicUpdate() // Выполняется каждый фрейм
    {
        
    }

    public virtual void PhysicsUpdate() // Выполняется каждый fixedupdate
    {
        DoChecks();
    }

    public virtual void DoChecks() // Выполняем все проверки
    {

    }

}
