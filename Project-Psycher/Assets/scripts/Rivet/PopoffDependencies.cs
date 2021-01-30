﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopoffDependencies : MonoBehaviour
{
    public List<GameObject> dependents;
    public void PopoffDependents()
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
                    popoffDependents.PopoffDependents();
                }
            }
        }
    }
}
