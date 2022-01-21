using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State  // Базовый класс для состояний игрока
{
    public State previousState; // Для записи предыдущего состояния

    protected Player player;
    
    // protected = private, однако наследники имеют доступ к переменной 

    protected StateMachine stateMachine;

    protected PlayerData playerData;

    protected bool ShiftInput;

    protected bool isAnimationFinished; // Переменная для флага конца анимации

    protected float startTime; // Сюда запишем время когда зашли в состояние

    private string animBoolName; // Название анимации для аниматора

    public State(Player player, StateMachine stateMachine, PlayerData playerData, string animBoolName) 
        // Конструктор для инстанса состояния
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter() // Функция выполняется при вхождении в состояние
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

    public virtual void Exit() // Функция выполняется при выходе из состояния
    {
        if(player.StateMachine.CurrentState == player.SpiritState)
        {
            player.Anim.SetBool(previousState.animBoolName, false);
        }
    }

    public virtual void LogicUpdate() // Выполняется каждый фрейм
    {
        ShiftInput = player.InputHandler.ShiftInput;

        if (ShiftInput && player.StateMachine.CurrentState != player.SpiritState) //Входим в состояние духа
        {
            player.InputHandler.UseShiftInput();

            player.RB.constraints = RigidbodyConstraints2D.FreezeAll;

            stateMachine.ChangeState(player.SpiritState);
        }
    }

    public virtual void PhysicsUpdate() // Выполняется каждый fixedupdate
    {
        DoChecks();
    }

    public virtual void DoChecks() // Выполняем все проверки
    {

    }

    public virtual void AnimationTrigger() { }

    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;

}


