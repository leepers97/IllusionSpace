using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_HCH : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotateSpeed = 40;
    public float jumpForce = 10;
    public bool isJump = false; 
    public Transform rotateTarget;

    // �ӵ� ����
    [Range(0.0f, 1.0f)]
    public float drag = 0.0f;

    Rigidbody rb;
    Vector3 lastMousePos;
    Vector3 currentRotation;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

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
    }

    void CharacterMove()
    {
        Vector3 newVelocity = rb.velocity;
        // ���� ȿ��
        if (!isJump)
        {
            newVelocity *= (1.0f - drag);
        }

        Vector3 f = rotateTarget.forward; f.y = 0.0f; f.Normalize();
        Vector3 r = rotateTarget.right; r.y = 0.0f; r.Normalize();

        newVelocity += f * Input.GetAxis("Vertical") * moveSpeed * Time.fixedDeltaTime;
        newVelocity += r * Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime;

        rb.velocity = newVelocity;
    }

    void CharacterJump()
    {
        // ���� ������ �� ���� �Ұ���
        if (isJump) return;
        //rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        transform.position += Vector3.up * jumpForce * Time.deltaTime;
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

    private void OnCollisionEnter(Collision collision)
    {
        // ���� ������ ���� ����
        if(collision.gameObject.layer == 6)
        {
            isJump = false;
        }
    }
}
