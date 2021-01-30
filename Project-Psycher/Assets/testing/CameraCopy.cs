using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCopy : MonoBehaviour
{
    public Vector3 offset;
    public GameObject target;

    private void Awake()
    {
        foreach(CopyRotation copyRotation in GetComponentsInChildren<CopyRotation>())
        {
            copyRotation.target = target;
        }
    }
    private void LateUpdate()
    {
        transform.position = target.transform.position;
    }
}
