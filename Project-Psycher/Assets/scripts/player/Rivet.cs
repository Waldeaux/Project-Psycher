using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rivet : MonoBehaviour
{
    public GameObject target;
    public GameObject attachedObject;
    public Rigidbody rb;
    private RivetInfo rivetInfo;
    public bool activated;

    public GameObject indicatorPrefab;
    public GameObject indicatorInstance;

    public GameObject representative;
    private float magnitude = 5;
    void FixedUpdate()
    {

        if (!activated || !rb)
        {
            return;
        }
        Vector3 gravityVector = (target.transform.position - transform.position);
        if(gravityVector.magnitude > magnitude)
        {
            gravityVector = gravityVector.normalized*magnitude;
        }

        rb.AddForce(gravityVector/Time.fixedDeltaTime, ForceMode.Acceleration);
       
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
        if (rb)
        {
            if (!target.GetComponent<ObjectNetworkRigidbody>())
            {
                ObjectNetworkRigidbody nRb = target.AddComponent<ObjectNetworkRigidbody>();
            }
        }
        if (joint && rb)
        {
            joint.connectedBody = rb;
            rivetInfo = target.AddComponent<RivetInfo>();
            rivetInfo.attachedRivet = this;
        }
    }
    public void PopoffObject()
    {
        ObjectNetworkRigidbody rb = attachedObject.GetComponent<ObjectNetworkRigidbody>();
        if (rb)
        {
            rb.networkKinematic = false;
            var popoffReactions = attachedObject.GetComponents<PopoffReactions>();
            foreach(PopoffReactions reactor in popoffReactions)
            {
                reactor.Reaction(transform.position - target.transform.position);
            }
            FixedJoint[] joints = attachedObject.GetComponents<FixedJoint>();
            foreach (FixedJoint joint in joints)
            {
                Destroy(joint);
            }
        }
        
    }

    public void ToggleGravity(bool useGravity = false)
    {
        return;
        ObjectNetworkRigidbody rb = attachedObject.GetComponent<ObjectNetworkRigidbody>();
        if (rb)
        {
            rb.networkUseGravity = useGravity;
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
        Destroy(representative);
        Destroy(rivetInfo);
        Destroy(this.gameObject);
    }

    public bool IsMobile()
    {
        return !attachedObject.GetComponent<Rigidbody>().isKinematic && attachedObject.GetComponent<FixedJoint>() == null;
    }
}
