using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State  // ������� ����� ��� ��������� ������
{
    public State previousState; // ��� ������ ����������� ���������

    protected Player player;
    
    // protected = private, ������ ���������� ����� ������ � ���������� 

    protected StateMachine stateMachine;

    protected PlayerData playerData;

    protected bool ShiftInput;

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
        if(previousState != null && previousState != player.SpiritState)
        {
            player.Anim.SetBool(previousState.animBoolName, false);
        } 
        startTime = Time.time;
        Debug.Log("Player " + animBoolName);
        player.Anim.SetBool(animBoolName, true);
        isAnimationFinished = false;
    }

    public virtual void Exit() // ������� ����������� ��� ������ �� ���������
    {
        if(player.StateMachine.CurrentState == player.SpiritState)
        {
            player.Anim.SetBool(previousState.animBoolName, false);
        }
    }

    public virtual void LogicUpdate() // ����������� ������ �����
    {
        ShiftInput = player.InputHandler.ShiftInput;

        if (ShiftInput && player.StateMachine.CurrentState != player.SpiritState) //������ � ��������� ����
        {
            player.InputHandler.UseShiftInput();

            player.RB.constraints = RigidbodyConstraints2D.FreezeAll;

            stateMachine.ChangeState(player.SpiritState);
        }
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


