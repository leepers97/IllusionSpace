using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDuplicator : MonoBehaviour
{
    public float holdTime = 2.0f; // Ŭ�� ���� �ð�
    public Transform player; // �÷��̾� Transform
    public float duplicationInterval = 0.1f; // ������Ʈ ���� ���� (�� ����)
    public LayerMask duplicableLayer; // ���� ������ ������Ʈ ���̾� ����

    private float holdTimer = 0.0f;
    private float duplicationTimer = 0.0f;
    private bool isDuplicating = false;
    private GameObject currentTarget; // ���� Ŭ���� ������Ʈ

    void Update()
    {
        if (Input.GetMouseButton(0)) // ���콺 ���� ��ư Ŭ�� ��
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

        // ����ĳ��Ʈ�� ���� Ŭ���� ������Ʈ ����
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
        // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        return player.position + player.forward * 2.0f; // �⺻ ��ġ�� �÷��̾� ��
    }
}
