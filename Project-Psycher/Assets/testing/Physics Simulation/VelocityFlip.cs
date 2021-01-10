using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityFlip : MonoBehaviour
{
    Rigidbody rb;
    bool up;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (up)
        {
            rb.velocity = new Vector3(0, 5, 0);
        }
        else
        {
            rb.velocity = new Vector3(0, -5, 0);
        }
        up = !up;
    }
}
