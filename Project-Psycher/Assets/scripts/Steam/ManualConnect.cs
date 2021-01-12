using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualConnect : MonoBehaviour
{
    ScrapperNetworkManager nm;
    // Start is called before the first frame update
    void Start()
    {
        nm = GetComponent<ScrapperNetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (nm)
            {
                var uri = new UriBuilder("127.0.0.1");
                nm.StartClient(uri.Uri);
            }
        }
    }
}
