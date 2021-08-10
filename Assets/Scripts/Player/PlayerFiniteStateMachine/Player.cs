using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }

    public PlayerMoveState MoveState { get; private set; }

    public PlayerJumpState JumpState { get; private set; }

    public PlayerInAirState InAirState { get; private set; }

    public PlayerLandState LandState { get; private set; }


    [SerializeField]
    private PlayerData playerData;

    #endregion

    #region Components
    public Animator Anim { get; private set; }
    public PlayerInputHandler InputHandler { get; private set; }
    public Rigidbody2D RB { get; private set; }

    #endregion

    #region Check Transforms

    [SerializeField]
    private Transform groundCheck;

    #endregion

    #region Other variables


    private Vector3 refVelocity;
    public Vector2 LocalVelocity { get; private set; }

    [Range(0, .01f)] [SerializeField] private float movementSmoothing = .1f;
    [Range(0, 5f)] [SerializeField] private float smoothDampCam = 1f;
    [Range(0, 25f)] [SerializeField] private float smoothDampRotation = 1f;
    [SerializeField] public LayerMask WhatIsYarn;
    [SerializeField] public float YarnCheckDistance;
    [SerializeField] private CinemachineVirtualCamera vcam;

    public bool facingRight = true;

    public Vector2 CurrentVector { get; private set; } = Vector2.down;
    public Vector3 CurrentEuler { get; private set; }

    private  Quaternion CurrentQuat;
    public float CurrentAngleGr { get; private set; } = 0f;
    public float CurrentAngleRad { get; private set; }
    public float SmoothAngle { get; private set; } = 0f;
    #endregion

    #region Unity Callback Functions

    private void Awake()
    {
        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "Jump");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
        LandState = new PlayerLandState(this, StateMachine, playerData, "land");
    }

    private void Start()
    {
        Anim = GetComponent<Animator>();

        InputHandler = GetComponent<PlayerInputHandler>();

        RB = GetComponent<Rigidbody2D>();

        StateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    #endregion

    #region Set functions
    public void SetVelocity(float input)
    {
        LocalVelocity = new Vector2(playerData.movementVelocity * input, 0f);
        Vector2 velocity = transform.TransformDirection(LocalVelocity);

        RB.velocity = Vector3.SmoothDamp(RB.velocity, velocity, ref refVelocity, movementSmoothing);
    }

    public void SetJump()
    {
        Vector2 localJumpForce = new Vector2(0, playerData.jumpForce);
        Vector2 jumpForce = transform.TransformDirection(localJumpForce);

        RB.AddForce(jumpForce, ForceMode2D.Impulse);      
    }

    public Vector2 LocalRbVelocity()
    {
        return transform.InverseTransformDirection(RB.velocity);
    }

    public void SetRotation(float CurrentAngleGr)
    {
        // Euler -> quaternion
        CurrentQuat.eulerAngles = new Vector3(0f, 0f, CurrentAngleGr);

        // rotate Player
        if(Quaternion.Angle(transform.rotation, CurrentQuat) > .5f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, CurrentQuat, smoothDampRotation);
        }
            
        // Rotate camera
        if(Mathf.Abs(SmoothAngle - CurrentAngleGr) > .5f)
        {
            SmoothAngle = Mathf.Lerp(SmoothAngle, CurrentAngleGr, smoothDampCam * Time.fixedDeltaTime);
            vcam.m_Lens.Dutch = SmoothAngle;
        }
        

        // Apply new gravity vector according to normal hit
       // Physics2D.gravity = new Vector3(CurrentVector.x, CurrentVector.y, 0f) * 9.8f;       
    }

    #endregion

    #region Check functions
    public void CheckYarn()
    {
        // Cast a ray from player's position to his feet
        RaycastHit2D hit = Physics2D.Raycast(transform.position, CurrentVector, YarnCheckDistance, WhatIsYarn);

        // If raycast hits Yarn layer -> estimate angle between hit normal and CurrentVector to define
        // the angle to rotate character/gravity/horizontalInput/jumpForce direction.
        if (hit)
        {
            CurrentAngleGr = -Vector2.SignedAngle(hit.normal, Vector2.up);      
                         
            // Define new direction for RaycastHit
            CurrentVector = -hit.normal;

            CurrentAngleRad = CurrentAngleGr * Mathf.PI / 180;

            SetRotation(CurrentAngleGr);

            Physics2D.gravity = new Vector2(CurrentVector.x, CurrentVector.y) * 9.8f;

            Debug.DrawRay(hit.point, hit.normal, Color.green);
        }


    }
    public void CheckIfFlip(float input)
    {
        if (input > 0 && !facingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (input < 0 && facingRight)
        {
            // ... flip the player.
            Flip();
        }
    }

    public bool CheckIfGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }

    #endregion

    #region Other functions

    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimationFinishedTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    #endregion
}
