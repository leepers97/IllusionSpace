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
    ConstantForce cf;

    // 속도 감쇠
    [Range(0.0f, 1.0f)]
    public float drag = 0.0f;

    Rigidbody rb;
    Vector3 currentRotation;

    // 우클릭 시 카메라 이동 막기 위한 변수
    public bool isCamMove = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cf = GetComponent<ConstantForce>(); // 점프시에 켜서 중력 강하게 먹이기
        feetCol = GetComponentInChildren<BoxCollider>(); // 나중에 점프가능 판정 수정예정

        currentRotation = rotateTarget.rotation.eulerAngles;
    }

    void FixedUpdate()
    {
        // 캐릭터 움직임
        CharacterMove();

        // 캐릭터 방향
        CharacterRoatation();
    }

    // Update is called once per frame
    void Update()
    {
        // 카메라 회전
        CameraRotaion();

        // 캐릭터 점프
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CharacterJump();
        }
    }

    void CharacterMove()
    {
        Vector3 newVelocity = rb.velocity;
        // 감쇠 효과
        if (!isJump) newVelocity *= (1.0f - drag);

        Vector3 f = rotateTarget.forward; f.y = 0.0f; f.Normalize();
        Vector3 r = rotateTarget.right; r.y = 0.0f; r.Normalize();

        newVelocity += f * Input.GetAxis("Vertical") * moveSpeed * Time.fixedDeltaTime;
        newVelocity += r * Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime;

        // 점프 중일 때 이동 속도를 제한
        if (isJump)
        {
            newVelocity.x = Mathf.Clamp(newVelocity.x, -moveSpeed / jumpMoveSpeedClamp, moveSpeed / jumpMoveSpeedClamp);
            newVelocity.z = Mathf.Clamp(newVelocity.z, -moveSpeed / jumpMoveSpeedClamp, moveSpeed / jumpMoveSpeedClamp);
        }

        rb.velocity = newVelocity;
    }

    void CharacterJump()
    {
        // 점프 상태일 때 점프 불가능
        if (isJump) return;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isJump = true;
        cf.enabled = true;
    }

    void CameraRotaion()
    {
        if (!isCamMove) return;
        currentRotation.y += Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
        currentRotation.x = Mathf.Clamp(currentRotation.x - Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime, -70.0f, 70.0f);

        rotateTarget.rotation = Quaternion.Euler(currentRotation);
    }

    void CharacterRoatation()
    {
        this.transform.rotation = rotateTarget.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 땅에 닿으면 점프 가능
        //if(collision.gameObject.layer == 7)
        //{
        //    isJump = false;
        //}

        isJump = false;
        cf.enabled = false;
    }
}
