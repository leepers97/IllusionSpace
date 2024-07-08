using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab_HCH : MonoBehaviour
{
    [Header("Components")]
    public Transform target;

    [Header("Parameters")]
    public LayerMask targetMask;
    public LayerMask ignoreTargetMask;
    // ���� ��ü ������ �ּҰŸ���
    public float offsetFactor;
    
    // ī�޶�� ������Ʈ���� ���� �Ÿ�
    float originalDistance;
    // ũ�� ���� �� ������Ʈ�� ���� ũ��
    Vector3 originalScale;
    // �� �����ӿ� ���� ������Ʈ�� ũ��
    Vector3 targetScale;

    // ��Ŭ���ϰ� ��ü ȸ�� �� �ӵ�
    public float rotateSpeed = 5;
    // ���� ȸ����
    Vector3 currentRotation;
    // ��ü ȸ�� �� ī�޶� ȸ�� ���� ���� player��ũ��Ʈ
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
        ResizeTarget();
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // ���� ���õ� Ÿ���� ���ٸ�
            if (target == null)
            {
                RaycastHit hit;
                // targetMask�� ���õ� layer�� ������Ʈ�� ray�� ����
                if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, targetMask))
                {
                    // ray �� �¾Ҵ��� Ȯ�ο�
                    Debug.DrawRay(transform.position, transform.forward * 20, Color.red);
                    Debug.Log("Raycast hit: " + hit.transform.name);

                    // ray�� ���� ������Ʈ�� Ÿ���� ��
                    target = hit.transform;

                    // Ÿ�� ������Ʈ�� ���ְ� �ϰ�
                    target.GetComponent<Rigidbody>().isKinematic = true;

                    // ĳ����(ī�޶�)�� Ÿ�� ������Ʈ ���� �Ÿ��� ���
                    originalDistance = Vector3.Distance(transform.position, target.position);

                    // Ÿ�� ������Ʈ�� ������ ���� ����
                    originalScale = target.localScale;

                    // �ϴ� �ٲ� Ÿ�� ������Ʈ ������ ���� ���� Ÿ�� ������Ʈ ������ �� ����
                    targetScale = target.localScale;
                }
            }
            // �ٽ� ��Ŭ���� �Ѵٸ�(���� ���õ� Ÿ���� �ִٸ�)
            else
            {
                // ������Ʈ�� �������¸� �ٽ� �ǵ�����
                target.GetComponent<Rigidbody>().isKinematic = false;

                // Ÿ���� ����
                target = null;
            }
        }
    }

    void RotateTarget()
    {
        // ���� ���õ� Ÿ���� ���ٸ� ����
        if (target == null) return;
        // ��Ŭ���ϸ� ī�޶� �̵� ����
        if (Input.GetMouseButtonDown(1)) player.isCamMove = false;
        if (Input.GetMouseButton(1))
        {
            // Ÿ���� ���� ���� �ޱ�
            currentRotation = target.rotation.eulerAngles;
            // ���콺 x ������ ���� �޾�
            currentRotation.y += -Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;
            // ��ü�� ȸ��
            target.rotation = Quaternion.Euler(currentRotation);
        }
        // ��Ŭ�� �����ϸ� �ٽ� ī�޶� �̵�
        if (Input.GetMouseButtonUp(1)) player.isCamMove = true;
    }

    void ResizeTarget()
    {
        // ���� ���õ� Ÿ���� ���ٸ�
        if (target == null)
        {
            // �ƹ��ϵ� �Ͼ�� ����
            return;
        }

        // Cast a ray forward from the camera position, ignore the layer that is used to acquire targets
        // so we don't hit the attached target with our ray
        RaycastHit hit;
        // Ÿ���� �� �� ���� ������Ʈ�� ray�� ����
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, ignoreTargetMask))
        {
            Debug.DrawRay(transform.position, transform.forward * 20, Color.red);
            // Ÿ���� ��ġ�� ray�� ���� �� ����(Ÿ���� �� �� ����)���� offsetFactor����ŭ�� �Ÿ��� �ΰ� ����
            target.position = hit.point - transform.forward * offsetFactor * targetScale.x;

            // ���� ĳ����(ī�޶�)�� Ÿ�� ������Ʈ ���� �Ÿ��� ����ϰ�
            float currentDistance = Vector3.Distance(transform.position, target.position);

            // ���� Ÿ�ٰ� ĳ���� ���� �Ÿ�, ���� Ÿ�ٰ� ĳ���� ���� �Ÿ��� ������ ����ϰ� 
            float distanceRatio = currentDistance / originalDistance;

            // Ÿ�� ������Ʈ�� x, y, z ������ ���� distanceRatio������ �Ҵ�
            targetScale.x = originalScale.x * distanceRatio;
            targetScale.y = originalScale.y * distanceRatio;
            targetScale.z = originalScale.z * distanceRatio;

            // Ÿ�� ������Ʈ�� ������ ���� ���� ������ ���� ����
            target.localScale = targetScale;
        }
    }
}
