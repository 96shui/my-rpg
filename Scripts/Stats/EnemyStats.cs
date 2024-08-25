using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{


    private Enemy enemy;
    private ItemDrop myDropSystem;
    public Stat soulsDropAmount;

    [Header("Level details")]
    [SerializeField] private int leval = 1;

    [Range(0f, 1f)]//一个使数值设置成为一定范围的设置
    [SerializeField] private float percantageModifier = .4f;//设置等级和成长比例

    public override void DoDamage(CharacterStats _targetStats)
    {
        base.DoDamage(_targetStats);
    }

    protected override void Start()
    {
        //改变伤害和生命值
        //解决初始血量在升级后不满
        soulsDropAmount.SetDefaultValue(100);
        ApplyLevelModifier();

        base.Start();

        enemy = GetComponent<Enemy>();
        myDropSystem = GetComponent<ItemDrop>();

    }

    private void ApplyLevelModifier()
    {
        Modify(strength);
        Modify(agility);
        Modify(intellgence);
        Modify(vitality);

        Modify(damage);
        Modify(critChance);
        Modify(critPower);

        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightingDamage);

        Modify(soulsDropAmount);
    }
    //专门对某个数值进行提升的函数
    private void Modify(Stat _stat)
    {
        for (int i = 1; i < leval; i++)
        {
            float modifier = _stat.GetValue() * percantageModifier;

            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }



    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);

    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();


        PlayerManager.instance.currency += soulsDropAmount.GetValue();
        myDropSystem.GenerateDrop();

        Destroy(gameObject, 5f);
    }
}
