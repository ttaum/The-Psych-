using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Sputnik : MonoBehaviour

{
    #region State Variables 
    // Выделяем память под инстансы состояний
    public SputnikStateMachine SputnikStateMachine { get; private set; }
    public SputnikIdleState IdleState { get; private set; }
    public SputnikFreeState FreeState { get; private set; }
    public SputnikAttackState AttackState { get; private set; }
    public SputnikDefenseState DefenseState { get; private set; }
    #endregion

    #region Components
    public Animator Anim { get; private set; } // Выделяем память под аниматор
    public PlayerInputHandler InputHandler { get; private set; } // Выделяем память под компонент ввода
    public CircleCollider2D MovementCollider { get; private set; }

    #endregion

    #region Objects
    [Header("Objects")]

    [SerializeField]
    public Player player;

    [SerializeField] 
    private GameObject input; // Input reference

    [SerializeField] 
    private Transform rotationCentre; // Rotation centre

    [SerializeField]
    private SputnikData sputnikData;

    [SerializeField]
    public GameObject playerVcam;

    [SerializeField]
    public GameObject sputnikVcam;

    [SerializeField]
    public ParticleSystem hitParticles;

    #endregion

    #region Other variables

    [SerializeField] private LayerMask WhatIsDestructible;

    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        // Создаем инстанс машины состояний
        SputnikStateMachine = new SputnikStateMachine();

        IdleState = new SputnikIdleState(this, SputnikStateMachine, sputnikData, "idle");
        FreeState = new SputnikFreeState(this, SputnikStateMachine, sputnikData, "free");
        AttackState = new SputnikAttackState(this, SputnikStateMachine, sputnikData, "attack");
        DefenseState = new SputnikDefenseState(this, SputnikStateMachine, sputnikData, "defense");
    }

    private void Start()
    {
        Anim = GetComponent<Animator>();

        InputHandler = input.GetComponent<PlayerInputHandler>();

        SputnikStateMachine.Initialize(IdleState);
    }

    private void Update()
    {
        SputnikStateMachine.CurrentState.LogicUpdate();
    }


    private void FixedUpdate()
    {
        SputnikStateMachine.CurrentState.PhysicsUpdate();
    }

    #endregion

    #region Functions

    public Vector3 MousePosition() //Follow-point calculation
    {
        // Define main plane 
        Plane plane = new Plane(Vector3.forward, Vector3.zero);
        // Ray from camera to mouse position
        Ray ray = Camera.main.ScreenPointToRay(InputHandler.MouseInput);
        // If ray intersects with plane return point of intersection
        if (plane.Raycast(ray, out float dist))
        {
            return ray.GetPoint(dist);
        }
        return Vector3.zero;
    }

    public void FreeMovement() // Movement in free state

    {
        Vector3 direction = MousePosition() - transform.position;

        if ((direction).magnitude > sputnikData.freeOffsetValue)
        {
            Vector3 toPos = MousePosition() - direction.normalized * sputnikData.freeOffsetValue;

            Vector3 curPos = Vector3.Lerp(transform.position, toPos,
                sputnikData.freeMovementDamp * Time.fixedDeltaTime);
            transform.position = curPos;
        }

        // Alligning X local axis with direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Smooth approach
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation,
            sputnikData.freeRotationDamp * Time.deltaTime);
    } 

    public void BindMovement() // Movement in binded state
    {
        Vector3 toPos = new Vector3(transform.position.x, transform.position.y);

        // Direction to be headed
        Vector3 direction = MousePosition() - rotationCentre.position;

        // Position to take
        if ((direction).magnitude > sputnikData.bindOffsetValue)
        {
            toPos = rotationCentre.position + direction.normalized * sputnikData.bindOffsetValue;
        }
        else
        {
            toPos = MousePosition();
        }

        // Smooth approach
        Vector3 curPos = Vector3.Lerp(transform.position, toPos,
            sputnikData.bindMovememtDamp * Time.fixedDeltaTime);
        transform.position = curPos;

        // Alligning X local axis with direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Smooth approach
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation,
            sputnikData.bindRotationDamp * Time.deltaTime);
    } 

    public void RayAttack()
    {
        Vector3 direction = MousePosition() - transform.position;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized,
            20f, WhatIsDestructible);

        Debug.DrawLine(transform.position, MousePosition(), Color.green);
        Debug.DrawLine(transform.position, hit.point, Color.red);

        if (hit)
        {
            Debug.DrawRay(hit.point, hit.normal, Color.yellow);

            Quaternion quaternion = Quaternion.LookRotation(hit.normal);

            hitParticles.transform.position = hit.point;
            hitParticles.transform.rotation = quaternion;

            hitParticles.Play();

            hit.transform.SendMessage("Dissolving");
        }
    }

    #endregion
}
