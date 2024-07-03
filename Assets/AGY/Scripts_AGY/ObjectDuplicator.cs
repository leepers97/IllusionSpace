using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDuplicator : MonoBehaviour
{
    public Camera mainCamera;
    public LayerMask interactableLayer;
    public GameObject duplicatePrefab;

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right mouse button for duplication
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
            {
                GameObject duplicate = Instantiate(duplicatePrefab, hit.transform.position, hit.transform.rotation);
                duplicate.transform.localScale = hit.transform.localScale;
            }
        }
    }
}
