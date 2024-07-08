using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTextureSetUp_HCH : MonoBehaviour
{
    public Camera portalCam_A;
    public Camera portalCam_B;

    public Material portalCamMat_A;
    public Material portalCamMat_B;

    // Start is called before the first frame update
    void Start()
    {
        if(portalCam_A.targetTexture != null)
        {
            portalCam_A.targetTexture.Release();
        }
        portalCam_A.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        portalCamMat_A.mainTexture = portalCam_A.targetTexture;

        if (portalCam_B.targetTexture != null)
        {
            portalCam_B.targetTexture.Release();
        }
        portalCam_B.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        portalCamMat_B.mainTexture = portalCam_B.targetTexture;
    }
}
