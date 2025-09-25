using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MapController_Manual : MonoBehaviour
{
    public static MapController_Manual Instance; // ✅ 싱글턴 추가

    public GameObject mapParent;
    private List<Image> mapImages;

    public Color highlightColour = Color.yellow;
    public Color dimmedColor = new Color(1f, 1f, 1f, 0.5f);

    public RectTransform playerIconTransform;

    private void Awake()
    {
        // ✅ 싱글턴 패턴
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        // ✅ 자식 Image들 가져오기
        mapImages = mapParent.GetComponentsInChildren<Image>().ToList();
    }

    public void HighlightArea(string areaName)
    {
        // 모든 영역 반투명 처리
        foreach (Image area in mapImages)
        {
            area.color = dimmedColor;
        }

        // 해당 영역 찾기
        Image currentArea = mapImages.Find(x => x.name == areaName);

        if (currentArea != null)
        {
            currentArea.color = highlightColour;

            RectTransform areaRect = currentArea.GetComponent<RectTransform>();
            Vector2 centerPos = areaRect.anchoredPosition + (Vector2)areaRect.rect.center;

            // 아이콘 위치를 영역 중심에 맞추기
            playerIconTransform.anchoredPosition = centerPos;
            // ✅ anchoredPosition을 써야 Canvas 내에서 정확하게 맞음
            //playerIconTransform.anchoredPosition =
            //    currentArea.GetComponent<RectTransform>().anchoredPosition;
        }
        else
        {
            Debug.LogWarning("Area not found: " + areaName);
        }
    }
}
