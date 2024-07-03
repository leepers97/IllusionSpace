using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManipulator : MonoBehaviour
{
    public Camera mainCamera;
    public LayerMask interactableLayer;
    public float resizeSpeed = 0.1f;

    private Transform selectedObject;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
            {
                if (selectedObject == hit.transform)
                {
                    selectedObject = null;
                }
                else
                {
                    selectedObject = hit.transform;
                }
            }
        }

        if (selectedObject != null)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                selectedObject.localScale += Vector3.one * resizeSpeed;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                selectedObject.localScale -= Vector3.one * resizeSpeed;
            }
        }
    }
}
