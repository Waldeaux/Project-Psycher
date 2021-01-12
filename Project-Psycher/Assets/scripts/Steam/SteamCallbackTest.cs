using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamCallbackTest : MonoBehaviour
{
    protected Callback<GameOverlayActivated_t> overlayCallback;
    // Start is called before the first frame update
    void Start()
    {
        overlayCallback = Callback<GameOverlayActivated_t>.Create(LogOverlay);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SteamFriends.ActivateGameOverlay("Friends");
        }
    }

    void LogOverlay(GameOverlayActivated_t callback)
    {
        print("overlay");
    }
}
