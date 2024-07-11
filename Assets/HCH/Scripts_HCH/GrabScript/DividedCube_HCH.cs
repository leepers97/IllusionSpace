using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DividedCube_HCH : MonoBehaviour
{
    public GameObject cubePrefab;
    // 폭발 파워
    public float divideForce;
    // 폭발 위치
    public Vector3 offset = Vector3.zero;

    public void DivideCube()
    {
        // 함수가 호출되면 여러 개의 작은 큐브를 생성한다
        GameObject dividedCube = Instantiate(cubePrefab, transform.position, Quaternion.identity);
        // 생성된 큐브의 크기는 기존 큐브의 크기에 비례한다
        dividedCube.transform.localScale = transform.localScale / 3;

        // 작은 큐브들의 rigidbody를 가져와 폭발시킨다
        Rigidbody[] rb = dividedCube.GetComponentsInChildren<Rigidbody>();
        for(int i = 0; i < rb.Length; i++)
        {
            rb[i].AddExplosionForce(divideForce, transform.position + offset, 10f);
        }
        // 기존 큐브는 비활성화한다
        gameObject.SetActive(false);
    }
}
