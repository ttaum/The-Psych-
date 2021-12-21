using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public float MovementInput { get; private set; } // ���������� ��� ������ ����� ��������
    public bool JumpInput { get; private set; } // ���������� ��� ������ ����� ������
    public bool JumpInputStop { get; private set; } // ���������� ��� �������� �������� ������ ��� ������������� ������ ������ ��������
    public int CrouchInput { get; private set; } // ���������� ��� ������ ����� ����������
    public Vector2 MouseInput { get; private set; } // ���������� ��� ������ ����� ��������� ��������� �����

    [SerializeField]
    private float inputHoldTime = 0.2f; // ����� ������� ���� ������ "������" ����� �������

    private float jumpInputStartTime;

    private void Update()
    {
        CheckJumpInputHoldTime();
    }
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<float>(); // ���������� �������� ������������      
    }

    public void OnMouseInput(InputAction.CallbackContext context)
    {
        MouseInput = context.ReadValue<Vector2>(); // ���������� �������� ��������� ��������� �����
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

    private void CheckJumpInputHoldTime() // ������ ����� �������� ���� ������ ���������� "����" (������������ ��� ����� ��������� ��������)
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
}
