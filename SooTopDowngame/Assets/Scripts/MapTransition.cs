using Unity.Cinemachine;  // ���� ���ӽ����̽� (Unity.Cinemachine ���)
using UnityEngine;

public class MapTransition : MonoBehaviour
{
    [SerializeField] PolygonCollider2D mapBoundary;   // �� ī�޶� ���
    [SerializeField] Direction direction = Direction.Up;
    [SerializeField] float additivePos = 2f;

    CinemachineConfiner2D confiner;

    enum Direction { Up, Down, Left, Right }

    void Awake()
    {
        confiner = FindAnyObjectByType<CinemachineConfiner2D>(); // Unity 2022+
        // confiner�� null�̸� ���� ����ī�޶�+Confiner�� ���� ����
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        // 1) ī�޶� ��� ��ü
        if (confiner && mapBoundary)
        {
            // CM3 �Ӽ���: BoundingShape2D / CM2: m_BoundingShape2D (�� �� ���� ����)
            confiner.BoundingShape2D = mapBoundary;

#if CINEMACHINE_3_OR_NEWER
            confiner.InvalidateBoundingShapeCache();
#else
            confiner.InvalidateCache();
#endif
        }

        // 2) �÷��̾� ��ġ �̵�
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

        // Rigidbody2D�� ������ ���� ��ǥ�� �̵��ϴ� �� �� ������
        var rb = player.GetComponent<Rigidbody2D>();
        if (rb != null) rb.position = newPos;
        else player.transform.position = newPos;
    }
}
