using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class OpenOverlay : MonoBehaviour
{

    public void ActivateOverlay()
    {
        SteamFriends.ActivateGameOverlay("Friends");
    }
}
