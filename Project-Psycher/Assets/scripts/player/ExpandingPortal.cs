using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandingPortal : MonoBehaviour
{
    public float currentScale;
    public float time;
    private float scalePerUpdate;
    private void Start()
    {
        if(time == 0)
        {
            time = 1;
        }

        scalePerUpdate = time / Time.fixedDeltaTime;
        scalePerUpdate = 1 / scalePerUpdate;
        Vector3 scale = transform.localScale;
        scale.x = 0;
        transform.localScale = scale;
    }
    private void FixedUpdate()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Min(scale.x + scalePerUpdate, 1);
        transform.localScale = scale;
    }
}
