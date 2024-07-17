using UnityEngine;

public class ObjectAController : MonoBehaviour
{
    public ObjectBController objectBController; // 물체 B의 ObjectBController 스크립트 참조
    private bool isColliding = false;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player") // 플레이어와의 충돌은 무시
        {
            if (!isColliding)
            {
                isColliding = true;
                objectBController.IncrementCollisionCount(); // 충돌 횟수 증가
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag != "Player") // 플레이어와의 충돌은 무시
        {
            if (isColliding)
            {
                isColliding = false;
                objectBController.DecrementCollisionCount(); // 충돌 횟수 감소
            }
        }
    }
}