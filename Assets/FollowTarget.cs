using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField]

    private Transform target;

    private void FixedUpdate()
    {
        gameObject.transform.position = target.position;
    }
}
