using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent;
    private CanvasGroup canvasGroup;
    public float minDropDistance = 2f;
    public float maxDropDistance = 2f;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent; // 원래 슬롯 저장
        transform.SetParent(transform.root); // 다른 UI 위로 올리기
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position; // 마우스를 따라감
    }
        
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        // 슬롯 찾기 (자식 오브젝트 클릭해도 부모에서 Slot을 가져오도록)
        Slot dropSlot = eventData.pointerEnter?.GetComponentInParent<Slot>();
        Slot originalSlot = originalParent.GetComponent<Slot>();

        if (dropSlot != null)
        {
            // 원래 슬롯 비우기
            originalSlot.currentItem = null;

            // 만약 드랍 슬롯에 이미 아이템이 있으면 → 스왑
            if (dropSlot.currentItem != null)
            {
                dropSlot.currentItem.transform.SetParent(originalSlot.transform);
                originalSlot.currentItem = dropSlot.currentItem;
                dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }

            // 아이템을 드랍 슬롯으로 이동
            transform.SetParent(dropSlot.transform);
            dropSlot.currentItem = gameObject;
        }
        else
        {
            //If where we're dropping is not within the inventory
            if (!IsWithinInventory(eventData.position))
            {
                //Drop our item
                DropItem(originalSlot);
            }
            else
            {   
                //Snap back to og slot
                transform.SetParent(originalParent);
            }

                // 슬롯이 아니면 원래 위치로
                transform.SetParent(originalParent);
        }

        // 슬롯 안에서 중앙 정렬
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    bool IsWithinInventory(Vector2 mousePosition)
    {
        RectTransform inventoryRect = originalParent.parent.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, mousePosition);
    }

    void DropItem(Slot originalslot)
    {
        originalslot.currentItem = null;

        //Find player
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if(playerTransform == null)
        {
            Debug.LogError("Missing 'Player' tag");
            return;
        }
        //Random drop position
        Vector2 dropOffset = Random.insideUnitCircle.normalized * Random.Range(minDropDistance, maxDropDistance);
        Vector2 dropPosition = (Vector2)playerTransform.position + dropOffset;
        //Instantiate drop item and Bounce
        GameObject dropItem = Instantiate(gameObject, dropPosition, Quaternion.identity);
        dropItem.GetComponent<BounceEffect>().StartBounce();
        //Destroy the UI one
        Destroy(gameObject);
    }
}
