using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VinylManager : MonoBehaviour
{
    public int maxYarnCharges { get; private set; }

    public int currentYarnCharges;

    private void Awake()
    {
        maxYarnCharges = 4;
        currentYarnCharges = maxYarnCharges;
    }

}
