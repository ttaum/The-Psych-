using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public float MovementInput { get; private set; } // Переменная для записи ввода движения
    public bool JumpInput { get; private set; } // Переменная для записи ввода прыжка
    public bool JumpInputStop { get; private set; } // Переменная для снижения скорости прыжка ака регулирование высоты прыжка отжатием
    public int CrouchInput { get; private set; } // Переменная для записи ввода приседания
    public Vector2 MouseInput { get; private set; } // Переменная для записи ввода координат указателя мышки
    public bool ShiftInput { get; private set; } // Переменная для записи состояния сдвига

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
        if (context.canceled)
        {
            JumpInputStop = true;
        }
    }

    public void UseJumpInput() => JumpInput = false;

    private void CheckJumpInputHoldTime() // Таймер после которого ввод прыжка становится "ложь" (используется для более приятного геймплея)
    {
        if(Time.time >= jumpInputStartTime + inputHoldTime)
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

        if (context.canceled)
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
}
