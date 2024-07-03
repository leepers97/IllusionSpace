using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_HCH : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotateSpeed = 40;
    public float jumpForce = 10;
    public bool isJump = false;
    float jumpMoveSpeedClamp = 10;

    public Transform rotateTarget;
    BoxCollider feetCol;

    // �ӵ� ����
    [Range(0.0f, 1.0f)]
    public float drag = 0.0f;

    Rigidbody rb;
    Vector3 lastMousePos;
    Vector3 currentRotation;

    // �׽�Ʈ�� �ڷ���Ʈ
    public Transform[] TelePos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        feetCol = GetComponentInChildren<BoxCollider>(); // ���߿� �������� ���� ��������

        lastMousePos = Input.mousePosition;
        currentRotation = rotateTarget.rotation.eulerAngles;
    }

    void FixedUpdate()
    {
        // ĳ���� ������
        CharacterMove();
    }

    // Update is called once per frame
    void Update()
    {
        // ī�޶� ȸ��
        CameraRotaion();

        // ĳ���� ����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CharacterJump();
        }

        // �׽�Ʈ�� �ڷ���Ʈ
        TestTeleport();
    }

    void CharacterMove()
    {
        Vector3 newVelocity = rb.velocity;
        // ���� ȿ��
        if (!isJump) newVelocity *= (1.0f - drag);

        Vector3 f = rotateTarget.forward; f.y = 0.0f; f.Normalize();
        Vector3 r = rotateTarget.right; r.y = 0.0f; r.Normalize();

        newVelocity += f * Input.GetAxis("Vertical") * moveSpeed * Time.fixedDeltaTime;
        newVelocity += r * Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime;

        // ���� ���� �� �̵� �ӵ��� ����
        if (isJump)
        {
            newVelocity.x = Mathf.Clamp(newVelocity.x, -moveSpeed / jumpMoveSpeedClamp, moveSpeed / jumpMoveSpeedClamp);
            newVelocity.z = Mathf.Clamp(newVelocity.z, -moveSpeed / jumpMoveSpeedClamp, moveSpeed / jumpMoveSpeedClamp);
        }

        rb.velocity = newVelocity;
    }

    void CharacterJump()
    {
        // ���� ������ �� ���� �Ұ���
        if (isJump) return;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isJump = true;
    }

    void CameraRotaion()
    {
        Vector2 mouseDelta = Input.mousePosition - lastMousePos;
        lastMousePos = Input.mousePosition;

        currentRotation.y += mouseDelta.x * rotateSpeed * Time.deltaTime;
        currentRotation.x = Mathf.Clamp(currentRotation.x - mouseDelta.y * rotateSpeed * Time.deltaTime, -70.0f, 70.0f);

        rotateTarget.rotation = Quaternion.Euler(currentRotation);
    }

    void TestTeleport()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            transform.position = TelePos[0].position;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            transform.position = TelePos[1].position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ���� ������ ���� ����
        //if(collision.gameObject.layer == 7)
        //{
        //    isJump = false;
        //}

        isJump = false;
    }
}
