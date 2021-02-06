using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandingPortal : MonoBehaviour
{
    public float currentScale;
    public float time;
    private float scalePerUpdate;
    public GameObject player;
    private float timer;

    public bool expanding;
    private void Start()
    {
        if(time == 0)
        {
            time = 1;
        }
        expanding = true;
        print(expanding);
        scalePerUpdate = time / Time.fixedDeltaTime;
        scalePerUpdate = 1 / scalePerUpdate;
        Vector3 scale = transform.localScale;
        scale.x = 0;
        transform.localScale = scale;
    }
    private void FixedUpdate()
    {
        Vector3 scale = transform.localScale;
        if (expanding)
        {
            scale.x = Mathf.Min(scale.x + scalePerUpdate, 1);
        }
        else
        {

            scale.x = Mathf.Max(scale.x - scalePerUpdate, 0);
        }
        transform.localScale = scale;
        if (!expanding && scale.x <= 0)
        {
            print("destroy");
            Transform parent = transform.parent;
            while (parent.parent)
            {
                parent = parent.parent;
            }
            Destroy(parent.gameObject);
        }
    }
}
