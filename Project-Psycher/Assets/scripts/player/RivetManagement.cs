using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RivetManagement : MonoBehaviour
{
    public GameObject rivetPrefab;
    Rivet prevRivet;
    bool shotRivet = false;
    public void ShootRivet(Vector3 direction, Vector3 position)
    {
        RaycastHit hitInfo;
        if(Physics.Raycast(position, direction, out hitInfo))
        {
            CreateRivet(hitInfo.collider.gameObject, hitInfo.normal, hitInfo.point);
        }

    }
    public void CreateRivet(GameObject target, Vector3 normal, Vector3 spawnPosition)
    {
        GameObject newRivet = GameObject.Instantiate(rivetPrefab);
        newRivet.transform.position = spawnPosition;
        newRivet.transform.rotation = Quaternion.LookRotation(normal);
        Rivet rivetScript = newRivet.GetComponent<Rivet>();
        rivetScript.AttachRivet(target);
        if (shotRivet)
        {
            prevRivet.ActivateRivet(newRivet);
            rivetScript.ActivateRivet(prevRivet.gameObject);
            print(prevRivet.mobile);
            print(rivetScript.mobile);
            if(!rivetScript.mobile)
            {
                prevRivet.TurnOffGravity();
                if (!prevRivet.mobile)
                {
                    prevRivet.PopoffObject();
                }
            }
            else if (!prevRivet.mobile)
            {
                rivetScript.TurnOffGravity();
            }
            prevRivet = null;
        }
        else
        {
            prevRivet = rivetScript;
        }
        shotRivet = !shotRivet;

    }
}
