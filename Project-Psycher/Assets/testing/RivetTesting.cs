using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RivetTesting : MonoBehaviour
{

    public GameObject object1;
    public GameObject object2;
    RivetManagement rivetManagement;
    int testNum = 0;
    // Start is called before the first frame update
    void Start()
    {
        rivetManagement = GetComponent<RivetManagement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ShootRivet();
        }
    }

    void ShootRivet()
    {

        switch (testNum)
        {
            case (0):
                rivetManagement.ShootRivet(object1.transform.position - transform.position, transform.position);
                break;
            case (1):
                rivetManagement.ShootRivet(object2.transform.position - transform.position, transform.position);
                break;
        }
        testNum++;
    }
}
