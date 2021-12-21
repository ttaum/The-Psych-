using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine // Нужен для вызова и смены состояний
{
    public PlayerState CurrentState { get; private set; }

    public void Initialize(PlayerState startingState) // Вызов состояния
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(PlayerState newState) // Смена состояния
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
