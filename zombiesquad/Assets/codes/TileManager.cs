using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField]
    private Grid grid;

    void Start()
    {
        grid = GetComponent<Grid>();
    }

    public Vector3 GetTileCenter(Vector3 worldPos)
    {
        Vector3Int cellPos = grid.WorldToCell(worldPos);
        return grid.CellToWorld(cellPos) + new Vector3(grid.cellSize.x / 2, grid.cellSize.y / 2, 0);
    }
}