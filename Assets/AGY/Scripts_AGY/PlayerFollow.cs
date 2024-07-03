using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform
    public float distance = 5.0f; // 플레이어와 복사된 오브젝트 간의 거리

    void Update()
    {
        // 플레이어의 시선을 따라서 오브젝트를 일정 거리만큼 떨어뜨리기
        Vector3 targetPosition = player.position + player.forward * distance;
        transform.position = targetPosition;
    }
}
