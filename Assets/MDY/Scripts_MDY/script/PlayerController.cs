using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpHeight = 2.0f;
    public float gravity = -9.81f;
    public float mouseSensitivity = 100.0f;
    public Transform cameraTransform;
    public GameObject crosshair;
    public Transform backgroundObject;  // 배경 물체

    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;
    private GameObject grabbedObject;
    private float xRotation = 0f;
    private Vector3 initialScale;
    private float initialDistance;
    private Vector3 initialPosition;
    private float backgroundInitialDistance;
    private bool isDragging = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;  // 커서를 숨깁니다.
    }

    void Update()
    {
        // 마우스 입력을 통한 카메라 회전
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // 플레이어 이동
        isGrounded = characterController.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        characterController.Move(move * speed * Time.deltaTime);

        // 점프
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // 중력 적용
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        // 물체 잡기 및 드래그
        if (Input.GetMouseButtonDown(0))
        {
            TryGrabObject();
        }

        if (Input.GetMouseButtonUp(0))
        {
            ReleaseObject();
        }

        if (grabbedObject != null)
        {
            DragObject();
            UpdateObjectScale();
        }
    }

    void TryGrabObject()
    {
        Ray ray = cameraTransform.GetComponent<Camera>().ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Draggable"))
            {
                grabbedObject = hit.collider.gameObject;
                grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                initialScale = grabbedObject.transform.localScale;
                initialDistance = Vector3.Distance(cameraTransform.position, grabbedObject.transform.position);
                initialPosition = grabbedObject.transform.position;
                backgroundInitialDistance = Vector3.Distance(cameraTransform.position, backgroundObject.position);
                isDragging = true;
            }
        }
    }

    void ReleaseObject()
    {
        if (grabbedObject != null)
        {
            grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
            grabbedObject = null;
            isDragging = false;
        }
    }

    void DragObject()
    {
        Ray ray = cameraTransform.GetComponent<Camera>().ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        Vector3 targetPosition = ray.GetPoint(initialDistance);
        grabbedObject.transform.position = targetPosition;
    }

    void UpdateObjectScale()
    {
        if (isDragging)
        {
            // 배경 물체와 플레이어 사이의 거리 계산
            float currentBackgroundDistance = Vector3.Distance(cameraTransform.position, backgroundObject.position);

            // 물체와 배경 사이의 거리 계산
            float objectToBackgroundDistance = Vector3.Distance(grabbedObject.transform.position, backgroundObject.position);

            // 배경과 물체 사이의 거리 변화에 따라 크기 조정
            float scaleMultiplier = backgroundInitialDistance / objectToBackgroundDistance;
            grabbedObject.transform.localScale = initialScale * scaleMultiplier;
        }
    }
}
