using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class ZoneManager : MonoBehaviour {
    public Tilemap zoneTilemap;  // 존을 표시할 타일맵
    public TileBase zoneTile;    // 존을 표현할 기본 타일

    public GameObject farmPrefab; // 농장 프리팹

    [SerializeField]
    private List<Zone> zones = new List<Zone>();

    private int gridWidth = 5;
    private int gridHeight = 5;

    void Start() {
        // 5×5 크기의 존을 생성 (모두 비어있는 상태)
        for (int x = 0; x < gridWidth; x++) {
            for (int y = 0; y < gridHeight; y++) {
                Zone newZone = new Zone();
                newZone.SetPosition(new Vector3Int(x, y, 0));
                newZone.SetOccupied(false); // 처음에는 아무것도 없음
                zones.Add(newZone);
            }
        }
        DrawZones();

        PlaceZone(new Vector3Int(2, 2, 0), farmPrefab);
    }

    // 새로운 존을 추가하는 함수 (기본적으로 비어 있는 존)
    public void AddZone(Vector3Int pos) {
        //시설이나 유닛 설치가 아닌 땅을 확장하는 개념
        Zone newZone = new Zone();
        newZone.SetPosition(pos);
        newZone.SetOccupied(false);
        zones.Add(newZone);
        DrawZones();
    }

    // 타일맵에 모든 존을 그리는 함수 (isOccupied 여부와 상관없이 모든 존을 그림)
    void DrawZones() {
        zoneTilemap.ClearAllTiles(); // 기존 타일 초기화
        
        foreach (Zone zone in zones) {
            zoneTilemap.SetTile(zone.GetPosition(), zoneTile); // 모든 존을 타일로 표시
        }
    }

    // 특정 존의 점유 상태를 변경하는 함수 (예: 건물 설치 여부)
    public void SetZoneOccupied(Vector3Int pos, bool occupied) {
        foreach (Zone zone in zones) {
            if (zone.GetPosition() == pos) {
                zone.SetOccupied(occupied);
                break;
            }
        }
    }

    // 특정 좌표의 존을 가져오는 함수
    public Zone GetZone(Vector3Int pos) {
        return zones.Find(zone => zone.GetPosition() == new Vector3Int(pos.x, pos.y, 0));
    }

    public void PlaceZone(Vector3Int pos, GameObject zoneObject){
        // 특정 존을 찾아서 해당 위치에 농장 배치
        Zone selectedZone = zones.Find(zone => zone.GetPosition() == pos);
        
        if (selectedZone != null && selectedZone.IsOccupied() == false) {
            // Farm 프리팹을 해당 위치에 배치
            GameObject zoneObj = Instantiate(zoneObject, zoneTilemap.CellToWorld(pos) + new Vector3(0,1.5f,-11), Quaternion.identity);

            selectedZone.SetZoneObject(zoneObj); // Zone에 FacilityZone 연결
        } 
        else {
            if (selectedZone == null) 
                Debug.LogWarning("해당 위치에 존이 없습니다.");
            else if (selectedZone.IsOccupied())
                Debug.LogWarning("해당 존은 이미 점유되어 있습니다.");
        }
    }
}
