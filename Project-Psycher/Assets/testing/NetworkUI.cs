using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUI : NetworkBehaviour
{
    [SyncVar(hook = "SetParent")]
    public string parentName;

    [SyncVar(hook = "UpdateCount")]
    public int count;

    [SyncVar(hook ="UpdateText")]
    public string text;

    public Text textComponent;

    void SetParent(string oldName, string name)
    {
        GameObject canvas = GameObject.Find(name);
        GameObject instance = this.gameObject;
        instance.transform.SetParent(canvas.transform);
        UpdatePosition(count);
    }

    void UpdateCount(int oldCount, int count)
    {
        UpdatePosition(count);
    }

    public void UpdatePosition(int count)
    {
        GameObject instance = this.gameObject;
        RectTransform rt = instance.GetComponent<RectTransform>();
        rt.anchorMax = new Vector2(1, 1 - count * .1f);
        rt.anchorMin = new Vector2(0, .9f - count * .1f);
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    public void UpdateText(string oldValue, string newValue)
    {
        if (textComponent)
        {
            textComponent.text = newValue;
        }
    }
}
