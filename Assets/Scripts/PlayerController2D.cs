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
    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .3f;  // How much to smooth out the movement
    [SerializeField] private LayerMask whatIsGround; // A mask determining what is ground to the character
    [SerializeField] private LayerMask whatIsYarn; // A mask determining what is yarn to the character
    [SerializeField] private Transform groundCheck; // A position marking where to check if the player is grounded.
    [SerializeField] private CinemachineVirtualCamera vcam;
    private Rigidbody2D playerRb;
    private CapsuleCollider2D capsuleCd;

    private Vector3 velocity = Vector3.zero;
    private Vector2 curVelocity;
    private Vector2 slopeNormalPerp;
    private Vector2 currentVector = Vector3.down;

    private Vector3 currentEuler;
    private Quaternion currentQuat;

    const float groundedRadius = .2f;   // Radius of the overlap circle to determine if grounded

    public bool facingRight = true;  // For determining which way the player is currently facing.
    private bool grounded;   // Whether or not the player is grounded.

    #region Slope Fields

    [SerializeField] private float slopeCheckDistance;  //Distance raycast shoots the ray in slopecheck
    [SerializeField] private float maxSlopeAngle;   // Max slope angle to climb on
    private float currentAngle = 0f;
    private float smoothAngle = 0f;

    public float smoothDamp; // Rotation smooth coeff for transform.rotation and camera

    private SpriteShapeRenderer spriteShape;
    #endregion

    #endregion

    #region StartUp
    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        capsuleCd = GetComponent<CapsuleCollider2D>();
      //  colliderSize = capsuleCd.size;
    }

    #endregion

    #region Physics
    private void FixedUpdate()
    {
        CheckGround();

      //  smoothAngle = Mathf.Lerp(smoothAngle, currentAngle, smoothDamp * Time.deltaTime);
      //  vcam.m_Lens.Dutch = smoothAngle;
    }

    private void Update()
    {
      
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

        // Cast a ray from player's position to his feet
        RaycastHit2D hit = Physics2D.Raycast(transform.position, currentVector, slopeCheckDistance, whatIsYarn);

        // If raycast hits Yarn layer -> estimate angle between hit normal and up vector to define
        // the angle to rotate character/gravity/horizontalInput/jumpForce direction.
        if (hit)
        {          
            currentAngle = -Vector2.SignedAngle(hit.normal, Vector2.up);
            // Define the anlge to rotate
            currentEuler = new Vector3(0f, 0f, currentAngle);

            // Euler -> quaternion
            currentQuat.eulerAngles = currentEuler;

            // Rotate character
            transform.rotation = Quaternion.Slerp(transform.rotation, currentQuat, smoothDamp * Time.deltaTime);
            // camera
            smoothAngle = Mathf.Lerp(smoothAngle, currentAngle, smoothDamp * Time.deltaTime);
            vcam.m_Lens.Dutch = smoothAngle;

            // Define new direction for RaycastHit
            currentVector = -hit.normal;

            // Apply new gravity vector according to normal hit
            Physics2D.gravity = new Vector3(-hit.normal.x, -hit.normal.y, 0f) * 9.8f;
            
            Debug.DrawRay(hit.point, slopeNormalPerp, Color.red);
            Debug.DrawRay(hit.point, hit.normal, Color.green);
            Debug.DrawRay(transform.position, currentVector, Color.blue);

            if (hit.transform.gameObject.GetComponent<SpriteShapeRenderer>() != null)
            {
                spriteShape = hit.transform.gameObject.GetComponent<SpriteShapeRenderer>();

              //  spriteShape.color = Color.green;
            }
            
        }

    }
    #endregion

    #region Movements
    public void Move(float move, bool jump, bool jumpRelease)
    {
        currentAngle = currentAngle * Mathf.PI / 180;

        if (grounded)
        {
            curVelocity.Set(Mathf.Cos(currentAngle) * move * 10f, Mathf.Sin(currentAngle) * move * 10f);
            
            playerRb.velocity = Vector3.SmoothDamp(playerRb.velocity, curVelocity,
                ref velocity, movementSmoothing);

            CheckFlip();
        }
        
        /*else if (!grounded)
        {
            curVelocity.Set(Mathf.Cos(currentAngle) * move * 10f, Mathf.Sin(currentAngle) * move * 10f);

            playerRb.velocity = Vector3.SmoothDamp(playerRb.velocity, curVelocity,
                ref velocity, movementSmoothing);
       
            CheckFlip();
        }*/

        void CheckFlip()
        {
            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !facingRight)
            {
                // ... flip the player.
                FlipHor();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && facingRight)
            {
                // ... flip the player.
                FlipHor();
            }
        }

        // If the player should jump...
        if (grounded && jump)
        {
            // Add a vertical force to the player.
            grounded = false;
            playerRb.AddForce(
                new Vector2(- Mathf.Sin(currentAngle) * jumpForce, Mathf.Cos(currentAngle) * jumpForce), ForceMode2D.Impulse);
        }
    }

    private void FlipHor()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    #endregion

    #region Slope kinematic

    private void SlopeCheckVertical(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, whatIsGround);

        if (hit)
        {
            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;

           // slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            Debug.DrawRay(hit.point, slopeNormalPerp, Color.red);
            Debug.DrawRay(hit.point, hit.normal, Color.green);

          //  if (slopeDownAngle != slopeDownAngleOld)
            {
              //  isOnSlope = true;
            }

           // slopeDownAngleOld = slopeDownAngle;
        }

      //  if (slopeDownAngle > maxSlopeAngle || slopeSideAngle > maxSlopeAngle)
      //  {
           // canWalkOnSlope = false;
      //  }
        else
        {
           // canWalkOnSlope = true;
        }
    }
    #endregion
}

