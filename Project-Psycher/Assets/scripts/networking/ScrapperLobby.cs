using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapperLobby : MonoBehaviour
{
    public GameObject entryObject;
    List<PlayerLobbyEntry> entries;
    private void Start()
    {
        entries = new List<PlayerLobbyEntry>();
    }

    public void AddUser(int connId)
    {
        GameObject entryInst = GameObject.Instantiate(entryObject);
        PlayerLobbyEntry entryScript = entryInst.GetComponent<PlayerLobbyEntry>();
        print(entryScript);
        entryInst.transform.SetParent(this.transform);
        entries.Add(entryScript);
        RectTransform rt = entryInst.GetComponent<RectTransform>();
        rt.anchorMax = new Vector2(1, 1.125f - (entries.Count) * .125f);
        rt.anchorMin = new Vector2(0, 1 - (entries.Count) * .125f);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        entryScript.connId = connId;
    }

    public void RemoveUser(int connId)
    {
        int index = 0;
        while(index < entries.Count)
        {
            PlayerLobbyEntry entry = entries[index];
            if(entry.connId == connId)
            {
                entries.RemoveAt(index);
                Destroy(entry.gameObject);
            }
            else
            {
                index++;
            }
        }
        ReconfigureLobby();
    }

    void ReconfigureLobby()
    {
        int index = 0;
        foreach(PlayerLobbyEntry entry in entries)
        {
            RectTransform rt = entry.GetComponent<RectTransform>();
            rt.anchorMax = new Vector2(1, 1.125f - (entries.Count) * .125f);
            rt.anchorMin = new Vector2(0, 1 - (entries.Count) * .125f);
        }
    }

}
