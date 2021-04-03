using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RivetIndicator : MonoBehaviour
{
    public SpriteRenderer renderer;
    public GameObject rivet1;
    public GameObject rivet2;

    void Update()
    {
        Vector3 vector = rivet1.transform.position - rivet2.transform.position;
        transform.position = rivet2.transform.position + vector / 2;
        transform.LookAt(rivet1.transform);
        renderer.size = new Vector2(vector.magnitude, 1);
    }
}
