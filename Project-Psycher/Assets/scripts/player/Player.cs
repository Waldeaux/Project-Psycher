using UnityEngine;

public class Player : MonoBehaviour
{
    RivetManagement rivetManagement;
    Camera cam;
    KeyCode shootRivet = KeyCode.E;
    void Start()
    {
        rivetManagement = GetComponent<RivetManagement>();
        cam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(shootRivet))
        {
            ShootRivet();
        }
    }

    void ShootRivet()
    {
        if (rivetManagement)
        {
            rivetManagement.ShootRivet(cam.transform.forward, cam.transform.position);
        }
    }
}
