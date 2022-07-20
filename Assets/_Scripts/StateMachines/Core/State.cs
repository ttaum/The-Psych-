using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State  
{
    protected Player player;

    protected StateMachine stateMachine;

    protected PlayerData playerData;

    protected bool ShiftInput;

    protected bool isAnimationFinished; 

    protected float startTime;

    private string animBoolName;

    public State(Player player, StateMachine stateMachine,
        PlayerData playerData, string animBoolName) 
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter() 
    {
        DoChecks();

        player.Anim.SetBool(animBoolName, true);
        Debug.Log("Player " + animBoolName);
        isAnimationFinished = false;
    }

    public virtual void Exit()
    {
        player.Anim.SetBool(animBoolName, false);
    }

    public virtual void LogicUpdate() 
    {
        if (InputManager.Instance.PullInput == true 
                && stateMachine.CurrentState != player.SpiritState) 
        {           
            stateMachine.ChangeState(player.SpiritState);
        }

    }

    public virtual void PhysicsUpdate() 
    {
        DoChecks();
    }

    public virtual void DoChecks() 
    {

    }

    public virtual void AnimationTrigger() { }

    public virtual void AnimationFinishTrigger() => isAnimationFinished = true;

}


