using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newData", menuName = "Data/Sputnik data/Base Data")]
public class SputnikData : ScriptableObject
{
    [Header("Free State")]

    [Range(0, 10f)]  public float offsetValue = 5.0f;
    [Range(0, 5f)]  public float movementSpeed = 2f;
    [Range(0, 15f)]  public float accelertaionDamp = 5f;
    [Range(0, 15f)]  public float decelerationDamp = 5f;

    [Range(0, 10f)] public float rotationDamp = 1f;
}
