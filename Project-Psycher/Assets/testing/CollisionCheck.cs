using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        print(gameObject);
        print(collision.gameObject.name);
    }
}
