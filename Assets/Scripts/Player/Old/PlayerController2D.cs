using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;
using UnityEngine.U2D;


public class PlayerController2D : MonoBehaviour
{
    #region Declaration Fields

    [SerializeField] private float jumpForce = 400f;    // Amount of force added when the player jumps.
    [Range(0, .5f)] [SerializeField] private float movementSmoothing = .3f;  // How much to smooth out the movement
    [SerializeField] private LayerMask whatIsGround; // A mask determining what is ground to the character
    [SerializeField] private LayerMask whatIsYarn; // A mask determining what is yarn to the character
    [SerializeField] private Transform groundCheck; // A position marking where to check if the player is grounded.
    [SerializeField] private float yarnCheckDistance; //Distance raycast shoots the ray in yarn
    [SerializeField] private CinemachineVirtualCamera vcam;
    private Rigidbody2D playerRb;

    private Vector3 velocity = Vector3.zero;
    private Vector2 curVelocity;
    private Vector2 slopeNormalPerp;
    private Vector2 currentVector = Vector3.down; // Current raycast direction vector

    private Vector3 currentEuler;
    private Quaternion currentQuat;

    const float groundedRadius = .2f;   // Radius of the overlap circle to determine if grounded

    public bool facingRight = true;  // For determining which way the player is currently facing.
    public bool grounded;   // Whether or not the player is grounded.

    private float currentAngle = 0f;
    private float smoothAngle = 0f;

    public float smoothDamp; // Rotation smooth coeff for transform.rotation and camera

    #endregion

    #region StartUp
    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    #endregion

    #region Physics
    private void FixedUpdate()
    {
        Rotation();
        CheckGround();
    }

    private void Update()
    {
        CheckYarn();
    }

    private void CheckGround()
    {
        grounded = false;
        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
            }
        }
    }


    private void Rotation()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, currentQuat, smoothDamp * Time.fixedDeltaTime);
        // camera
        smoothAngle = Mathf.Lerp(smoothAngle, currentAngle, smoothDamp * Time.fixedDeltaTime);

        vcam.m_Lens.Dutch = smoothAngle;
     
        // Apply new gravity vector according to normal hit
        Physics2D.gravity = new Vector3(currentVector.x, currentVector.y, 0f) * 9.8f;
    }
    private void CheckYarn()
    {
        // Cast a ray from player's position to his feet
        RaycastHit2D hit = Physics2D.Raycast(transform.position, currentVector, yarnCheckDistance, whatIsYarn);

        // If raycast hits Yarn layer -> estimate angle between hit normal and CurrentVector to define
        // the angle to rotate character/gravity/horizontalInput/jumpForce direction.
        if (hit)
        {
            currentAngle = -Vector2.SignedAngle(hit.normal, Vector2.up);
            Debug.Log(currentAngle);
            // Define the anlge to rotate
            currentEuler = new Vector3(0f, 0f, currentAngle);
            // Euler -> quaternion
            currentQuat.eulerAngles = currentEuler;
            // Rotate character
            // Define new direction for RaycastHit
            currentVector = -hit.normal;

            Debug.DrawRay(hit.point, slopeNormalPerp, Color.red);
            Debug.DrawRay(hit.point, hit.normal, Color.green);
            Debug.DrawRay(transform.position, currentVector, Color.blue);
        }

    }

    #endregion

    #region Movements
    public void Move(float move, bool jump, bool jumpRelease)
    {
        float currentAngleMove = currentAngle * Mathf.PI / 180;

        if (grounded)
        {
            curVelocity.Set(Mathf.Cos(currentAngleMove) * move * 10f, Mathf.Sin(currentAngleMove) * move * 10f);

            playerRb.velocity = Vector3.SmoothDamp(playerRb.velocity, curVelocity,
                             ref velocity, movementSmoothing);
            CheckFlip();
        }

        void CheckFlip()
        {
            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !facingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && facingRight)
            {
                // ... flip the player.
                Flip();
            }
        }

        // If the player should jump...
        if (grounded && jump)
        {
            // Add a jump force to the player.
            grounded = false;
            playerRb.AddForce(
                new Vector2(-Mathf.Sin(currentAngleMove) * jumpForce, Mathf.Cos(currentAngleMove) * jumpForce),
                ForceMode2D.Impulse);
        }
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

}

    #endregion
//  if (slopeDownAngle != slopeDownAngleOld)

//  isOnSlope = true;


// slopeDownAngleOld = slopeDownAngle;






