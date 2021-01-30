using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsSaving : MonoBehaviour
{
    public List<SimulationData> simulationData;
    public List<Vector3> positionData;
    private Rigidbody rb;
    private bool saving;
    private int rewindFrame = 0;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        print(simulationData != null);
        simulationData = new List<SimulationData>();
        positionData = new List<Vector3>();
        int numFixedFrames = (int)(2 / Time.fixedDeltaTime);
        for(int x = 0; x < numFixedFrames; x++)
        {
            simulationData.Add(new SimulationData()
            {
                position = rb.position,
                rotation = rb.rotation,
                velocity = rb.velocity,
                angularVelocity = rb.angularVelocity
            });
            positionData.Add(rb.position);
        }
        print("awake");
        saving = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!Physics.autoSimulation)
        {
            return;
        }
            int index = simulationData.Count - 1;

            simulationData.RemoveAt(index);
            positionData.RemoveAt(index);
            simulationData.Insert(0, new SimulationData()
            {
                position = rb.position,
                rotation = rb.rotation,
                velocity = rb.velocity,
                angularVelocity = rb.angularVelocity
            });
            positionData.Insert(0, rb.position);
        
    }

    public void RewindFrames(int index)
    {
        rb.position = simulationData[index].position;
        transform.position = simulationData[index].position;
        rb.rotation = simulationData[index].rotation;
        rb.velocity = simulationData[index].velocity;
        rb.angularVelocity = simulationData[index].angularVelocity;
        rewindFrame = index;
    }

    public void CustomSimulate(PhysicsScene scene)
    {
        scene.Simulate(Time.fixedDeltaTime);
        print(rewindFrame);
        simulationData.RemoveAt(rewindFrame);

        simulationData.Insert(rewindFrame, new SimulationData()
        {
            position = rb.position,
            rotation = rb.rotation,
            velocity = rb.velocity,
            angularVelocity = rb.angularVelocity
        });
        rewindFrame--;
    }

    public void UpdateCurrentFrame()
    {
        simulationData.RemoveAt(rewindFrame);
        simulationData.Insert(rewindFrame, new SimulationData()
        {
            position = rb.position,
            rotation = rb.rotation,
            velocity = rb.velocity,
            angularVelocity = rb.angularVelocity
        });
    }
}
