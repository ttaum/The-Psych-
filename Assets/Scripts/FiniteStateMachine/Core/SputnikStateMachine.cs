using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SputnikStateMachine : MonoBehaviour
{
    public SputnikState CurrentState { get; private set; }

    public void Initialize(SputnikState startingState) // ����� ���������
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(SputnikState newState) // ����� ���������
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
