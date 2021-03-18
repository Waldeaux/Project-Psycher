using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopoffDependencies : PopoffReactions
{
    public List<GameObject> dependents;
    public override void Reaction(Vector3 gravity)
    {
        foreach(GameObject dependent in dependents)
        {
            Rigidbody rb = dependent.GetComponent<Rigidbody>();
            if (rb && rb.isKinematic)
            {
                rb.isKinematic = false;
                PopoffDependencies popoffDependents = dependent.GetComponent<PopoffDependencies>();
                if (popoffDependents)
                {
                    popoffDependents.Reaction(gravity);
                }
            }
        }
    }
}
