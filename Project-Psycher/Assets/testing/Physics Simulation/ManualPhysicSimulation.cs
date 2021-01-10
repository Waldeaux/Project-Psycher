using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManualPhysicSimulation : MonoBehaviour
{
    PhysicsScene scene1Physics;
    int rewindFrame;
    GameObject[] physicsObjects;
    // Start is called before the first frame update
    void Start()
    {
        /*
        foreach (GameObject gameObject in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                    rb.AddForce(new Vector3(1, 0), ForceMode.VelocityChange);
            }
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (scene1Physics != null && Input.GetKey(KeyCode.S))
        {
            scene1Physics.Simulate(Time.fixedDeltaTime);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            SimulateScene();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            AdvanceSimulation();
        }
    }

    void SimulateScene()
    {
        CreateSceneParameters csp = new CreateSceneParameters(LocalPhysicsMode.Physics3D);
        Scene scene1 = SceneManager.CreateScene("SimulationScene", csp);

        scene1Physics = scene1.GetPhysicsScene();

        foreach (GameObject gameObject in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (gameObject.GetComponent<Collider>())
            {
                GameObject newObject = GameObject.Instantiate(gameObject);
                SceneManager.MoveGameObjectToScene(newObject, scene1);
                newObject.name = "duplicate";
                PhysicsSaving physicsSaving = newObject.GetComponent<PhysicsSaving>();
                if (physicsSaving != null)
                {
                    RelatePhysicsObject relatedObject = newObject.AddComponent<RelatePhysicsObject>();
                    relatedObject.originalObject = gameObject;
                    PhysicsSaving oldPhysicsSaving = gameObject.GetComponent<PhysicsSaving>();
                    print(oldPhysicsSaving.simulationData[50].position);
                    physicsSaving.simulationData = oldPhysicsSaving.simulationData;
                    print(physicsSaving.simulationData[50].position);
                    physicsSaving.RewindFrames(50);
                    Rigidbody rb = newObject.GetComponent<Rigidbody>();
                    rb.AddForce(new Vector3(1, 0), ForceMode.VelocityChange);
                }
                gameObject.SetActive(false);
            }
        }

        rewindFrame = 50;
        physicsObjects = scene1.GetRootGameObjects();

        Physics.autoSimulation = false;

        while(rewindFrame > -1)
        {
            AdvanceSimulation();
        }
        Physics.autoSimulation = true;
        foreach (GameObject gameObject in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            gameObject.SetActive(true);
        }
        
        foreach(GameObject gameObject in scene1.GetRootGameObjects())
        {
            RelatePhysicsObject relatePhysicsObject = gameObject.GetComponent<RelatePhysicsObject>();
            if (relatePhysicsObject)
            {
                GameObject originalObject = relatePhysicsObject.originalObject;
                PhysicsSaving originalPhysics = originalObject.GetComponent<PhysicsSaving>();
                PhysicsSaving objectPhysics = gameObject.GetComponent<PhysicsSaving>();
                originalPhysics.simulationData = objectPhysics.simulationData;
                foreach (SimulationData data in objectPhysics.simulationData)
                {
                    print(data.position);
                }
                Rigidbody rb = originalObject.GetComponent<Rigidbody>();
                SimulationData currentData = objectPhysics.simulationData[0];
                rb.velocity = currentData.velocity;
                rb.angularVelocity = currentData.angularVelocity;
                rb.position = currentData.position;
                rb.rotation = currentData.rotation;
            }
            Destroy(gameObject);
        }
    }

    void AdvanceSimulation()
    {

        foreach (GameObject gameObject in physicsObjects)
        {
            PhysicsSaving physicsSaving = gameObject.GetComponent<PhysicsSaving>();
            if (physicsSaving != null)
            {
                physicsSaving.CustomSimulate(scene1Physics);
            }
        }
        rewindFrame--;
    }
}
