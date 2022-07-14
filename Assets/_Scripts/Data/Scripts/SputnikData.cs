using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newData", menuName = "Data/Sputnik data/Base Data")]
public class SputnikData : ScriptableObject
{
/*    [Header("Bind State")]
    public float bindOffsetValue = 3.0f;    // Distance value from player
    public float bindMovememtDamp = 10f;    //Linear Smoothing
    public float bindRotationDamp = 5f; // Spherical rotation Smoothing
*/

    [Header("Free State")]

    [Range(0, 10f)] [SerializeField] public float offsetValue = 5.0f;
    [Range(0, 5f)] [SerializeField] public float movementSpeed = 2f;
    [Range(0, 1f)] [SerializeField] public float accelertaionDamp = .1f;
    [Range(0, 1f)] [SerializeField] public float decelerationDamp = .1f;


    [Range(0, 10f)] [SerializeField] public float rotationDamp = .1f;
}
