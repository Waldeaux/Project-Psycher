using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;

public class SteamName : MonoBehaviour
{

    void Start()
    {
        if (SteamManager.Initialized)
        {
            Text text = GetComponent<Text>();
            text.text = SteamFriends.GetPersonaName();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

}
