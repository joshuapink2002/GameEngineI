using Unity.Cinemachine;  // 권장 네임스페이스 (Unity.Cinemachine 대신)
using UnityEngine;

public class MapTransition : MonoBehaviour
{
    [SerializeField] PolygonCollider2D mapBoundary;   // 새 카메라 경계
    [SerializeField] Direction direction = Direction.Up;
    [SerializeField] float additivePos = 2f;

    CinemachineConfiner2D confiner;

    enum Direction { Up, Down, Left, Right }

    void Awake()
    {
        confiner = FindAnyObjectByType<CinemachineConfiner2D>(); // Unity 2022+
        // confiner가 null이면 씬에 가상카메라+Confiner가 없는 상태
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        // 1) 카메라 경계 교체
        if (confiner && mapBoundary)
        {
            // CM3 속성명: BoundingShape2D / CM2: m_BoundingShape2D (둘 다 대응 가능)
            confiner.BoundingShape2D = mapBoundary;

#if CINEMACHINE_3_OR_NEWER
            confiner.InvalidateBoundingShapeCache();
#else
            confiner.InvalidateCache();
#endif
        }

        // 2) 플레이어 위치 이동
        UpdatePlayerPosition(collision.gameObject);
    }

    void UpdatePlayerPosition(GameObject player)
    {
        Vector3 newPos = player.transform.position;

        switch (direction)
        {
            case Direction.Up: newPos.y += additivePos; break;
            case Direction.Down: newPos.y -= additivePos; break;
            case Direction.Left: newPos.x -= additivePos; break;
            case Direction.Right: newPos.x += additivePos; break;
        }

        // Rigidbody2D가 있으면 물리 좌표로 이동하는 게 더 안정적
        var rb = player.GetComponent<Rigidbody2D>();
        if (rb != null) rb.position = newPos;
        else player.transform.position = newPos;
    }
}
