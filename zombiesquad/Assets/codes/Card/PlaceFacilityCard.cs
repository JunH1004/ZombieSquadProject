using UnityEngine;

public class PlaceFacilityCard : MonoBehaviour, ICardAction
{
    public GameObject facilityPrefab;  // 배치할 시설의 프리팹

    // Execute 메서드: 특정 위치에 시설을 배치하는 동작
    void ICardAction.Execute(ZoneManager zoneManager, Vector3Int targetPosition)
    {
        // 해당 위치의 존을 찾기
        Zone selectedZone = zoneManager.GetZone(targetPosition);
        
        if (selectedZone != null && !selectedZone.IsOccupied())  // 존이 비어있으면
        {
            // 시설 배치
            zoneManager.PlaceZone(targetPosition, facilityPrefab);
        }
        else
        {
            //레벨업 기능
            Debug.LogWarning("존이 이미 점유되어 있거나 유효하지 않은 위치입니다.");
        }
    }

    // Execute 메서드: 인자가 없는 경우, 기본 동작을 수행할 수 있는 메서드 (필요한 경우 사용)
    public void Execute()
    {
        // 기본 동작을 정의하지 않으면 경고 메시지 출력
        Debug.LogWarning("PlaceFacilityCard는 타겟 위치가 필요합니다.");
    }
}
