using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    void Start()
    {
    }

    public void AddPlayer(GameObject player)
    {
            player.GetComponent<Player>().networkEnabled = true;
            player.GetComponent<PlayerNetworkRigidbody>().networkKinematic = false;
    }

}
