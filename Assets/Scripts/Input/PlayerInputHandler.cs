using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public float MovementInput { get; private set; } // Переменная для записи ввода движения
    public float BranchInput { get; private set; } // Ввод ветвления
    public bool JumpInput { get; private set; } // Переменная для записи ввода прыжка
    public bool JumpInputStop { get; private set; } // Переменная для снижения скорости прыжка ака регулирование высоты прыжка отжатием
    public int CrouchInput { get; private set; } // Переменная для записи ввода приседания
    public Vector2 MouseInput { get; private set; } // Переменная для записи ввода координат указателя мышки
    public bool ShiftInput { get; private set; } // Переменная для записи состояния сдвига
    public int AttackInput { get; private set; } // Переменная для перехода в состояние атаки
    public int DefenseInput { get; private set; } // Переменная для перехода в состояние защиты
    public bool InteractionInput { get; private set; } // Переменная для взаимодействий

    [SerializeField]
    private float inputHoldTime = 0.2f; // Время которое ввод прыжка "истина" после нажатия

    private float jumpInputStartTime;

    private void Update()
    {
        CheckJumpInputHoldTime();
    }
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<float>(); // Записываем значение передвижения      
    }

    public void OnBranchInput(InputAction.CallbackContext context)
    {
        BranchInput = context.ReadValue<float>();
    }

    public void OnMouseInput(InputAction.CallbackContext context)
    {
        MouseInput = context.ReadValue<Vector2>(); // Записываем значение координат указателя мышки
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpInput = true;
            JumpInputStop = false;
            jumpInputStartTime = Time.time;
        }
        else if(context.canceled)
        {
            JumpInputStop = true;
        }
    }

    private void CheckJumpInputHoldTime() // Таймер после которого ввод прыжка становится "ложь" (используется для более приятного геймплея)
    {
        if (Time.time >= jumpInputStartTime + inputHoldTime)
        {
            JumpInput = false;
        }
    }

    public void OnCrouchInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            CrouchInput = 1;
        }
        else if(context.canceled)
        {
            CrouchInput = 0;
        }
    }

    public void OnShiftInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ShiftInput = !ShiftInput;
        }
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackInput = 1;
        }
        else if (context.canceled)
        {
            AttackInput = 0;
        }

    }

    public void OnDefenseInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DefenseInput = 1;
        }
        else if (context.canceled)
        {
            DefenseInput = 0;
        }
    }

    public void OnInteractionInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            InteractionInput = true;
        }
        else if (context.canceled)
        {
            InteractionInput = false;
        }
    }

    public void UseJumpInput() => JumpInput = false;
    public void UseShiftInput() => ShiftInput = false;
    public void UseInteractionInput() => InteractionInput = false;
}

