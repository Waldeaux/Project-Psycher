using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapperNetworkManager : NetworkManager
{
    long startingTick;
    public override void Start()
    {
        base.Start();
        startingTick = DateTime.Now.Ticks;
    }
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
        NetworkPlayerTest newPlayer = conn.identity.gameObject.GetComponent<NetworkPlayerTest>();
        if (newPlayer)
        {
            newPlayer.startingTime = startingTick;
        }
    }
}
