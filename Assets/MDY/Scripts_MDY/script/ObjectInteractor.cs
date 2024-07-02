using UnityEngine;

public class ObjectInteractor : MonoBehaviour
{
    private Camera mainCamera;
    private GameObject selectedObject;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Draggable"))
                {
                    selectedObject = hit.collider.gameObject;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            selectedObject = null;
        }

        if (selectedObject != null)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Vector3.Distance(mainCamera.transform.position, selectedObject.transform.position);
            selectedObject.transform.position = mainCamera.ScreenToWorldPoint(mousePos);
        }
    }
}

