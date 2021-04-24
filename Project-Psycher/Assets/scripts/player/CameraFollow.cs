using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;

    private enum State { immobile, follow, stay, offset}
    private State currentState = State.stay;
    private float rightAnglePeriod = .25f;
    void Start()
    {
        target = transform.parent.gameObject;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        switch (currentState)
        {
            case (State.immobile):
                SmoothTranslation(target.transform.position, Quaternion.LookRotation(Vector3.forward, Vector3.up));
                break;
            case (State.follow):
                Vector3 offset = transform.position - target.transform.position;
                offset = Vector3.ProjectOnPlane(offset, Vector3.up);
                if (offset.magnitude > 10)
                {
                    offset = offset.normalized * 10;
                }
                offset += Vector3.up;
                Vector3 targetPoint = target.transform.position + offset;
                transform.position = Vector3.Lerp(transform.position, targetPoint, Time.deltaTime);
                transform.LookAt(target.transform);
                break;
            case (State.offset):
                SmoothTranslation(target.transform.position + target.transform.up *3, Quaternion.LookRotation(-target.transform.up, target.transform.forward));
                break;
        }
    }

    public void SwitchToFollow()
    {
        transform.SetParent(null);
        currentState = State.follow;
    }

    public void SwitchToImmobile()
    {
        currentState = State.immobile;
    }

    public void SwitchToStay()
    {
        transform.SetParent(target.transform);
        currentState = State.stay;
    }

    public void SmoothTranslation(Vector3 targetPosition, Quaternion targetRotation)
    {
        Quaternion remainingRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360);

        float totalAngle = Quaternion.Angle(remainingRotation, transform.rotation);
        if (totalAngle > 0)
        {
            Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 90 / rightAnglePeriod);
            float movingAngle = Quaternion.Angle(newRotation, transform.rotation);
            float angle = movingAngle / totalAngle;
            transform.rotation = newRotation;
            Vector3 immobileOffset = (targetPosition - transform.position);
            transform.position = transform.position + immobileOffset * Mathf.Min(1, angle);
        }
        else
        {
            SwitchToStay();
        }
    }
}