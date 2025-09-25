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
        confiner = FindAnyObjectByType<CinemachineConfiner2D>();
        if (confiner == null)
            Debug.LogWarning("⚠️ CinemachineConfiner2D not found in scene!");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        // 1) 카메라 경계 교체
        if (confiner && mapBoundary)
        {
            confiner.BoundingShape2D = mapBoundary;

#if CINEMACHINE_3_OR_NEWER
            confiner.InvalidateBoundingShapeCache();
#else
            confiner.InvalidateCache();
#endif
        }

        // 2) 플레이어 위치 이동
        UpdatePlayerPosition(collision.gameObject);

        // 3) 맵 UI 업데이트 (싱글턴)
        MapController_Manual.Instance?.HighlightArea(mapBoundary.name);
        MapController_Dynamic.Instance?.UpdateCurrentArea(mapBoundary.name);
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

        var rb = player.GetComponent<Rigidbody2D>();
        if (rb != null) rb.position = newPos;
        else player.transform.position = newPos;
    }
}