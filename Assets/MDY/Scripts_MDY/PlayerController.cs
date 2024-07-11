using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 20f;
    public float lookSpeed = 2f;
    public float jumpForce = 5f;
    public float fixedGrabDistance = 1f;
    public Image crosshairImage;
    public float dropMoveSpeedMultiplier = 10f;
    public float collisionThreshold = 0.01f;
    public float minLookAngle = -45f;
    public float maxLookAngle = 45f;
    public float minScale = 0.1f;
    public float groundCheckDistance = 0.1f;

    private Camera playerCamera;
    private GameObject grabbedObject;
    private GameObject clonedObject;
    private bool isGrabbing = false;
    private bool isDropping = false;
    private Vector3 initialScale;
    private float initialDistance;
    private Vector3 dropDirection;
    private Collider grabbedCollider;
    private Rigidbody rb;
    private float rotationX = 0f;
    private bool isGrounded = true;
    private Collider playerCollider;

    void Start()
    {
        playerCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();

        if (crosshairImage != null)
        {
            crosshairImage.rectTransform.position = new Vector2(Screen.width / 2, Screen.height / 2);
        }
    }

    void Update()
    {
        HandleMouseLook();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (isGrabbing)
            {
                PrepareToDropObject();
            }
            else
            {
                TryGrabObject();
            }
        }

        if (isGrabbing && grabbedObject != null)
        {
            MoveObjectToCrosshair();
            VisualizeRayThroughObject();
        }

        if (isDropping)
        {
            ContinueDropObject();
        }
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime;
        float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.fixedDeltaTime;
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        rb.MovePosition(rb.position + move);
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, minLookAngle, maxLookAngle);

        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    void TryGrabObject()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null && hit.collider.CompareTag("Grab"))
            {
                grabbedObject = hit.collider.gameObject;
                grabbedCollider = grabbedObject.GetComponent<Collider>();
                grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                isGrabbing = true;

                Physics.IgnoreCollision(grabbedCollider, playerCollider, true);

                initialDistance = Vector3.Distance(playerCamera.transform.position, grabbedObject.GetComponent<Renderer>().bounds.center);
                initialScale = grabbedObject.transform.localScale;
            }
        }
    }

    void PrepareToDropObject()
    {
        if (grabbedObject != null)
        {
            isGrabbing = false;
            isDropping = true;

            Vector3 objectCenter = grabbedObject.GetComponent<Renderer>().bounds.center;
            dropDirection = (objectCenter - playerCamera.transform.position).normalized;

            Physics.IgnoreCollision(grabbedCollider, playerCollider, false);

            // Check camera rotation before cloning the object
            float cameraRotationX = playerCamera.transform.localRotation.eulerAngles.x;

            // Clone the object only if camera is looking downwards (rotationX >= 90)
            if (cameraRotationX >= 90f)
            {
                // Clone the object and keep it in front of the camera
                clonedObject = Instantiate(grabbedObject, grabbedObject.transform.position, grabbedObject.transform.rotation);
                clonedObject.transform.localScale = grabbedObject.transform.localScale;
                clonedObject.GetComponent<Rigidbody>().isKinematic = true;

                // Disable the collider of the cloned object to prevent collisions
                Collider clonedCollider = clonedObject.GetComponent<Collider>();
                if (clonedCollider != null)
                {
                    clonedCollider.enabled = false;
                }

                StartCoroutine(DestroyCloneAfterDelay(0.25f)); // Set delay to 0.1 seconds
            }
        }
    }

    IEnumerator DestroyCloneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (clonedObject != null)
        {
            Destroy(clonedObject);
        }
    }

    void ContinueDropObject()
    {
        if (grabbedObject != null)
        {
            float step = moveSpeed * dropMoveSpeedMultiplier * Time.deltaTime;
            Vector3 currentPos = grabbedObject.GetComponent<Renderer>().bounds.center;
            float currentDistance = Vector3.Distance(playerCamera.transform.position, currentPos);

            float scaleMultiplier = currentDistance / initialDistance;
            Vector3 newScale = new Vector3(
                Mathf.Max(initialScale.x * scaleMultiplier, minScale),
                Mathf.Max(initialScale.y * scaleMultiplier, minScale),
                Mathf.Max(initialScale.z * scaleMultiplier, minScale)
            );
            grabbedObject.transform.localScale = newScale;

            Vector3 nextPosition = grabbedObject.transform.position + dropDirection * step;

            RaycastHit hit;
            if (Physics.Raycast(nextPosition, Vector3.down, out hit, groundCheckDistance))
            {
                if (Vector3.Dot(hit.normal, Vector3.up) > 0.7f)
                {
                    nextPosition.y = hit.point.y + (grabbedObject.GetComponent<Collider>().bounds.extents.y);
                }
            }

            if (IsPositionValid(nextPosition))
            {
                grabbedObject.transform.position = nextPosition;
            }
            else
            {
                isDropping = false;
                grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
                grabbedObject = null;
            }
        }
    }

    bool IsPositionValid(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapBox(position, grabbedCollider.bounds.extents, grabbedObject.transform.rotation);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject != grabbedObject)
            {
                return false;
            }
        }
        return true;
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, GetComponent<Collider>().bounds.extents.y + 0.1f);
    }

    void DropObject()
    {
        if (grabbedObject != null)
        {
            grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
            grabbedObject = null;
            isGrabbing = false;
        }
    }

    void MoveObjectToCrosshair()
    {
        Vector3 targetPosition = playerCamera.transform.position + playerCamera.transform.forward * fixedGrabDistance;
        grabbedObject.transform.position = targetPosition;

        float scaleMultiplier = fixedGrabDistance / initialDistance;
        grabbedObject.transform.localScale = initialScale * scaleMultiplier;
    }

    void VisualizeRayThroughObject()
    {
        if (grabbedObject != null)
        {
            Vector3 objectCenter = grabbedObject.GetComponent<Renderer>().bounds.center;
            Vector3 direction = objectCenter - playerCamera.transform.position;
            float maxDistance = 300f;

            Debug.DrawLine(playerCamera.transform.position, playerCamera.transform.position + direction * maxDistance, Color.red);
        }
    }
}