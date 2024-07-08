using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Transform door;
    public Vector3 openPositionOffset = new Vector3(3f, 0, 0);  // 문이 열릴 때의 위치 오프셋
    public float slideSpeed = 2f;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpening = false;
    private bool isClosing = false;

    void Start()
    {
        closedPosition = door.localPosition;
        openPosition = closedPosition + openPositionOffset;
    }

    void Update()
    {
        if (isOpening)
        {
            door.localPosition = Vector3.Lerp(door.localPosition, openPosition, Time.deltaTime * slideSpeed);
        }
        else if (isClosing)
        {
            door.localPosition = Vector3.Lerp(door.localPosition, closedPosition, Time.deltaTime * slideSpeed);
        }
    }

    public void StartOpening()
    {
        isOpening = true;
        isClosing = false;
    }

    public void StartClosing()
    {
        isOpening = false;
        isClosing = true;
    }
}


