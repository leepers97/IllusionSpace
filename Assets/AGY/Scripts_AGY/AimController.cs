using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
    public GameObject objectToDuplicate; // 복제할 오브젝트
    public LayerMask duplicateLayer; // 복사 가능한 레이어

    private Transform aimTransform; // 에임의 Transform
    private Renderer lastHoveredRenderer; // 마지막으로 호버된 Renderer

    void Start()
    {
        aimTransform = GetComponent<Transform>();
    }

    void Update()
    {
        // 에임 위치 설정
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, duplicateLayer))
        {
            // 마우스가 오브젝트 위에 있을 때 색상 변경
            Renderer renderer = hit.collider.GetComponent<Renderer>();
            if (renderer != null)
            {
                // 이전에 호버한 Renderer의 색상 초기화
                if (lastHoveredRenderer != null && lastHoveredRenderer != renderer)
                {
                    lastHoveredRenderer.material.color = Color.white;
                }

                // 현재 호버한 Renderer의 색상 변경
                renderer.material.color = Color.yellow;
                lastHoveredRenderer = renderer;
            }

            // 마우스 클릭 시 오브젝트 복사
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 spawnPosition = hit.point;
                Quaternion spawnRotation = Quaternion.identity;

                // 물체 복제
                GameObject clonedObject = Instantiate(objectToDuplicate, spawnPosition, spawnRotation);
            }
        }
        else
        {
            // 마우스가 오브젝트 위에 없을 때 이전에 호버한 Renderer의 색상 초기화
            if (lastHoveredRenderer != null)
            {
                lastHoveredRenderer.material.color = Color.white;
                lastHoveredRenderer = null;
            }
        }

        // 에임 위치 업데이트
        aimTransform.position = hit.point;
        aimTransform.rotation = Quaternion.LookRotation(ray.direction);
    }
}
