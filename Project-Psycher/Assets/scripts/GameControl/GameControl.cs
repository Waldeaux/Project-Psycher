using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    void Start()
    {
        print(GameObject.FindGameObjectWithTag("GameInfo").GetComponent<GameInfo>().numPlayers);
    }

    public void AddPlayer(GameObject player)
    {
            player.GetComponent<Player>().networkEnabled = true;
            player.GetComponent<NetworkRigidbody>().networkKinematic = false;
    }

}
