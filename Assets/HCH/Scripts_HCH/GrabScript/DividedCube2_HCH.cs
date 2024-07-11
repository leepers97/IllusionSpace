using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DividedCube2_HCH : MonoBehaviour
{
    // �ڽ� ������Ʈ�� rigidbody�� �޾Ƽ�
    // grab�� Ÿ������ �ڽ� ������Ʈ�� ���õȴٸ�
    // isKinematic�� ��� ����
    [SerializeField]
    Rigidbody[] rb;
    public bool isDivide = false;

    private void Start()
    {
        rb = gameObject.GetComponentsInChildren<Rigidbody>();
    }

    private void Update()
    {
        foreach (Rigidbody rigid in rb)
        {
            if (rigid.gameObject.transform == GameManager.instance.grab.target)
            {
                isDivide = true;
                break;
            }
        }
        if (isDivide)
        {
            foreach (Rigidbody rigid in rb)
            {
                if (rigid.gameObject.transform == GameManager.instance.grab.target) return;
                rigid.isKinematic = false;
            }
        }
    }

    public void Divide()
    {
        rb = gameObject.GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody rigid in rb)
        {
            if(rigid.gameObject == GameManager.instance.grab.target)
            {
                isDivide = true;
                break;
            }
        }
        if (isDivide)
        {
            foreach (Rigidbody rigid in rb)
            {
                rigid.isKinematic = false;
            }
        }
    }
}
