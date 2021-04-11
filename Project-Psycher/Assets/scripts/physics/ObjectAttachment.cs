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
            print(relativeOffset);
            GameObject testCube = GameObject.Instantiate(spotIndicator);
            testCube.transform.position = transform.position + relativeOffset + transform.up * .05f + -transform.up * .1f;
        }

        if(Physics.Raycast(transform.position+relativeOffset+transform.up*.05f, -transform.up, out hitInfo, .1f))
        {
            if (debug)
            {
                print("good");
            }
            transform.position = hitInfo.point - relativeOffset;
            if (hitInfo.rigidbody)
            {
                FixedJoint joint = gameObject.AddComponent<FixedJoint>();
                joint.connectedBody = hitInfo.rigidbody;
                if (breakforce > 0)
                {
                    joint.breakForce = breakforce;
                }
            }
        }
    }

}
