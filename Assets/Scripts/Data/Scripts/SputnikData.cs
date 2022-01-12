using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newData", menuName = "Data/Sputnik data/Base Data")]
public class SputnikData : ScriptableObject
{
    [Header("Bind State")]
    public float bindOffsetValue = 3.0f;    // Distance value from player
    public float bindMovememtDamp = 10f;    //Linear Smoothing
    public float bindRotationDamp = 5f; // Spherical rotation Smoothing

    [Header("Free State")]
    public float freeOffsetValue = 3.0f;
    public float freeMovementDamp = 10f;
    public float freeRotationDamp = 5f;
}
