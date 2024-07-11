using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotalCam_HCH : MonoBehaviour
{
    public Transform playerCam;
    public Transform portal;
    public Transform otherPortal;

    // Update is called once per frame
    void Update()
    {
        // 플레이어와 다른 포탈과의 거리를 구하고
        Vector3 playerOffsetFromPortal = playerCam.position - otherPortal.position;
        // 스크립트가 붙은 포탈의 거리에 플레이어와 포탈과의 거리를 더한다
        transform.position = portal.position + playerOffsetFromPortal;

        float angleDifferenceBetweenPortalRotations = Quaternion.Angle(portal.rotation, otherPortal.rotation);

        Quaternion portalRotationDifference = Quaternion.AngleAxis(angleDifferenceBetweenPortalRotations, Vector3.up);
        Vector3 newCamDir = portalRotationDifference * playerCam.forward;
        transform.rotation = Quaternion.LookRotation(newCamDir, Vector3.up);
    }
}
