using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine // ����� ��� ������ � ����� ���������
{
    public State CurrentState { get; private set; }

    public State BooState { get; private set; } // ���� ������� ���������� ���������

    public void Initialize(State startingState) // ����� ���������
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(State newState) // ����� ���������
    {
        BooState = CurrentState;
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.previousState = BooState;
        CurrentState.Enter();
    }
}
