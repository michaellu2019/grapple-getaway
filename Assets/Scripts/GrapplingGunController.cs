using UnityEngine;

public class GrapplingGunController : MonoBehaviour
{
    [Header("Scripts Ref:")]
    public GrapplingRopeController grappleRope;

    [Header("Layers Settings:")]
    [SerializeField] private bool grappleToAll = false;
    [SerializeField] private int grappableLayerNumber = 6;

    [Header("Transform Ref:")]
    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;
    public GameObject cursor;

    public Sprite xCursorMarker;
    public Sprite oCursorMarker;

    [Header("Physics Ref:")]
    public SpringJoint2D m_springJoint2D;
    public Rigidbody2D m_rigidbody;

    [Header("Rotation:")]
    [SerializeField] private bool rotateOverTime = true;
    [Range(0, 60)] [SerializeField] private float rotationSpeed = 4;

    [Header("Distance:")]
    [SerializeField] private bool hasMaxDistance = false;   // Set this value in GUI
    [SerializeField] private float maxDistance = 20;        // Set this value in GUI

    private enum LaunchType
    {
        Transform_Launch,
        Physics_Launch
    }

    [Header("Launching:")]
    [SerializeField] private bool launchToPoint = true;
    [SerializeField] private LaunchType launchType = LaunchType.Physics_Launch;
    [SerializeField] private float launchSpeed = 1;

    [Header("No Launch To Point")]
    [SerializeField] private bool autoConfigureDistance = false;
    [SerializeField] public float targetDistance = 3;
    [SerializeField] public float desiredDistance = 3;
    [SerializeField] private float targetFrequncy = 1;

    [HideInInspector] public Vector2 grapplePoint;
    [HideInInspector] public Vector2 grappleDistanceVector;

    private void Start()
    {
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
        cursor.transform.localScale = new Vector3(4, 4, 4);
    }

    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        ShowGrapplePoint();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SetGrapplePoint();
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            if (grappleRope.enabled)
            {
                RotateGun(grapplePoint, false);
            }
            else
            {
                RotateGun(mouseWorldPosition, true);
            }

            if (launchToPoint && grappleRope.isGrappling)
            {
                if (launchType == LaunchType.Transform_Launch)
                {
                    Vector2 firePointDistance = firePoint.position - gunHolder.localPosition;
                    Vector2 targetPos = grapplePoint - firePointDistance;
                    gunHolder.position = Vector2.Lerp(gunHolder.position, targetPos, Time.deltaTime * launchSpeed);
                    
                }
                
                Vector2 distanceVector = firePoint.position - gunHolder.position;
                m_springJoint2D.distance = distanceVector.magnitude * desiredDistance;
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            grappleRope.enabled = false;
            m_springJoint2D.enabled = false;
            m_rigidbody.gravityScale = 1;
        }
        else
        {
            RotateGun(mouseWorldPosition, true);
        }
    }

    void RotateGun(Vector3 lookPoint, bool allowRotationOverTime)
    {
        // return;
        Vector3 distanceVector = lookPoint - gunPivot.position;

        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        // Debug.Log("ANGLE " + angle.ToString());
        if (rotateOverTime && allowRotationOverTime)
        {
            gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotationSpeed);
        }
        else
        {
            gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void ShowGrapplePoint() 
    {
        if (grappleRope.enabled) {
            cursor.GetComponent<SpriteRenderer>().enabled = false;
        } else {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10;
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            SpriteRenderer cursorSpriteRenderer = cursor.GetComponent<SpriteRenderer>();
            
            Vector2 distanceVector = mouseWorldPosition - gunPivot.position;
            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, distanceVector.normalized);
            if (hit)
            {
                if (hit.transform.gameObject.layer == grappableLayerNumber || grappleToAll)
                {
                    if (Vector2.Distance(hit.point, firePoint.position) <= maxDistance || !hasMaxDistance)
                    {
                        cursorSpriteRenderer.enabled = true;
                        cursorSpriteRenderer.color = new Color(0, 1, 0.3f, 1);
                        cursorSpriteRenderer.sprite = oCursorMarker;
                        cursor.transform.position = hit.point;
                    } else 
                    {
                        cursorSpriteRenderer.enabled = true;
                        cursorSpriteRenderer.color = new Color(1, 0, 0, 1);
                        cursorSpriteRenderer.sprite = xCursorMarker;
                        cursor.transform.position = hit.point;
                    }
                } else 
                {
                    cursorSpriteRenderer.enabled = false;
                }
            } else 
            {
                cursorSpriteRenderer.enabled = false;
            }
        }
    }

    void SetGrapplePoint()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        
        Vector2 distanceVector = mouseWorldPosition - gunPivot.position;
        if (Physics2D.Raycast(firePoint.position, distanceVector.normalized))
        {
            RaycastHit2D hit = Physics2D.Raycast(firePoint.position, distanceVector.normalized);
            if (hit.transform.gameObject.layer == grappableLayerNumber || grappleToAll)
            {
                if (Vector2.Distance(hit.point, firePoint.position) <= maxDistance || !hasMaxDistance)
                {
                    grapplePoint = hit.point;
                    grappleDistanceVector = grapplePoint - (Vector2) gunPivot.position;
                    grappleRope.enabled = true;
                }
            }
        }
    }

    public void Grapple()
    {
        // Debug.Log("shot");
        m_springJoint2D.connectedAnchor = grapplePoint;
        m_springJoint2D.enabled = true;
        // m_springJoint2D.autoConfigureDistance = false;
        if (!launchToPoint && !autoConfigureDistance)
        {
            m_springJoint2D.distance = targetDistance;
            m_springJoint2D.frequency = targetFrequncy;
        }
        if (!launchToPoint)
        {
            if (autoConfigureDistance)
            {
                m_springJoint2D.autoConfigureDistance = true;
                m_springJoint2D.frequency = 0;
            }
            
            m_springJoint2D.connectedAnchor = grapplePoint;
            m_springJoint2D.enabled = true;
        }
        else
        {
            switch (launchType)
            {
                case LaunchType.Physics_Launch:
                    // Debug.Log("tf");
                    m_springJoint2D.connectedAnchor = grapplePoint;

                    Vector2 distanceVector = firePoint.position - gunHolder.position;

                    // m_springJoint2D.distance = distanceVector.magnitude; 
                    m_springJoint2D.distance = distanceVector.magnitude * targetDistance;
                    desiredDistance = targetDistance;
                    m_springJoint2D.frequency = launchSpeed;
                    m_springJoint2D.enabled = true;
                    break;
                case LaunchType.Transform_Launch:
                    // Debug.Log("tf2");
                    m_rigidbody.gravityScale = 0;
                    m_rigidbody.velocity = Vector2.zero;
                    break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (firePoint != null) {
            if (hasMaxDistance)
            {
                Gizmos.color = Color.green;
            } else {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawWireSphere(firePoint.position, maxDistance);
        }
    }

}