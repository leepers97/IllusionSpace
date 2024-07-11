using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 1. 원근법 매커니즘 수정 필요(잡고 이동 시 버벅거림)
// 2. 클릭 후 이동 중에 다른 오브젝트와 충돌 막기

public class Grab2_HCH : MonoBehaviour
{

    [Header("Components")]
    public Transform target;

    [Header("Parameters")]
    public LayerMask targetMask;
    public LayerMask ignoreTargetMask;
    // 벽과 물체 사이의 최소거리값
    public float offsetFactor;

    // 카메라와 오브젝트간의 원래 거리
    float originalDistance;
    // 크기 변경 전 오브젝트의 원래 크기
    Vector3 originalScale;
    // 매 프레임에 변할 오브젝트의 크기
    Vector3 targetScale;

    // 우클릭하고 물체 회전 시 속도
    public float rotateSpeed = 5;
    // 현재 회전값
    Vector3 currentRotation;
    // 물체 회전 시 카메라 회전 막기 위한 player스크립트
    Player_HCH player;

    void Start()
    {
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        player = GetComponentInParent<Player_HCH>();
    }

    void Update()
    {
        HandleInput();
        RotateTarget();
        //ResizeTarget();
        Debug.DrawRay(transform.position, transform.forward * 20, Color.red);

    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 현재 선택된 타겟이 없다면
            if (target == null)
            {
                RaycastHit hit;
                // targetMask로 선택된 layer의 오브젝트만 ray에 맞음
                if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, targetMask))
                {
                    // ray 잘 맞았는지 확인용
                    Debug.DrawRay(transform.position, transform.forward * 20, Color.red);
                    Debug.Log("Raycast hit: " + hit.transform.name);

                    // ray에 맞은 오브젝트가 타겟이 됨
                    target = hit.transform;

                    // 타겟 오브젝트가 떠있게 하고 다른 오브젝트와의 충돌을 막는다
                    target.GetComponent<Rigidbody>().isKinematic = true;
                    target.GetComponent<Collider>().isTrigger = true;

                    // 캐릭터(카메라)와 타겟 오브젝트 간의 거리를 계산
                    originalDistance = Vector3.Distance(transform.position, target.position);

                    // 타겟 오브젝트의 스케일 값을 저장
                    originalScale = target.localScale;

                    // 일단 바뀔 타겟 오브젝트 스케일 값에 현재 타겟 오브젝트 스케일 값 저장
                    targetScale = target.localScale;

                    target.gameObject.transform.localScale = originalScale / originalDistance;
                    target.gameObject.transform.SetParent(transform);
                    target.transform.localPosition = new Vector3(0, 0, 1.0f);

                    target.gameObject.layer = LayerMask.NameToLayer("Targeting");
                }
            }
            // 다시 좌클릭을 한다면(현재 선택된 타겟이 있다면)
            else
            {
                // 만약 분리되는 오브젝트라면 분리시킨다
                if (target.CompareTag("DivideCube"))
                {
                    target.GetComponent<DividedCube_HCH>().DivideCube();
                }
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, ignoreTargetMask))
                {
                    float currentDistance = Vector3.Distance(transform.position, hit.point);
                    currentDistance -= offsetFactor;

                    Debug.Log(currentDistance);
                    target.transform.localScale *= currentDistance;
                    Vector3 targetPos = target.transform.localPosition;
                    targetPos.z *= currentDistance;
                    target.transform.localPosition = targetPos;
                    // target.transform.SetParent(null);
                    target.transform.parent = null;


                    // 오브젝트의 물리상태를 다시 되돌리고 다시 다른 오브젝트와 충돌을 가능하게 한다
                    target.GetComponent<Rigidbody>().isKinematic = false;
                    target.GetComponent<Collider>().isTrigger = false;

                    // 타겟을 삭제
                    target.gameObject.layer = LayerMask.NameToLayer("Targetable");
                    target = null;
                }
            }
        }
    }

    void RotateTarget()
    {
        // 현재 선택된 타겟이 없다면 리턴
        // if (target == null) return;
        // 우클릭하면 카메라 이동 막고
        if (Input.GetMouseButtonDown(1)) player.isCamMove = false;
        if (Input.GetMouseButton(1))
        {
            // 타겟의 현재 각도 받기
            currentRotation = target.rotation.eulerAngles;
            // 마우스 x 포지션 값을 받아
            currentRotation.y += -Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
            // 물체를 회전
            target.rotation = Quaternion.Euler(currentRotation);
        }
        // 우클릭 해제하면 다시 카메라 이동
        if (Input.GetMouseButtonUp(1)) player.isCamMove = true;
    }
       

    public float smoothSpeed = 10f;
    void ResizeTarget()
    {
        // 현재 선택된 타겟이 없다면
        if (target == null)
        {
            // 아무일도 일어나지 않음
            return;
        }

        RaycastHit hit;
        // 타겟이 될 수 없는 오브젝트만 ray에 맞음
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, ignoreTargetMask))
        {
            Debug.DrawRay(transform.position, transform.forward * 20, Color.red);
            // 타겟의 위치를 ray가 맞은 끝 지점(타겟이 될 수 없는)에서 offsetFactor값만큼의 거리를 두고 설정
            Vector3 offset = new Vector3(
            transform.forward.x * offsetFactor * targetScale.x,
            transform.forward.y * offsetFactor * targetScale.y,
            transform.forward.z * offsetFactor * targetScale.z
            );
            //target.position = hit.point - offset;
            //target.position = hit.point - transform.forward * offsetFactor * targetScale.x;

            Vector3 targetPos = hit.point - offset;
            //Vector3 targetPos = hit.point - transform.forward * offsetFactor * targetScale.x;
            target.position = Vector3.Lerp(target.position, targetPos, Time.deltaTime * smoothSpeed);

            // 현재 캐릭터(카메라)와 타겟 오브젝트 간의 거리를 계산하고
            float currentDistance = Vector3.Distance(transform.position, target.position);

            // 원래 타겟과 캐릭터 간의 거리, 현재 타겟과 캐릭터 간의 거리의 비율을 계산하고 
            float distanceRatio = currentDistance / originalDistance;

            // 타겟 오브젝트의 x, y, z 스케일 값을 distanceRatio값으로 할당
            targetScale.x = originalScale.x * distanceRatio;
            targetScale.y = originalScale.y * distanceRatio;
            targetScale.z = originalScale.z * distanceRatio;

            // 타겟 오브젝트의 스케일 값에 원래 스케일 값을 곱함
            target.localScale = targetScale;
        }
    }
}
