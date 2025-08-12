using UnityEngine;

public interface IHitable
{
    bool Hited(int dmg, DamageType damageType, AttackType attackType, GameObject hitter);
    void Damage(int dmg);
    void Heal(int dmg);
    bool IsAlive();
}
