using UnityEngine;

public class PerspectiveScalingAndDragging : MonoBehaviour
{
    public Camera mainCamera;
    public float scaleSpeed = 0.1f;
    private GameObject selectedObject;
    private float initialDistance;
    private Vector3 initialScale;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Scalable"))
                {
                    selectedObject = hit.collider.gameObject;
                    initialDistance = Vector3.Distance(mainCamera.transform.position, selectedObject.transform.position);
                    initialScale = selectedObject.transform.localScale;
                }
            }
        }

        if (Input.GetMouseButton(0) && selectedObject != null)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(mainCamera.transform.forward, initialDistance);
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);
                selectedObject.transform.position = hitPoint;
                float currentDistance = Vector3.Distance(mainCamera.transform.position, hitPoint);
                float scaleMultiplier = currentDistance / initialDistance;
                selectedObject.transform.localScale = initialScale * scaleMultiplier;
            }
        }

        if (Input.GetMouseButtonUp(0) && selectedObject != null)
        {
            selectedObject = null;
        }
    }
}
