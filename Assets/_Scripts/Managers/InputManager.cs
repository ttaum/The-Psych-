using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : PersistentSingleton<InputManager>
{

    #region Variables
    public float MovementInput { get; private set; }
    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }
    public int CrouchInput { get; private set; }
    public float BranchInput { get; private set; }
    public bool ShiftInput { get; private set; }
    public Vector2 MouseInput { get; private set; } 
    public bool ActionInput { get; private set; } 
    public bool PullInput { get; private set; }
    public bool InteractionInput { get; private set; } 

    [SerializeField]
    private float inputHoldTime = 0.2f; 

    private float jumpInputStartTime;

    #endregion

    #region Unity Callback Functions

    private void Update()
    {
        CheckJumpInputHoldTime();
    }

    #endregion

    #region Character Inputs

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<float>();      
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpInput = true;
            JumpInputStop = false;
            jumpInputStartTime = Time.time;
        }
        else if (context.canceled)
        {
            JumpInputStop = true;
        }
    }

    private void CheckJumpInputHoldTime()
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

    public void OnBranchInput(InputAction.CallbackContext context)
    {
        BranchInput = context.ReadValue<float>();
    }

    #endregion

    #region Sputnik Inputs
    public void OnMouseInput(InputAction.CallbackContext context)
    {
        MouseInput = context.ReadValue<Vector2>();
    }

    public void OnActionInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ActionInput = true;
        }
        else if (context.canceled)
        {
            ActionInput = false;
        }
    }
    public void OnPullInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PullInput = true;
        }
        else if (context.canceled)
        {
            PullInput = false;
        }
    }

    #endregion

    #region Other Functions
    public void UseJumpInput() => JumpInput = false;
    public void UseShiftInput() => ShiftInput = false;
    public void UseInteractionInput() => InteractionInput = false;
    #endregion

}

