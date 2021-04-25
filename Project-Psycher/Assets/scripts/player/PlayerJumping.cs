using UnityEngine;

public class PlayerJumping : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 gravity;
    public bool debug;

    private enum GravityMode
    {
        standard,
        jumping
    }
    private Vector3 jumpCastExtents = new Vector3(.5f, .1f, .5f);

    private GravityMode currentMode;
    public GameObject target;
    public Vector3 targetPoint;
    public GameObject indicator;
    private Vector3 targetUp;

    private Player player;

    private Camera cam;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gravity = -transform.up;
        cam = GetComponentInChildren<Camera>();
        indicator.transform.SetParent(null);
        player = GetComponent<Player>();
        if (!Input.GetKey(KeyCode.Mouse1))
        {
            indicator.SetActive(false);
        }
    }

    void Update()
    {
        switch (currentMode)
        {
            case (GravityMode.standard):
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    indicator.SetActive(true);
                }
                if (Input.GetKeyUp(KeyCode.Mouse1))
                {
                    indicator.SetActive(false);
                }
                UpdateIndicator();
                break;
            case (GravityMode.jumping):
                gravity = (targetPoint - transform.position).normalized;
                Correction();
                break;
        }
        rb.AddForce(gravity * Time.deltaTime * 9.8f, ForceMode.VelocityChange);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Physics.BoxCast(transform.position, jumpCastExtents, -transform.up, transform.rotation, .9f))
            {
                if (Input.GetKey(KeyCode.Mouse1))
                {
                    RaycastHit hitInfo;
                    Physics.Raycast(transform.position, indicator.transform.position - transform.position, out hitInfo);
                    targetUp = hitInfo.normal;
                    targetPoint = hitInfo.point;
                    SwitchToJumping();
                }
                else
                {
                    Jump();
                }
            }
        }
    }

    public void FixedUpdate()
    {
    }
    private void Jump()
    {
        print("jump successful");
        Vector3 currentUp = Vector3.Project(rb.velocity, transform.up);
        rb.AddForce(transform.up * (5 - currentUp.magnitude), ForceMode.VelocityChange);
    }

    public void SwitchToStandard()
    {
        gravity = -transform.up;
        currentMode = GravityMode.standard;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    void UpdateIndicator()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, cam.transform.forward, out hitInfo))
        {
            indicator.transform.position = hitInfo.point + hitInfo.normal * .1f;
            Quaternion rotation = Quaternion.FromToRotation(transform.up, hitInfo.normal);
            indicator.transform.rotation = Quaternion.LookRotation(rotation * transform.forward, hitInfo.normal);
        }
    }
    void SwitchToJumping()
    {
        currentMode = GravityMode.jumping;
        gravity = (targetPoint - transform.position).normalized;
        rb.AddForce(gravity * 2, ForceMode.VelocityChange);
        player.SwitchToJumping();

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (currentMode == GravityMode.jumping)
        {
            SwitchToStandard();
        }
    }

    private void Correction()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(indicator.transform.forward, indicator.transform.up), Time.deltaTime * 180);
    }
}
