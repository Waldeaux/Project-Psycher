using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RivetManagement : MonoBehaviour
{
    public GameObject rivetPrefab;
    Rivet prevRivet;
    bool shotRivet = false;
    public List<Rivet> activeRivets;
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
            if(!rivetScript.IsMobile())
            {
                prevRivet.ToggleGravity();
                if (!prevRivet.IsMobile())
                {
                    prevRivet.PopoffObject();
                }
            }
            else if (!prevRivet.IsMobile())
            {
                rivetScript.ToggleGravity();
            }
            prevRivet = null;
        }
        else
        {
            prevRivet = rivetScript;
            activeRivets.Add(rivetScript);
        }
        shotRivet = !shotRivet;
    }

    public void DestroyAllRivets()
    {
        foreach(Rivet rivet in activeRivets)
        {
            rivet.DestroyRivet();
        }
        shotRivet = false;
        activeRivets = new List<Rivet>();
    }
}
