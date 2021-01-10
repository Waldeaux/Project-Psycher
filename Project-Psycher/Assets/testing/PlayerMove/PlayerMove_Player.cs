using UnityEngine;

public class PlayerMove_Player : MonoBehaviour
{
    Rigidbody rb;
    public float maxSpeed;
    Vector3 internalVelocity;
    float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        internalVelocity = Vector3.zero;
        rb.velocity = transform.right * 20;
    }

    // Update is called once per frame
    void Update()
    {
        //Get move vector
        Vector3 moveVector = (transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical")).normalized * maxSpeed;

        Vector3 appliedAccelerationVector = Vector3.zero;

        //Get velocity relative to movement plane
        Vector3 planarMovement = Vector3.ProjectOnPlane(rb.velocity, transform.up);


        Vector3 externalVelocity = planarMovement - internalVelocity;

        //If current rigidbody velocity is not only composed of player inputs
        if (externalVelocity.magnitude > 1f) {

            //Separate move vector into components that are parallel relative to external velocity and perpendicular relative to external velocity
            Vector3 projectedMoveVector = Vector3.Project(moveVector, externalVelocity);
            Vector3 perpendicularMoveVector = moveVector - projectedMoveVector;

            //Perpendicular move vector is always used
            appliedAccelerationVector = perpendicularMoveVector + externalVelocity;
            internalVelocity = perpendicularMoveVector;


            float projectedDot = Vector3.Dot(projectedMoveVector.normalized, externalVelocity.normalized);

            //Parallel component is co-directional
            if (projectedDot > 0)
            {
                //Three cases:
                //external velocity magnitude is greater than max speed - magnitude of resultant parallel component calculated to 0
                //external velocity magnitude is less than max speed and the difference is greater than parallel component magnitude - magnitude of resultant parallel component calculated to magnitude of input parallel component
                //external velocity magnitude is less than max speed and the difference is less than parallel component magnitude - magnitude of resultant parallel component calculated to magnitude difference
                Vector3 calculatedProjectedVector = projectedMoveVector.normalized * Mathf.Min(projectedMoveVector.magnitude, Mathf.Max(0, maxSpeed - externalVelocity.magnitude));
                appliedAccelerationVector += calculatedProjectedVector;
                internalVelocity += calculatedProjectedVector;
            }

            //Move vector component that is parallel to external velocity are opposite directional
            else
            {
                //apply parallel component to acceleration vector
                appliedAccelerationVector += projectedMoveVector * Time.deltaTime;

                //internal velocity should only retain portion of parallel component that exceeds external velocity
                internalVelocity += Mathf.Max(projectedMoveVector.magnitude * Time.deltaTime - externalVelocity.magnitude, 0) * projectedMoveVector.normalized;
            }
        }

        //If current velocity is only player inputs
        else
        {
            //Save move vector and use move vector as velocity
            appliedAccelerationVector = moveVector;
            internalVelocity = moveVector;
        }
        rb.velocity = (appliedAccelerationVector);
        timer += Time.deltaTime;
        print(timer);
        print(rb.velocity);
    }

    bool GreaterThanOrEqualTo(Vector3 comparer, Vector3 comparee)
    {
        float dotProduct = Vector3.Dot(comparer.normalized, comparee.normalized);
        print(dotProduct);
        if (dotProduct != 1)
        {
            return false;
        }
        print(comparer.magnitude);
        print(comparee.magnitude);
        if(comparer.magnitude <= comparee.magnitude)
        {
            return false;
        }
        return true;
    }
}
