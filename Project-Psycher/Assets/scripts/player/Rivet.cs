using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rivet : MonoBehaviour
{
    public GameObject target;
    public GameObject attachedObject;
    public Rigidbody rb;
    private RivetInfo rivetInfo;
    bool activated;
    void Update()
    {
        if (!activated || !rb)
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
            rivetInfo = target.AddComponent<RivetInfo>();
            rivetInfo.attachedRivet = this;
        }
    }
    public void PopoffObject()
    {
        Rigidbody rb = attachedObject.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.isKinematic = false;
            var popoffReactions = attachedObject.GetComponents<PopoffReactions>();
            foreach(PopoffReactions reactor in popoffReactions)
            {
                reactor.Reaction(transform.position - target.transform.position);
            }
        }
        
    }

    public void ToggleGravity(bool useGravity = false)
    {
        Rigidbody rb = attachedObject.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.useGravity = useGravity;
        }
    }

    public void DestroyRivet(bool destroyPair = true)
    {
        if (activated && destroyPair)
        {
            target.GetComponent<Rivet>().DestroyRivet(false);
        }
        if (!attachedObject.GetComponent<DelayedGravityToggle>())
        {
            attachedObject.AddComponent<DelayedGravityToggle>();
        }
        Destroy(rivetInfo);
        Destroy(this.gameObject);
    }

    public bool IsMobile()
    {
        return !attachedObject.GetComponent<Rigidbody>().isKinematic;
    }
}
