using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    // private float movementInput; Для придания толчка в воздухе
    private bool isGrounded;
    private bool jumpInput;
    private bool jumpInputStop;
    private bool isJumping;
    
    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = player.CheckIfGrounded();
    }

    public override void Enter()
    {
        base.Enter();

        // player.isAirForceAllowed = true; Для придания толчка в воздухе
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // movementInput = player.InputHandler.MovementInput; Для придания толчка в воздухе
        jumpInput = player.InputHandler.JumpInput;
        jumpInputStop = player.InputHandler.JumpInputStop;

        CheckJumpMultiplier();

        if (isGrounded && player.LocalRbVelocity().y < 0.01f) // Условия перехода в состояние приземления = на земле + скорость меньше 0.01f
        {
            stateMachine.ChangeState(player.LandState);
        }
        else if (jumpInput && player.JumpState.CanJump())
        {
            player.InputHandler.UseJumpInput();
            stateMachine.ChangeState(player.JumpState);
        }
        else
        {
            player.Anim.SetFloat("yVelocity", player.LocalRbVelocity().y); // Передача значения вертикальной скорости аниматору

            // Придание толчка в воздухе

            /*if (player.isAirForceAllowed && movementInput != 0)
            {
                player.SetAirForce(movementInput);
                player.isAirForceAllowed = false;
            }*/
        }
    }

    private void CheckJumpMultiplier() // Применяем множитель к прыжку если отжат пробел
    {
        if (isJumping)
        {
            if (jumpInputStop)
            {
                player.SetJump(player.LocalRbVelocity().y * playerData.variableJumpHeightMultiplier);
                isJumping = false;
            }
            else if (player.LocalRbVelocity().y <= 0f)
            {
                isJumping = false;
            }
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        player.CheckYarn();
    }

    public void SetIsJumping() => isJumping = true;
}
