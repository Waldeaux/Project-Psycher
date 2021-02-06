using UnityEngine;

public class PersonalPortal : MonoBehaviour
{
    public GameObject portalPrefab;
    private float timer;
    GameObject currentPortal;
    GameObject secondPortal;
    public PlayerPortalCamera cam;
    public void FixedUpdate()
    {
        if (transform.position.y < -20)
        {
            if (!currentPortal)
            {
                currentPortal = GameObject.Instantiate(portalPrefab);
                currentPortal.transform.rotation = Quaternion.LookRotation(transform.up);
                secondPortal = GameObject.Instantiate(portalPrefab);
                secondPortal.transform.position = new Vector3(0, 30, 0);
                secondPortal.transform.rotation = Quaternion.LookRotation(transform.up);
                SelectivePortal currentPortalScript = currentPortal.GetComponentInChildren<SelectivePortal>();
                SelectivePortal secondPortalScript = secondPortal.GetComponentInChildren<SelectivePortal>();
                if (currentPortalScript && secondPortalScript)
                {
                    currentPortalScript.linkedPortal = secondPortalScript;
                    secondPortalScript.linkedPortal = currentPortalScript;
                    currentPortalScript.allowedObject = this.gameObject;
                    cam.AddPortal(currentPortalScript);
                    cam.AddPortal(secondPortalScript);
                }
            }
            currentPortal.transform.position = transform.position - transform.up * 20 * Mathf.Max(0, (3f - timer));
            timer += Time.fixedDeltaTime;
        }
        else
        {
            if (currentPortal)
            {
                currentPortal.GetComponentInChildren<ExpandingPortal>().expanding = false;
                secondPortal.GetComponentInChildren<ExpandingPortal>().expanding = false;
                cam.RemovePortal(currentPortal.GetComponentInChildren<SelectivePortal>());
                cam.RemovePortal(secondPortal.GetComponentInChildren<SelectivePortal>());
                currentPortal = null;
                secondPortal = null;
            }
        }
    }
}
