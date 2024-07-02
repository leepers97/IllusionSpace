using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
   public Transform playerBody; // 플레이어의 몸통
    public Transform playerHead; // 플레이어의 머리

    public float mouseSensitivity = 100f;
    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None; // 커서 항상 활성화
        Cursor.visible = true; // 커서 항상 보이게 설정
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 플레이어의 몸통을 좌우로 회전
        playerBody.Rotate(Vector3.up * mouseX);

        // 플레이어의 머리를 상하로 회전
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // 상하 회전 범위 제한

        playerHead.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
