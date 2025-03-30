using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapManager : MonoBehaviour {
    // 타일 에셋을 인스펙터에서 할당 (TileBase 또는 Tile)
    public TileBase tileAsset;
    // 타일맵 컴포넌트 참조
    public Tilemap tilemap;
    // 타일 그리드의 크기 설정
    public int gridSize = 5;

    void Start() {
        // 5x5 그리드에 타일을 채우기
        for (int x = 0; x < gridSize; x++) {
            for (int y = 0; y < gridSize; y++) {
                Vector3Int tilePos = new Vector3Int(x, y, 0);
                tilemap.SetTile(tilePos, tileAsset);
            }
        }
    }
}
