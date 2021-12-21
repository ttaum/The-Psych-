using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SputnikStateMachine : MonoBehaviour
{
    public SputnikState CurrentState { get; private set; }

    public void Initialize(SputnikState startingState) // Вызов состояния
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(SputnikState newState) // Смена состояния
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
