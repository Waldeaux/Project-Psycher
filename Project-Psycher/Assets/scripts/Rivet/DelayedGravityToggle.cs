using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedGravityToggle : MonoBehaviour
{
    void Update()
    {
        if (!this.gameObject.GetComponent<RivetInfo>())
        {
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.useGravity = true;
            }
        }
        Destroy(this);
    }
}
