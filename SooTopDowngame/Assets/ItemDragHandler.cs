using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent;
    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent; // ���� ���� ����
        transform.SetParent(transform.root); // �ٸ� UI ���� �ø���
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position; // ���콺�� ����
    }
        
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        // ���� ã�� (�ڽ� ������Ʈ Ŭ���ص� �θ𿡼� Slot�� ����������)
        Slot dropSlot = eventData.pointerEnter?.GetComponentInParent<Slot>();
        Slot originalSlot = originalParent.GetComponent<Slot>();

        if (dropSlot != null)
        {
            // ���� ���� ����
            originalSlot.currentItem = null;

            // ���� ��� ���Կ� �̹� �������� ������ �� ����
            if (dropSlot.currentItem != null)
            {
                dropSlot.currentItem.transform.SetParent(originalSlot.transform);
                originalSlot.currentItem = dropSlot.currentItem;
                dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }

            // �������� ��� �������� �̵�
            transform.SetParent(dropSlot.transform);
            dropSlot.currentItem = gameObject;
        }
        else
        {
            // ������ �ƴϸ� ���� ��ġ��
            transform.SetParent(originalParent);
        }

        // ���� �ȿ��� �߾� ����
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
}
