using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAttachment : MonoBehaviour
{
    public bool debug;
    public float breakforce;
    public Vector3 offset;
    public GameObject spotIndicator;
    void Start()
    {
        RaycastHit hitInfo;
        Vector3 relativeOffset = transform.TransformVector(offset);
        if(debug)
        {
            print(gameObject.name);
            print(relativeOffset);
            GameObject testCube = GameObject.Instantiate(spotIndicator);
            testCube.transform.position = transform.position + relativeOffset + transform.up * .05f + -transform.up * .1f;
        }

        if (Physics.Raycast(transform.position + relativeOffset + transform.up * .05f, -transform.up, out hitInfo))
        {
            if (debug)
            {
                print("good");
            }
            Rigidbody rb = hitInfo.collider.gameObject.GetComponentInParent<Rigidbody>();
            if (rb)
            {
                GetComponent<Rigidbody>().useGravity = false;
                FixedJoint joint = gameObject.AddComponent<FixedJoint>();
                joint.connectedBody = rb;
                if (breakforce > 0)
                {
                    joint.breakForce = breakforce;
                }
            }
        }
    }

    private void OnJointBreak(float breakForce)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb)
        {
            rb.useGravity = true;
        }
    }

}
