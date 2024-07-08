using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이 지역에 player가 들어왔을 때
// 특정 각도인지 체크하고
// 일정 시간 동안 특정 각도를 유지하면
// 오브젝트를 생성하고
// 필드 오브젝트는 끈다

// 오차범위 +- 1.5정도?

public class CombineObject_HCH : MonoBehaviour
{
    public GameObject combinedObjectPrefab;
    public GameObject dividedObject;

    public Transform combinedAngle;
    public Transform combinedSpawnPos;

    public float checkRotationRange = 1.5f;
    public float checkTime = 2f;
    float currentTime;

    bool isSpawn = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (isSpawn) return;
        // player와 충돌 시
        if (other.gameObject.CompareTag("Player"))
        {
            // Angle을 저장한 게임 오브젝트와 rotation x, y값이 checkRotationRange이내라면
            // player의 rotation값과 target의 rotation값을 저장
            Vector3 playerRotation = other.transform.rotation.eulerAngles;
            Vector3 targetRotation = combinedAngle.rotation.eulerAngles;

            // player의 rotation.x값과 target의 rotation.y값의 차이를 구하고
            float rotX = Mathf.Abs(playerRotation.x - targetRotation.x);
            float rotY = Mathf.Abs(playerRotation.y - targetRotation.y);

            // 그 차이가 checkRotationRange이내라면
            if (rotX <= checkRotationRange && rotY <= checkRotationRange)
            {
                Debug.Log($"플레이어와 대상 오브젝트의 회전 값이 {checkRotationRange} 오차 이내입니다.");
                // 시간을 측정하고 checkTime이 되면
                currentTime += Time.deltaTime;
                if(currentTime > checkTime)
                {
                    // 합쳐진 게임 오브젝트를 생성하고 기존의 분리된 게임 오브젝트를 비활성화
                    GameObject combinedObject = Instantiate(combinedObjectPrefab);
                    combinedObject.transform.position = combinedSpawnPos.position;
                    dividedObject.SetActive(false);
                    isSpawn = true;
                }
            }
            else
            {
                Debug.Log($"플레이어와 대상 오브젝트의 회전 값이 {checkRotationRange} 오차 초과입니다.");
                currentTime = 0f;
            }
        }
    }
}
