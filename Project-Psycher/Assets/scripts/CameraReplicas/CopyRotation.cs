using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyRotation : MonoBehaviour
{
    public GameObject target;
    private void LateUpdate()
    {
        transform.rotation = target.transform.rotation;
    }
}
