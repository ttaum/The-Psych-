using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State  // ������� ����� ��� ��������� ������
{

    protected Player player;
    
    // protected = private, ������ ���������� ����� ������ � ���������� 

    protected StateMachine stateMachine;

    protected PlayerData playerData;

    protected bool isAnimationFinished; // ���������� ��� ����� ����� ��������

    protected float startTime; // ���� ������� ����� ����� ����� � ���������

    private string animBoolName; // �������� �������� ��� ���������

    public State(Player player, StateMachine stateMachine, PlayerData playerData, string animBoolName) 
        // ����������� ��� �������� ���������
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter() // ������� ����������� ��� ��������� � ���������
    {
        DoChecks();
        player.Anim.SetBool(animBoolName, true);
        startTime = Time.time;
        //Debug.Log(animBoolName);
        isAnimationFinished = false;
    }

    public virtual void Exit() // ������� ����������� ��� ������ �� ���������
    {
        player.Anim.SetBool(animBoolName, false);
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

    public virtual void AnimationTrigger() { }

    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;


}

