using UnityEngine;

public class Tile : MonoBehaviour {
    public bool isOccupied = false;  // 구역이 사용 중인지 여부
    public Unit unit;                // 배치된 유닛 (null이면 없음)

    // 유닛을 구역에 배치하는 메서드
    public void PlaceUnit(Unit newUnit) {
        if (!isOccupied) {
            unit = newUnit;
            isOccupied = true;
            unit.transform.position = this.transform.position; // 유닛 위치를 타일 위치로 설정
            Debug.Log($"유닛 배치: {unit.unitName} at {this.transform.position}");
        } else {
            Debug.Log("이 구역은 이미 사용 중입니다.");
        }
    }
}