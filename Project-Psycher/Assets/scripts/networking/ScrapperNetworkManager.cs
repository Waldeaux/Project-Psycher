using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapperNetworkManager : NetworkManager
{
    long startingTick;
    List<NetworkPlayerTest> players;
    public ScrapperLobby lobby;
    public override void Start()
    {
        base.Start();
        startingTick = DateTime.Now.Ticks;
        players = new List<NetworkPlayerTest>();
        
    }
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
        print(conn.connectionId);
        NetworkPlayerTest newPlayer = conn.identity.gameObject.GetComponent<NetworkPlayerTest>();
        newPlayer.connId = conn.connectionId;
        players.Add(newPlayer);
        foreach(NetworkPlayerTest playerTest in players)
        {
            playerTest.RpcSetKinematic();
        }
        if (newPlayer)
        {
            newPlayer.startingTime = startingTick;
        }
        if (lobby)
        {
            lobby.AddUser(conn.connectionId);
        }
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        int index = 0;
        while(index  < players.Count)
        {
            NetworkPlayerTest playerTest = players[index];
            if(playerTest.connId == conn.connectionId)
            {
                players.Remove(playerTest);
                if (lobby)
                {
                    lobby.RemoveUser(conn.connectionId);
                }
            }
            else
            {
                index++;
            }
        }

    }
}
