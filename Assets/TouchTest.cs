using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchTest : MonoBehaviour
{
    public Transform target;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 0.3F;

    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity,
            smoothTime);
    }
}
