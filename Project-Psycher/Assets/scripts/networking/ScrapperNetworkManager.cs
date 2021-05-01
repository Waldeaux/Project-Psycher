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
    public NetworkManagerHUD networkManagerHUD;

    public GameControl gameControl;
    public override void Start()
    {
        base.Start();
        startingTick = DateTime.Now.Ticks;
        players = new List<NetworkPlayerTest>();
        if (!SteamManager.Initialized)
        {
            networkManagerHUD.enabled = true;
        }
    }
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);
        print("add player");
        NetworkPlayerTest newPlayer = conn.identity.gameObject.GetComponent<NetworkPlayerTest>();
        if (newPlayer != null)
        {
            newPlayer.connId = conn.connectionId;
            players.Add(newPlayer);
            if (newPlayer)
            {
                newPlayer.startingTime = startingTick;
            }
        }
        foreach(NetworkPlayerTest playerTest in players)
        {
            playerTest.RpcSetKinematic();
        }
        if (lobby)
        {
            lobby.AddUser(conn.connectionId);
        }
        print(gameControl);
        print(conn.identity.gameObject);
        if (gameControl)
        {
            gameControl.AddPlayer(conn.identity.gameObject);
        }
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);
        GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
        print("scene change");
        if (gameController)
        {
            gameControl = gameController.GetComponent<GameControl>();
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
