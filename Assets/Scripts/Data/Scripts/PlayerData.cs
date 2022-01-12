using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newData", menuName = "Data/Player data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Move State")]
    public float movementVelocity = 10f;

    [Header("Jump State")]
    public float jumpForce = 500f;
    public int amountOfJumps = 1;
    // public float airForce = 100f; Для толчка в воздухе

    [Header("In Air State")]
    // public float coyoteTime = 0.2f; Для coyote
    public float variableJumpHeightMultiplier = 0.5f;
    public float airForce = 10f;

    [Header("Crouch States")]
    public float crouchMovementVelocity = 5f;
    public float crouchColliderHeight = 1f;
    public float standColliderHeight = 2f;

    [Header("Check variables")]
    public float groundCheckRadius = 0.3f;
    public LayerMask whatIsGround;
}
