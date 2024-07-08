using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager_HCH : MonoBehaviour
{
    public GameObject player;
    // 테스트용 텔레포트
    public Transform[] TelePos;

    // 광각 테스트 변수들
    public GameObject lightSign;
    public bool testLighting = false;
    public Light directionalLight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 테스트용 텔레포트
        TestTeleport();

        // LightPlace일 경우에만 암전
        if (testLighting)
        {
            directionalLight.enabled = false;
            RenderSettings.ambientIntensity = 0;
            RenderSettings.reflectionIntensity = 0;
            //lightSign.SetActive(true);
        }
        else
        {
            directionalLight.enabled = true;
            RenderSettings.ambientIntensity = 1;
            RenderSettings.reflectionIntensity = 1;
            //lightSign.SetActive(false);
        }
    }

    void TestTeleport()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            player.transform.position = TelePos[0].position;
            testLighting = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            player.transform.position = TelePos[1].position;
            testLighting = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            player.transform.position = TelePos[2].position;
            testLighting = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            player.transform.position = TelePos[3].position;
            testLighting = false;
        }
    }
}
