using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sputnik : MonoBehaviour

{
    #region State Variables 
    // Выделяем память под инстансы состояний
    public SputnikStateMachine SputnikStateMachine { get; private set; }
    public SputnikBindState BindState { get; private set; }
    public SputnikFreeState FreeState { get; private set; }
    #endregion

    #region Components
    public Animator Anim { get; private set; } // Выделяем память под аниматор
    public PlayerInputHandler InputHandler { get; private set; } // Выделяем память под компонент ввода
    public CircleCollider2D MovementCollider { get; private set; }

    #endregion

    #region Objects
    [Header("Objects")]
    [SerializeField] 
    private GameObject input; // Input reference

    [SerializeField] 
    private Transform rotationCentre; // Rotation centre

    [SerializeField]
    private SputnikData sputnikData;

    #endregion

    #region Other variables

    #endregion

    #region Unity Callback Functions
    private void Awake()
    {
        // Создаем инстанс машины состояний
        SputnikStateMachine = new SputnikStateMachine();

        BindState = new SputnikBindState(this, SputnikStateMachine, sputnikData, "bind");
        FreeState = new SputnikFreeState(this, SputnikStateMachine, sputnikData, "free");

    }

    private void Start()
    {
        Anim = GetComponent<Animator>();

        InputHandler = input.GetComponent<PlayerInputHandler>();

        SputnikStateMachine.Initialize(BindState);

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

    public void BindMovement()
    {
        Vector3 toPos = new Vector3(transform.position.x, transform.position.y);

        // Direction to be headed
        Vector3 direction = MousePosition() - rotationCentre.position;

        // Position to take
        if ((direction).magnitude > sputnikData.offsetValue)
        {
            toPos = rotationCentre.position + (direction.normalized * sputnikData.offsetValue);
        }
        else
        {
            toPos = MousePosition();
        }

        // Smooth approach
        Vector3 curPos = Vector3.Lerp(transform.position, toPos,
            sputnikData.movememtDamp * Time.fixedDeltaTime);
        transform.position = curPos;

        // Alligning X local axis with direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Smooth approach
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation,
            sputnikData.rotationDamp * Time.deltaTime);
    }
}