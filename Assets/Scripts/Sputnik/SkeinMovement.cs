using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeinMovement : MonoBehaviour
{

    [SerializeField] private GameObject player; // Players 
    [SerializeField] private Transform rotationCentre;

    [SerializeField] private float offsetValue = 1.0f;  // Distance value from player
    [SerializeField] private float distDamp = 10f;  //Linear Smoothing
    [SerializeField] private float rotationSpeed = 5f; // Spherical rotation Smoothing

    public PlayerInputHandler InputHandler { get; private set; }

    private Vector3 TargetPos;

    private void Awake()
    {
        InputHandler = player.GetComponent<PlayerInputHandler>();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private Vector3 MousePosition() //Follow-point calculation
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

    private void Movement()
    {
        TargetPos = rotationCentre.position;

        Vector3 toPos = new Vector3(transform.position.x, transform.position.y);

        // Direction to be headed
        Vector3 direction = MousePosition() - TargetPos;

        // Position to take
        if ((MousePosition() - TargetPos).magnitude > offsetValue)
        {
            toPos = TargetPos + (direction.normalized * offsetValue);
        }
        else
        {
            toPos = MousePosition();
        }
           
        // Smooth linear approach
        Vector3 curPos = Vector3.Lerp(transform.position, toPos, distDamp * Time.fixedDeltaTime);
        transform.position = curPos;

        // Smooth alligning X local axis with direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }


    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Danger"))
        {
            Destroy(gameObject);
        }
    }*/
}
