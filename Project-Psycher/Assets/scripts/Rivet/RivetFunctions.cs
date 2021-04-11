using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

    static class RivetFunctions 
    {
    static public void CreateRivet(Vector3 position, Vector3 spawnNormal, GameObject targetObject, GameObject newRivet, bool shotRivet, Rivet prevRivet)
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
                //prevRivet.ToggleGravity();
                if (!prevRivet.IsMobile())
                {
                    prevRivet.PopoffObject();
                }
            }
            else if (!prevRivet.IsMobile())
            {
                //rivetScript.ToggleGravity();
            }
        }
    }
}
