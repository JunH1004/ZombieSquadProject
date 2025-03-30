using System.Collections.Generic;
using UnityEngine;

public class Zone {
    private Vector3Int cellPosition;            // 타일맵상의 좌표
    private bool isOccupied;                           // 해당 존에 유닛이 배치되어 있는지 여부
    private GameObject zoneObject;                      // 존을 표현할 게임 오브젝트 facilityZone or unit
    public void SetPosition(Vector3Int pos) {
        cellPosition = pos;
    }
    public Vector3Int GetPosition() {
        return cellPosition;
    }
    public void SetOccupied(bool value) {
        isOccupied = value;
    }
    public bool IsOccupied() {
        return isOccupied;
    }
    public void SetZoneObject(GameObject obj) {
        if (obj.GetComponent<FacilityZone>() == null && obj.GetComponent<UnitZone>() == null) {
            Debug.LogError("Zone object must be FacilityZone or Unit");
            return;
        }
        SetOccupied(true);
        zoneObject = obj;
    }
    public GameObject GetZoneObject() {
        return zoneObject;
    }
    public void ClearZoneObject() {
        SetOccupied(false);
        zoneObject = null;
    }
}
