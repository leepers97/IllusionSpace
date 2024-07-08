using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 이 스크립트가 달린 오브젝트와 target오브젝트의 scale값을 같게 한다
public class SameScale_HCH : MonoBehaviour
{
    public GameObject target;

    // Update is called once per frame
    void Update()
    {
        target.transform.localScale = gameObject.transform.localScale;
    }
}
