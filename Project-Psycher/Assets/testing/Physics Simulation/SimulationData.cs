using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationData
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 velocity;
    public Vector3 angularVelocity;
}

public class PlayerSimulationData : SimulationData
{
    public PlayerInputData inputData;
    public bool ragdoll;
}

public class PlayerInputData
{
    public Vector3 moveVector;
    public bool jump;
}