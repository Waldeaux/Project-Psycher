using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using System;

public class Matchmaking : MonoBehaviour
{
    ScrapperNetworkManager networkManager;
    Callback<GameLobbyJoinRequested_t> lobbyJoinCallback;
    Callback<GameOverlayActivated_t> overlayCallback;
    // Start is called before the first frame update
    void Start()
    {
        networkManager = GetComponent<ScrapperNetworkManager>();
        lobbyJoinCallback = Callback<GameLobbyJoinRequested_t>.Create(JoinLobby);
        overlayCallback = Callback<GameOverlayActivated_t>.Create(LogOverlay);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void LogOverlay(GameOverlayActivated_t callback)
    {
        Debug.Log("overlay");
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

    public void CreateLobby()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 2);
        networkManager.StartHost();
    }
}
