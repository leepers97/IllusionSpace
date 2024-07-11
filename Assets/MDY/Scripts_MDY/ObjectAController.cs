using UnityEngine;

public class ObjectAController : MonoBehaviour
{
    public GameObject objectB; // 물체 B에 대한 참조
    public static int collisionCount = 0; // 정적 카운터를 사용하여 A 물체들과의 충돌 횟수 추적

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player") // 플레이어와의 충돌은 무시
        {
            collisionCount++; // 충돌 횟수 증가
            CheckActivation(); // B 물체 활성화 여부 검사
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag != "Player") // 플레이어와의 충돌은 무시
        {
            collisionCount--; // 충돌 횟수 감소
            CheckActivation(); // B 물체 활성화 여부 검사
        }
    }

    void CheckActivation()
    {
        if (collisionCount == 2) // 모든 A 물체들과 충돌 중일 때만 B 물체 활성화
        {
            objectB.GetComponent<ObjectBFader>().StartFadingOut();
        }
        else if (collisionCount < 2) // 두 물체 중 하나라도 충돌이 해제되면 B 물체 비활성화
        {
            objectB.GetComponent<ObjectBFader>().StartFadingIn();
        }
    }
}