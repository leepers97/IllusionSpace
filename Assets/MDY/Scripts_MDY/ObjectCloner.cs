using UnityEngine;

public class ObjectCloner : MonoBehaviour
{
    public float cloneInterval = 0.5f; // 복제 간격 (초)
    private float lastCloneTime;
    private Vector3 lastPosition;
    private bool isGrabbed = false; // 물체가 잡혔는지 여부

    void Start()
    {
        lastCloneTime = Time.time;
        lastPosition = transform.position;
    }

    void Update()
    {
        if (isGrabbed && Vector3.Distance(transform.position, lastPosition) > 0.01f) // 물체가 움직였는지 확인
        {
            if (Time.time - lastCloneTime >= cloneInterval)
            {
                CloneObject();
                lastCloneTime = Time.time;
            }
            lastPosition = transform.position;
        }
    }

    void CloneObject()
    {
        GameObject clone = Instantiate(gameObject, transform.position, transform.rotation);
        clone.transform.localScale = transform.localScale;
        ObjectCloner cloneCloner = clone.GetComponent<ObjectCloner>();
        if (cloneCloner != null)
        {
            Destroy(cloneCloner); // 복제된 물체에는 복제 스크립트를 비활성화
        }

        Rigidbody cloneRigidbody = clone.GetComponent<Rigidbody>();
        if (cloneRigidbody != null)
        {
            cloneRigidbody.isKinematic = false; // 복제된 물체의 isKinematic 설정을 변경하지 않음
        }
    }

    public void SetGrabbed(bool grabbed)
    {
        isGrabbed = grabbed;
    }
}
