using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using System;

public class Matchmaking : MonoBehaviour
{
    ScrapperNetworkManager networkManager;
    Callback<GameLobbyJoinRequested_t> lobbyJoinCallback;
    // Start is called before the first frame update
    void Start()
    {
        networkManager = GetComponent<ScrapperNetworkManager>();
        lobbyJoinCallback = Callback<GameLobbyJoinRequested_t>.Create(JoinLobby);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void JoinLobby(GameLobbyJoinRequested_t joinRequest)
    {
        uint ip;
        ushort port;
        CSteamID id;
        if(Steamworks.SteamMatchmaking.GetLobbyGameServer(joinRequest.m_steamIDLobby, out ip, out port, out id))
        {
            print(ip);
            print(port);
            var url = $"{ip}:{port}";
            print(url);
            var uri = new UriBuilder(url);
            if (networkManager)
            {
                networkManager.StartClient(uri.Uri);
            }
        }
    }
}
