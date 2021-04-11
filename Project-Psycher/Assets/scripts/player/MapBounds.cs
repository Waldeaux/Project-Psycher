using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBounds : MonoBehaviour
{
    bool outOfBounds;
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.angularVelocity = transform.right*100;
        rb.velocity = transform.right * 100;
    }
    private void FixedUpdate()
    {
            if (transform.position.magnitude > 250)
            {
                rb.useGravity = false;
                float ratio = (1 - Mathf.Min(1, 2 * Time.deltaTime));
                rb.velocity = rb.velocity* ratio;
                rb.angularVelocity = rb.angularVelocity  *ratio;
                if (rb.velocity.magnitude < 1)
                {
                    rb.velocity = Vector3.zero;
                    if (rb.angularVelocity.magnitude > 1)
                    {
                        rb.angularVelocity = rb.angularVelocity.normalized;
                    }
                }
            }

    }
}
