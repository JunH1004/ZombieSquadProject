using System.Collections.Generic;
using UnityEngine;
public enum UnitType { Katana, Pistol, Shotgun, Sniper, MachineGun, RocketLauncher, Grenade, Shield, Heal, Buff, Debuff }
public class UnitZone : MonoBehaviour{
    private int maxUnitNum;
    private UnitType unitType;
    private List<Unit> units = new List<Unit>();
    public void SetMaxUnitNum(int num) {
        maxUnitNum = num;
    }
    public int GetMaxUnitNum() {
        return maxUnitNum;
    }
    public void SetUnitType(UnitType type) {
        unitType = type;
    }
    public UnitType GetUnitType() {
        return unitType;
    }
    public void AddUnit(Unit unit) {
        units.Add(unit);
    }
    
}
