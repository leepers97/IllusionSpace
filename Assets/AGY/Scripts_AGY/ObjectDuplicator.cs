using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDuplicator : MonoBehaviour
{
    public float holdTime = 2.0f; // 클릭 유지 시간
    public Transform player; // 플레이어 Transform
    public float duplicationInterval = 0.1f; // 오브젝트 복사 간격 (초 단위)
    public LayerMask duplicableLayer; // 복사 가능한 오브젝트 레이어 설정

    private float holdTimer = 0.0f;
    private float duplicationTimer = 0.0f;
    private bool isDuplicating = false;
    private GameObject currentTarget; // 현재 클릭한 오브젝트

    void Update()
    {
        if (Input.GetMouseButton(0)) // 마우스 왼쪽 버튼 클릭 시
        {
            holdTimer += Time.deltaTime;
            if (holdTimer >= holdTime)
            {
                if (!isDuplicating)
                {
                    StartDuplicating();
                }
                duplicationTimer += Time.deltaTime;
                if (duplicationTimer >= duplicationInterval)
                {
                    DuplicateObject();
                    duplicationTimer = 0.0f;
                }
            }
        }
        else
        {
            holdTimer = 0.0f;
            duplicationTimer = 0.0f;
            isDuplicating = false;
            currentTarget = null;
        }
    }

    void StartDuplicating()
    {
        isDuplicating = true;

        // 레이캐스트를 통해 클릭된 오브젝트 감지
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, duplicableLayer))
        {
            if (hit.collider != null && hit.collider.CompareTag("Duplicate"))
            {
                currentTarget = hit.collider.gameObject;
            }
        }
    }

    void DuplicateObject()
    {
        if (currentTarget != null && player != null)
        {
            Vector3 spawnPosition = GetMouseWorldPosition();
            Instantiate(currentTarget, spawnPosition, Quaternion.identity);
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        // 마우스 위치를 월드 좌표로 변환
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        return player.position + player.forward * 2.0f; // 기본 위치는 플레이어 앞
    }
}
