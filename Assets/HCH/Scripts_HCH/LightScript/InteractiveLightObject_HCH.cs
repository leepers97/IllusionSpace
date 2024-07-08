using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 빛의 range, intensity값은 물체의 scale값에 비례한다. 
// 빛 오브젝트를 특정 투명 오브젝트(ex: 벽)에 가깝게 가져다대면 투명을 해제한다.
// 빛의 range, intensity 값이 어느정도 이상이어야 투명을 해제한다.
public class InteractiveLightObject_HCH : MonoBehaviour
{
    Vector3 originalScale;

    Light light;
    public float lightRangePower = 3;
    public float lightIntensityPower = 3;
    public float lightLimit;

    public float lineSize = 3;
    public float scanRange;
    public Collider[] hitInfo;
    public LayerMask targetLayer;
    HashSet<Collider> previousDetectedObjects = new HashSet<Collider>();


    // Start is called before the first frame update
    void Start()
    {
        light = GetComponentInChildren<Light>();
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        // 스케일 비율 계산
        Vector3 currentScale = transform.localScale;
        float scaleMultiplier = currentScale.x / originalScale.x;

        // 스케일 비율에 따른 빛의 범위, 세기 조정
        light.range = scaleMultiplier * lightRangePower;
        light.intensity = scaleMultiplier * lightIntensityPower;

        // 근처 오브젝트 탐지(수정함 -> 일단 빛 세기, 범위 조절만으로도 충분)
        //DetectTransparent();
    }

    // 사용 안함
    void DetectTransparent()
    {
        scanRange = light.range;
        hitInfo = Physics.OverlapSphere(transform.position, scanRange, targetLayer);
        HashSet<Collider> currentDetectedObjects = new HashSet<Collider>(hitInfo);
        // 새롭게 감지된 오브젝트 처리
        foreach (Collider hit in hitInfo)
        {
            if (!previousDetectedObjects.Contains(hit))
            {
                // 벽이 사라진 도중에는 scanRange < lightLimit 안먹음
                if (scanRange < lightLimit) return;
                hit.GetComponent<MeshRenderer>().enabled = false;
            }
        }

        // 빠져나간 오브젝트 처리
        foreach (Collider hit in previousDetectedObjects)
        {
            if (!currentDetectedObjects.Contains(hit))
            {
                hit.GetComponent<MeshRenderer>().enabled = true;
            }
        }

        // 현재 감지된 오브젝트로 처리 
        previousDetectedObjects = currentDetectedObjects;

    }

    void OnDrawGizmos()
    {
        // 탐지 범위 기즈모 표시
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, scanRange);
    }
}
