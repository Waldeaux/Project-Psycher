using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddAttempts : MonoBehaviour
{
    public GameObject player;
    public float timer = 0;
    void Update()
    {
        timer += Time.deltaTime;
        GameControl control = GameObject.FindGameObjectWithTag("GameController")?.GetComponent<GameControl>();
        if (control)
        {
            control.AddPlayer(player);
        }
        if(control || timer >= 5)
        {
            Destroy(this);
        }
    }
}
