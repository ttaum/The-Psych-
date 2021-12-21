using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
    #region State Variables 
    // Выделяем память под инстансы состояний
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
    public Animator Anim { get; private set; } // Выделяем память под аниматор
    public PlayerInputHandler InputHandler { get; private set; } // Выделяем память под компонент ввода
    public Rigidbody2D RB { get; private set; }
    public CapsuleCollider2D MovementCollider { get; private set; }

    #endregion

    #region Objects

    [Header("Objects")]

    [SerializeField]
    private GameObject input;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private PlayerData playerData;

    [SerializeField]
    private Transform ceilingCheck;

    [SerializeField]
    public ParticleSystem particleToSputnik;

    // Раскоментить если нужно взаимодействие с камерой
    /*[SerializeField]
    private CinemachineVirtualCamera vcam;
    */

    #endregion

    #region Other variables

    private Vector2 workspace;

    private Vector2 localVelocity; // Локальная скорость перемещения  
    private Vector2 globalVelocity; // Глобальная скорость перемещения
    private Vector3 refVelocity; // тех. значение для smoothDamp

    private Vector2 globalJumpForce; // Глобальное значение для прыжка

    [Header("Smoothing and damping")]

    [Range(0, .2f)] [SerializeField] private float movementSmoothing = .01f;
    //  [Range(0, 5f)] [SerializeField] private float smoothDampCam = 1f;  Раскоментить для вращения камеры
    [Range(0, 25f)] [SerializeField] private float smoothDampRotation = 1f;

    [Header("Other")]

    [SerializeField] private LayerMask WhatIsYarn;
    [SerializeField] private float YarnCheckDistance;

    private bool facingRight = true;

    // public bool isAirForceAllowed; Для придания толчка в воздухе

    public Vector2 CurrentVector { get; private set; } = Vector2.down;

    private Quaternion currentQuat;

    private Vector3 currentVectorEulerAngles;
    public float CurrentFloatEulerAngles { get; private set; }

    private Vector2 gravityVector;
 
    #endregion

    #region Unity Callback Functions

    private void Awake()
    {
        // Создаем инстанс машины состояний
        StateMachine = new StateMachine();

        // Создаем инстасы состояний
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
        Anim = GetComponent<Animator>(); // Референсим компонент аниматор

        InputHandler = input.GetComponent<PlayerInputHandler>(); // Референсим компонент ввода

        RB = GetComponent<Rigidbody2D>();

        MovementCollider = GetComponent<CapsuleCollider2D>();

        StateMachine.Initialize(IdleState); // Инициализация машины состояний
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

    // Пересчет локальной скорости в глобальную (лог.)
    public void SetVelocity(float movementInput)
    {
        localVelocity.Set(movementInput, 0f);    
        globalVelocity = transform.TransformDirection(localVelocity);      
    }

    // Смягченное изменение скорости (физ.)
    public void ApplyVelocity()
    {
        RB.velocity = Vector3.SmoothDamp(RB.velocity, globalVelocity, ref refVelocity, movementSmoothing);
    }

    public void SetJump(float localJumpForce)
    {
        globalJumpForce = transform.TransformDirection(LocalRbVelocity().x, localJumpForce, 0f);

        RB.velocity = globalJumpForce; // Через скорость
       // RB.AddForce(globalJumpForce, ForceMode2D.Impulse); // Через силу
    }



    // Для придания толчка в воздухе

    /*public void SetAirForce(float input) 
    {
        Vector2 localAirForce = new Vector2(playerData.airForce * input, 0); // мб переписать
        Vector2 AirForce = transform.TransformDirection(localAirForce);

        RB.AddForce(AirForce, ForceMode2D.Impulse);
    }*/

    public Vector2 LocalRbVelocity()
    {
        return transform.InverseTransformDirection(RB.velocity);
    }

    public void SetRotation(float currentAngleGr)
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
    public void CheckYarn()
    {
        // Cast a ray from player's position to his feet
        RaycastHit2D hit = Physics2D.Raycast(transform.position, CurrentVector, YarnCheckDistance, WhatIsYarn);

        // If raycast hits Yarn layer -> estimate angle between hit normal and CurrentVector to define
        // the angle to rotate character/gravity/horizontalInput/jumpForce direction.

        if (hit)
        {
            CurrentFloatEulerAngles = -Vector2.SignedAngle(hit.normal, Vector2.up);      
                         
            // Define new direction for RaycastHit
            CurrentVector = -hit.normal;

            SetRotation(CurrentFloatEulerAngles);

            gravityVector.Set(CurrentVector.x * 9.8f, CurrentVector.y * 9.8f);

            // Physics2D.gravity = new Vector2(CurrentVector.x, CurrentVector.y) * 9.8f;
            Physics2D.gravity = gravityVector;

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

    public bool CheckForCeiling()
    {
        return Physics2D.OverlapCircle(ceilingCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }

    // Для придания толчка в воздухе

    /*public void CheckIfAirForce(bool check) => isAirForceAllowed = check;*/

    #endregion

    #region Other functions

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

    private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

    private void AnimationFinishedTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    #endregion
}
