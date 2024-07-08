using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public Transform button;
    public DoorController doorController;
    public string triggerTag = "Draggable";  // 버튼을 눌릴 수 있는 물체의 태그
    public float buttonPressSpeed = 2f;

    private Vector3 initialButtonPosition;
    private Vector3 pressedPositionOffset = new Vector3(0, -0.1f, 0);  // 버튼이 눌릴 때의 위치 오프셋
    private bool isPressed = false;

    void Start()
    {
        initialButtonPosition = button.localPosition;
    }

    void Update()
    {
        Vector3 targetPosition = isPressed ? initialButtonPosition + pressedPositionOffset : initialButtonPosition;
        button.localPosition = Vector3.Lerp(button.localPosition, targetPosition, Time.deltaTime * buttonPressSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggerTag))
        {
            isPressed = true;
            doorController.StartOpening();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(triggerTag))
        {
            isPressed = false;
            doorController.StartClosing();
        }
    }
}

