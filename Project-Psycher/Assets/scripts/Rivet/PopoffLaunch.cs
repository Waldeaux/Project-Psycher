using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopoffLaunch : PopoffReactions
{
    public override void Reaction(Vector3 gravity)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb)
        {
            rb.AddForce(-10*Vector3.Project(gravity, transform.up), ForceMode.VelocityChange);
        }
    }
}
