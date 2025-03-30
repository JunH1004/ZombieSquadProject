using UnityEngine;

public class Unit : MonoBehaviour {
    public string unitName;       // 유닛 이름
    public int health = 50;       // 체력 (기본값 50)
    public int attackPower = 10;  // 공격력 (기본값 10)
    public float attackSpeed = 1f; // 공격 속도 (초당 공격 횟수)
}