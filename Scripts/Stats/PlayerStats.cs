using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;

    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);

    }

    protected override void Die()
    {
        base.Die();

        player.Die();

        GameManager.instance.lostCurrencyAmount = PlayerManager.instance.currency;
        PlayerManager.instance.currency = 0;

        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

    protected override void DecreaseHealthBy(int _damage)
    {
        base.DecreaseHealthBy(_damage);

        if (_damage > GetMaxHealthValue() * .3f)
        {
            player.SetupKnockbackPower(new Vector2(50,5));
            player.fx.ScreenShake(player.fx.shakeHighDamage);
        }

        ItemData_Equipment currentArmor =Inventory.instance.GetEquipment(EquipmentType.Armor);

        if (currentArmor != null)
            currentArmor.Effect(player.transform);
    }

    public override void OnEvasion()
    {
        player.skill.dodge.CreateMirageOnDoDogge();
    }

    public void CloneDoDamage(CharacterStats _targetStats,float _multipller)
    {
        if (TargetCanAvoidAttack(_targetStats))
        {
            return;
        }


        int totalDamage = damage.GetValue() + strength.GetValue();

        if (_multipller>0)
        {
            totalDamage = Mathf.RoundToInt(totalDamage * _multipller);
        }

        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }


        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        _targetStats.TakeDamage(totalDamage);

        DoMagicalDamage(_targetStats);
    }
}
