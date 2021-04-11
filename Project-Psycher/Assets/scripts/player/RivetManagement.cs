using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RivetManagement : NetworkBehaviour
{
    public GameObject rivetPrefab;
    Rivet prevRivet;
    bool shotRivet = false;
    public List<Rivet> activeRivets;
    public GameObject rivetRepresentative;
    enum MobileResult
    {
        PreviousObject,
        CurrentObject,
        Neither
    };
    public void ShootRivet(Vector3 direction, Vector3 position)
    {
        RaycastHit hitInfo;
        if(Physics.Raycast(position, direction))
        {
            CmdCreateRivet(position, direction);
        }

    }

    [Command]
    public void CmdCreateRivet(Vector3 position, Vector3 direction)//GameObject target, Vector3 normal, Vector3 spawnPosition)
    {
        RaycastHit hitInfo;
        if (!Physics.Raycast(position, direction, out hitInfo))
        {
            return;
        }
        Vector3 spawnPosition = hitInfo.point;
        Vector3 normal = hitInfo.normal;
        GameObject target = hitInfo.collider.gameObject;
        GameObject newRivet = GameObject.Instantiate(rivetPrefab);
        Rivet rivetScript = newRivet.GetComponent<Rivet>();
        RivetFunctions.CreateRivet(spawnPosition, normal, target, newRivet, shotRivet, prevRivet);
        if (!shotRivet)
        {
            prevRivet = rivetScript;
            activeRivets.Add(rivetScript);
        }
        GameObject rivetRepresentative = GameObject.Instantiate(this.rivetRepresentative);
        rivetRepresentative.transform.position = spawnPosition;
        rivetRepresentative.transform.rotation = Quaternion.LookRotation(normal);
        NetworkServer.Spawn(rivetRepresentative);
        if (shotRivet)
        {
            AttachRivetRepresentative(target, rivetRepresentative, prevRivet.representative);
            prevRivet = null;
        }
        else
        {
            AttachRivetRepresentative(target, rivetRepresentative, null);
        }
        rivetScript.representative = rivetRepresentative;
        shotRivet = !shotRivet;
    }
    public void CreateRivet(Vector3 position, Vector3 spawnNormal, GameObject targetObject, GameObject newRivet, bool shotRivet, Rivet prevRivet)
    {
        newRivet.transform.position = position;
        newRivet.transform.rotation = Quaternion.LookRotation(spawnNormal);
        Rivet rivetScript = newRivet.GetComponent<Rivet>();
        rivetScript.AttachRivet(targetObject);
        if (shotRivet)
        {
            prevRivet.ActivateRivet(newRivet);
            rivetScript.ActivateRivet(prevRivet.gameObject);
            if (!rivetScript.IsMobile())
            {
                if (!prevRivet.IsMobile())
                {
                    prevRivet.PopoffObject();
                }
            }
        }
    }
    [ClientRpc]
    void AttachRivetRepresentative(GameObject target, GameObject rivet, GameObject pairedRepresentative)
    {
        rivet.GetComponent<FixedJoint>().connectedBody = target.GetComponent<Rigidbody>();
        if(pairedRepresentative != null && isLocalPlayer)
        {
            rivet.GetComponent<RivetRepresentative>().CreateInstance(pairedRepresentative);
        }
    }

    [Command]
    public void CmdDestroyAllRivets()
    {
        foreach(Rivet rivet in activeRivets)
        {
            rivet.DestroyRivet();
        }
        shotRivet = false;
        activeRivets = new List<Rivet>();
    }
}
