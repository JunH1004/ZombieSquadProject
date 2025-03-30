using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDropHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image cardImage; // 카드 이미지
    private RectTransform rectTransform;
    private Vector3 originalPosition;
    private ICardAction cardAction; // 카드 동작 인터페이스

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.position;
        cardAction = GetComponent<ICardAction>(); // 카드에 붙은 ICardAction 컴포넌트
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        cardImage.color = new Color(cardImage.color.r, cardImage.color.g, cardImage.color.b, 0.6f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        cardImage.color = new Color(cardImage.color.r, cardImage.color.g, cardImage.color.b, 1f);

        // 스크린 좌표를 월드 좌표로 변환
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(eventData.position);

        // ZoneManager에서 타일맵 참조
        ZoneManager zoneManager = FindObjectOfType<ZoneManager>();
        if (zoneManager != null && zoneManager.zoneTilemap != null)
        {
            // 월드 좌표를 타일맵 셀 좌표로 변환
            Vector3Int targetPosition = zoneManager.zoneTilemap.WorldToCell(worldPos);

            // 디버그 출력
            Debug.Log($"드래그 종료 - 스크린: {eventData.position}, 월드: {worldPos}, 타일: {targetPosition}");

            // 카드 동작 실행
            if (cardAction != null)
            {
                cardAction.Execute(zoneManager, new Vector3Int(targetPosition.x, targetPosition.y, 0));
            }
            else
            {
                Debug.LogWarning("ICardAction이 없습니다.");
            }
        }
        else
        {
            Debug.LogWarning("ZoneManager 또는 zoneTilemap을 찾을 수 없습니다.");
        }

        // 원래 위치로 복귀
        rectTransform.position = originalPosition;
    }
}