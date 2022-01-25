using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SputnikState // ������� ����� ��� ��������� ��������
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

    public virtual void Enter() // ������� ����������� ��� ��������� � ���������
    {
        DoChecks();
        sputnik.Anim.SetBool(animBoolName, true); 
      //  Debug.Log("Sputnik " + animBoolName);
    }
    public virtual void Exit() // ������� ����������� ��� ������ �� ���������
    {
        sputnik.Anim.SetBool(animBoolName, false);
    }

    public virtual void LogicUpdate() // ����������� ������ �����
    {
        
    }

    public virtual void PhysicsUpdate() // ����������� ������ fixedupdate
    {
        DoChecks();
    }

    public virtual void DoChecks() // ��������� ��� ��������
    {

    }

}
