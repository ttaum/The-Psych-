using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Declaration Fields

    public PlayerController2D controller;
    public PlayerInputActions controls;
   // public GameObject shieldObject;

    [Range(0, 60f)] [SerializeField] private float runSpeed = 40f;
    
     
    private float horizontalMove = 0f;  // Variable to store the input value

    // Bool variables to check jump, shield and move conditions
    bool jump = false;
    bool jumpRelease = false;
    bool shieldIsOn = false;
    bool ableToMove = true;

    #endregion

    private void Awake()
    {
        // Define input actions
        controls = new PlayerInputActions();
        controls.Player.Move.performed += ctx => horizontalMove = ctx.ReadValue<float>();
        controls.Player.Jump.started += ctx => jump = true;
        controls.Player.Jump.canceled += ctx => jumpRelease = true;
        // controls.Player.Shield.started += ctx => ShieldActivate();
        // controls.Player.Shield.canceled += ctx => ShieldDisactive();
    }

    private void FixedUpdate()
    {
        // Checking if shields is on (might use shieldIsOn instead of ableToMove)
        if (ableToMove)
            controller.Move(horizontalMove * Time.fixedDeltaTime * runSpeed, jump, jumpRelease);
        else if(!ableToMove)
            controller.Move(0, jump, jumpRelease);

        // Jump switch to false
        jump = false;
    }

    

    private void ShieldActivate()
    {
        // Shield activation function
        shieldIsOn = true;
        ableToMove = false;
       // shieldObject.SetActive(shieldIsOn);
    }

    private void ShieldDisactive()
    {
        // Shield disabling function
        shieldIsOn = false;
        ableToMove = true;
       // shieldObject.SetActive(shieldIsOn);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
