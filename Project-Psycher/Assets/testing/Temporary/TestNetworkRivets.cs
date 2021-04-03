using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNetworkRivets : NetworkBehaviour
{
    public GameObject rivet;
    bool shotRivet;
    public List<Rivet> activeRivets;
    private Rivet prevRivet;
    GameObject target1;
    GameObject target2;
    public GameObject rivetRepresentative;
    
    void Start()
    {
        TargetStorage storage = GameObject.FindGameObjectWithTag("GameController").GetComponent<TargetStorage>();
        target1 = storage.target1;
        target2 = storage.target2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isLocalPlayer)
        {
            RaycastHit hitInfo;
            if (!shotRivet)
            {
                if (Physics.Raycast(transform.position, target1.transform.position - transform.position, out hitInfo))
                {
                    CmdCreateRivet(hitInfo.collider.gameObject, hitInfo.normal, hitInfo.point);
                }
            }
            else
            {
                if (Physics.Raycast(transform.position, target2.transform.position - transform.position, out hitInfo))
                {
                    CmdCreateRivet(hitInfo.collider.gameObject, hitInfo.normal, hitInfo.point);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.R) && isLocalPlayer)
        {
            foreach(Rivet rivet in activeRivets)
            {
                rivet.DestroyRivet();
            }
        }
    }

    [Command]
    public void CmdCreateRivet(GameObject target, Vector3 normal, Vector3 spawnPosition)
    {
        GameObject newRivet = GameObject.Instantiate(rivet);
        newRivet.transform.position = spawnPosition;
        newRivet.transform.rotation = Quaternion.LookRotation(normal);
        Rivet rivetScript = newRivet.GetComponent<Rivet>();
        rivetScript.AttachRivet(target);
        if (shotRivet)
        {
            prevRivet.ActivateRivet(newRivet);
            rivetScript.ActivateRivet(prevRivet.gameObject);
            if (!rivetScript.IsMobile())
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
        GameObject rivetRepresentative = GameObject.Instantiate(this.rivetRepresentative);
        rivetRepresentative.transform.position = spawnPosition;
        rivetRepresentative.transform.rotation = Quaternion.LookRotation(normal);
        NetworkServer.Spawn(rivetRepresentative);
        AttachRivetRepresentative(target, rivetRepresentative);
        rivetScript.representative = rivetRepresentative;
    }

    [ClientRpc]
    void AttachRivetRepresentative(GameObject target, GameObject rivet)
    {
        rivet.GetComponent<FixedJoint>().connectedBody = target.GetComponent<Rigidbody>();
    }
}
