using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine // Нужен для вызова и смены состояний
{
    public State CurrentState { get; private set; }

    public State BooState { get; private set; } // Сюда запишем предыдущее состояние

    public void Initialize(State startingState) // Вызов состояния
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(State newState) // Смена состояния
    {
        BooState = CurrentState;
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.previousState = BooState;
        CurrentState.Enter();
    }
}
