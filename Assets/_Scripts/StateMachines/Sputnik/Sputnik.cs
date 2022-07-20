using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Sputnik : Singleton<Sputnik>
{
    #region State Variables 

    public SputnikStateMachine SputnikStateMachine { get; private set; }
    public SputnikFreeState FreeState { get; private set; }
    public SputnikAttackState AttackState { get; private set; }
    public SputnikDefenseState DefenseState { get; private set; }

    #endregion

    #region Components
    public Rigidbody2D RB { get; private set; }
    public Animator Anim { get; private set; }
    public CircleCollider2D MovementCollider { get; private set; }

    #endregion

    #region Objects
    [Header("Objects")]

    [SerializeField]
    private SputnikData sputnikData;

    #endregion

    #region Other variables

    private Plane MainPlane = new Plane(Vector3.forward, Vector3.zero);

    private Vector3 direction;
    private Vector2 curVelocity;

    [SerializeField] private LayerMask WhatIsDestructible;

    #endregion

    #region Unity Callback Functions
    protected override void Awake()
    {
        base.Awake();
        
        SputnikStateMachine = new SputnikStateMachine();

        FreeState = new SputnikFreeState(this, SputnikStateMachine, sputnikData, "free");
        AttackState = new SputnikAttackState(this, SputnikStateMachine, sputnikData, "attack");
        DefenseState = new SputnikDefenseState(this, SputnikStateMachine, sputnikData, "defense");
    }

    private void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();

        SputnikStateMachine.Initialize(FreeState);
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

    public void SetVelocity()
    {
        // Ray from camera to mouse position
        Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.MouseInput);

        // If ray intersects with plane return point of intersection
        direction = MainPlane.Raycast(ray, out float dist) ? ray.GetPoint(dist) - transform.position : Vector3.zero;

        // Calculate velocity vector based on direction
        curVelocity = direction * sputnikData.movementSpeed;

    }

    public void ApplyVelocity(Vector2 curVelocity, float Damp) => 
        RB.velocity = Vector3.Lerp(RB.velocity, curVelocity, Damp * Time.fixedDeltaTime);

    public void SputnikFreeMove()     
    {

        if ((direction).magnitude > sputnikData.offsetValue)
        {
            ApplyVelocity(curVelocity, sputnikData.accelertaionDamp);
        }
        else
        {
            ApplyVelocity(curVelocity, sputnikData.decelerationDamp);
        }

    } 

    public void SputnikRotation()
    {
        // Alligning X local axis with direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Smoothing
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation,
            sputnikData.rotationDamp * Time.fixedDeltaTime);
    }


    /*public void RayAttack()
    {
        Vector3 direction = SetVelocity() - transform.position;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized,
            20f, WhatIsDestructible);

        Debug.DrawLine(transform.position, SetVelocity(), Color.green);
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
    }*/

    #endregion

}
