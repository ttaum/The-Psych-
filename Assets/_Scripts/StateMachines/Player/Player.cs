using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : Singleton<Player>
{
    #region State Variables 

    // State machine instances
    public StateMachine StateMachine { get; private set; }
    public PlayerIdleState IdleState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerInAirState InAirState { get; private set; }
    public PlayerLandState LandState { get; private set; }
    public PlayerCrouchIdleState CrouchIdleState { get; private set; }
    public PlayerCrouchMoveState CrouchMoveState { get; private set; }
    public PlayerSpiritState SpiritState { get; private set; }

    #endregion

    #region Components
    public Animator Anim { get; private set; } 
    public Rigidbody2D RB { get; private set; }
    public CapsuleCollider2D MovementCollider { get; private set; }
    #endregion

    #region Objects

    [Header("Objects")]

    [SerializeField]
    private PlayerData playerData;

    [SerializeField]
    private GameObject input;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private Transform ceilingCheck;

    [SerializeField]
    private Transform sputnikTransform;
    public float SpiritEnterRadius { get; private set; } = 5.0f;


    // Particles 
    public ParticleSystem particleToSputnik;
    public ParticleSystem sputnikParticles;
    public ParticleSystem deathParticles; 

    #endregion

    #region Other variables

    private Vector2 localVelocity; // Local speed  
    private Vector2 globalVelocity; // Global speed
    private Vector3 refVelocity; // technical value for smoothDamp func
    private Vector2 globalJumpForce; // Global value for jump

    private Vector2 workspace;

    [Header("Smoothing and damping")]

    [Range(0, .5f)] [SerializeField] private float movementSmoothing = .015f;
    [Range(0, 25f)] [SerializeField] private float smoothDampRotation = 1f;

    [Header("Other")]

    [SerializeField] private LayerMask WhatIsYarn;
    [SerializeField] private float YarnCheckDistance;


    // Technical  values

    private bool facingRight = true;

    public float CurrentFloatEulerAngles { get; private set; }
    private Vector2 CurrentVector  = Vector2.down;
    private Vector2 OldCurrentVector = Vector2.down;
    private Quaternion currentQuat;
    private Vector3 currentVectorEulerAngles;
    
    private Vector2 gravityVector;

    #endregion

    #region Unity Callback Functions

    private void OnEnable()
    {
       // EventManager.DeathEvent += DeathTrigger;
    }
    protected override void Awake()
    {
        base.Awake();
       
        StateMachine = new StateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
        MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
        JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
        InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
        LandState = new PlayerLandState(this, StateMachine, playerData, "land");
        CrouchIdleState = new PlayerCrouchIdleState(this, StateMachine, playerData, "crouchIdle");
        CrouchMoveState = new PlayerCrouchMoveState(this, StateMachine, playerData, "crouchMove");
        SpiritState = new PlayerSpiritState(this, StateMachine, playerData, "spirit");
    }

    private void Start()
    {
        Anim = GetComponent<Animator>();
        RB = GetComponent<Rigidbody2D>();
        MovementCollider = GetComponent<CapsuleCollider2D>();

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

    private void OnDisable()
    {
      //  EventManager.DeathEvent -= DeathTrigger;
    }

    #endregion

    #region Set and Apply functions

    // Recalculation movement from local to global (logic)
    public void SetVelocity(float movementInput)
    {
        localVelocity = transform.InverseTransformDirection(RB.velocity);
        localVelocity.Set(movementInput, localVelocity.y);
        globalVelocity = transform.TransformDirection(localVelocity);      
    }

    // Smooth velocity change (physics)
    public void ApplyVelocity() => RB.velocity = Vector3.SmoothDamp(RB.velocity, globalVelocity,
        ref refVelocity, movementSmoothing);
    
    public void ApplyJump(float localJumpForce)
    {
        globalJumpForce = transform.TransformDirection(LocalRbVelocity().x, localJumpForce, 0f);
        RB.velocity = globalJumpForce;    
    }

    public void SetYarn()
    {
        // Cast a ray from player's position to his feet
        RaycastHit2D hit = Physics2D.Raycast(transform.position, CurrentVector,
            YarnCheckDistance, WhatIsYarn);

        // Calculate the difference normal and character
        CurrentFloatEulerAngles = -Vector2.SignedAngle(hit.normal, Vector2.up);

        // If raycast hits Yarn layer -> estimate angle between hit
        // normal and CurrentVector to define
        // the angle to rotate character/gravity/horizontalInput/jumpForce direction.

        if (hit)
        {
            // Define new direction for RaycastHit
            CurrentVector = -hit.normal;

            ApplyRotation(CurrentFloatEulerAngles);

            if (OldCurrentVector != CurrentVector)  // Changes grav if CurrentVector has changed
            {
                gravityVector.Set(CurrentVector.x * 9.8f, CurrentVector.y * 9.8f);

                Physics2D.gravity = gravityVector;
            }

            OldCurrentVector = CurrentVector;

            Debug.DrawRay(hit.point, hit.normal, Color.red);
        }
    }

    public void ApplyRotation(float currentAngleGr)
    {
        currentVectorEulerAngles.Set(0f, 0f, currentAngleGr);

        // Euler -> quaternion
        currentQuat.eulerAngles = currentVectorEulerAngles;

        // rotate Player
        if(Quaternion.Angle(transform.rotation, currentQuat) > .05f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, currentQuat, smoothDampRotation);
        }
    }

    #endregion

    #region Check functions

    public void CheckIfFlip(float movementInput)
    {
        if (movementInput > 0 && !facingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (movementInput < 0 && facingRight)
        {
            // ... flip the player.
            Flip();
        }
    }

    public bool CheckIfGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }

    public bool CheckForCeiling()
    {
        return Physics2D.OverlapCircle(ceilingCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }

    #endregion

    #region Other functions

    // Returns distance between sputnik and player
    public float SputnikDistance() =>
        (sputnikTransform.position - transform.position).magnitude;

    public void SetColliderHeight(float height)
    {
        Vector2 center = MovementCollider.offset;
        workspace.Set(MovementCollider.size.x, height);

        center.y += (height - MovementCollider.size.y) / 2;

        MovementCollider.size = workspace;
        MovementCollider.offset = center;
    }
    
    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void DeathTrigger()
    {
        Debug.Log("GG");
        deathParticles.Play();
    }

    public Vector2 LocalRbVelocity()
    {
        return transform.InverseTransformDirection(RB.velocity);
    }



    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimationFinishedTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    #endregion

    #region Editor
    private void OnDrawGizmos() // Радиус сцепки со спутником
    {
        Gizmos.DrawWireSphere(transform.position, SpiritEnterRadius);

        Vector3 vector3current = CurrentVector;

        Gizmos.DrawLine(transform.position, transform.position + vector3current * YarnCheckDistance);
    }

    #endregion
}
