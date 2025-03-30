using UnityEngine;

public interface ICardAction
{
    void Execute();
    void Execute(ZoneManager zoneManager, Vector3Int targetPosition);
}
