using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rivet : MonoBehaviour
{
    public GameObject target;
    public GameObject attachedObject;
    public Rigidbody rb;
    public bool mobile = false;
    bool activated;
    void Update()
    {
        if (!activated)
        {
            return;
        }
        Vector3 gravityVector = target.transform.position - transform.position;
        rb.AddForce(gravityVector, ForceMode.Acceleration);
    }

    public void ActivateRivet(GameObject targetInput)
    {
        target = targetInput;
        activated = true;
    }

    public void AttachRivet(GameObject target)
    {
        attachedObject = target;
        FixedJoint joint = GetComponent<FixedJoint>();
        rb = target.GetComponent<Rigidbody>();
        if (joint && rb)
        {
            joint.connectedBody = rb;
            mobile = !rb.isKinematic;
        }
    }
    public void PopoffObject()
    {
        Rigidbody rb = attachedObject.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.isKinematic = false;
            PopoffDependencies popoffDependencies = attachedObject.GetComponent<PopoffDependencies>();
            if (popoffDependencies)
            {
                popoffDependencies.PopoffDependents();
            }
        }
        
    }

    public void TurnOffGravity()
    {
        Rigidbody rb = attachedObject.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.useGravity = false;
        }
    }
}
