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

    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;
    private GameObject grabbedObject;
    private float xRotation = 0f;
    private bool isGrabbing = false; // 물체가 그랩된 상태를 유지하는 변수
    private float initialGrabDistance; // 물체와 카메라 사이의 초기 거리
    private Vector3 initialScale; // 물체의 초기 크기

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
            if (isGrabbing)
            {
                ReleaseObject();
            }
            else
            {
                TryGrabObject();
            }
        }

        if (isGrabbing && grabbedObject != null)
        {
            UpdateObjectPosition();
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
                grabbedObject.GetComponent<ObjectCloner>().SetGrabbed(true); // 물체가 잡혔음을 설정
                initialGrabDistance = Vector3.Distance(cameraTransform.position, grabbedObject.transform.position);
                initialScale = grabbedObject.transform.localScale; // 물체의 초기 크기 저장
                isGrabbing = true; // 물체가 그랩된 상태로 변경

                // 물체를 바로 그 위치로 이동시키되 크기는 변하지 않음
                UpdateObjectPosition();
            }
        }
    }

    void ReleaseObject()
    {
        if (grabbedObject != null)
        {
            grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
            grabbedObject.GetComponent<ObjectCloner>().SetGrabbed(false); // 물체가 놓였음을 설정
            grabbedObject = null;
            isGrabbing = false; // 물체가 그랩되지 않은 상태로 변경
        }
    }

    void UpdateObjectPosition()
    {
        if (grabbedObject == null) return;

        Ray ray = cameraTransform.GetComponent<Camera>().ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        Vector3 targetPosition = ray.GetPoint(initialGrabDistance);

        grabbedObject.transform.position = targetPosition;
    }
}
