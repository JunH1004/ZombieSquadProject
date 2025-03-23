using UnityEngine;

public abstract class Weapon
{
    public float Damage { get; protected set; }
    public float Range { get; protected set; }

    public abstract void Attack(GameObject target, PlayerController player);
}

public class Pistol : Weapon
{
    public Pistol()
    {
        Damage = 10f;
        Range = 2f; // PlayerController의 attackRange와 동기화 가능
    }

    public override void Attack(GameObject target, PlayerController player)
    {
        ZombieController zombie = target.GetComponent<ZombieController>();
        if (zombie != null)
        {
            zombie.TakeDamage(Damage);
            Debug.Log($"Pistol attacked {target.name} for {Damage} damage!");
        }
    }
}