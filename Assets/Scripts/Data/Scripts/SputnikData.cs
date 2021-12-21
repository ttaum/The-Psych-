using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newData", menuName = "Data/Sputnik data/Base Data")]
public class SputnikData : ScriptableObject
{
    [Header("Bind State")]
    public float offsetValue = 3.0f;    // Distance value from player
    public float movememtDamp = 10f;    //Linear Smoothing
    public float rotationDamp = 5f; // Spherical rotation Smoothing
}
