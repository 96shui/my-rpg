
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask
}
[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;

    [Header("Unique effect")]
    public float itemCooldown;
    public ItemEffect[] itemEffects;

    [TextArea]
    public string itemEffectDescription;//给装备加上独特的描述


    [Header("Major stats")]
    public int strength; // 力量 增伤1点 爆伤增加 1% 物抗
    public int agility;// 敏捷 闪避 1% 闪避几率增加 1%
    public int intelligence;// 1 点 魔法伤害 1点魔抗 
    public int vitality;//加血的

    [Header("Offensive stats")]
    public int damage;
    public int critChance;      // 暴击率
    public int critPower;       //150% 爆伤

    [Header("Defensive stats")]
    public int maxHealth;
    public int armor;
    public int evasion;//闪避值
    public int magicResistance;

    [Header("Magic stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightingDamage;

    [Header("Craft requirements")]
    public List<InventoryItem> craftingMaterials;

    private int descriptionlength;

    public void Effect(Transform _enemyPosition)//循环调用Effect类里的Effect的函数
    {
        foreach (var item in itemEffects)
        {
            item.ExecuteEffect(_enemyPosition);
        }
    }


    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intellgence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);

        playerStats.damage.AddModifier(damage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critPower.AddModifier(critPower);

        playerStats.maxHealth.AddModifier(maxHealth);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);

        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightingDamage.AddModifier(lightingDamage);
    }

    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intellgence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);

        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critPower.RemoveModifier(critPower);

        playerStats.maxHealth.RemoveModifier(maxHealth);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);

        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightingDamage.RemoveModifier(lightingDamage);

    }

    public override string GetDescription()
    {

        sb.Length = 0;
        descriptionlength = 0;


        AddItemDescription(strength, "strength");
        AddItemDescription(agility, "agility");
        AddItemDescription(intelligence, "intelligence");
        AddItemDescription(vitality, "vitality");

        AddItemDescription(damage, "damage");
        AddItemDescription(critChance, "critChance");
        AddItemDescription(critPower, "critPower");

        AddItemDescription(maxHealth, "health");
        AddItemDescription(evasion, "evasion");
        AddItemDescription(armor, "armor");
        AddItemDescription(magicResistance, "magicResistance");

        AddItemDescription(fireDamage, "fireDamage");
        AddItemDescription(iceDamage, "iceDamage");
        AddItemDescription(lightingDamage, "lightingDamage");

        Debug.Log("In GetDescription");


        if (descriptionlength < 5)
        {
            for (int i = 0; i < 5 - descriptionlength; ++i)
            {
                sb.AppendLine();
                sb.Append("");
            }
        }

        if (itemEffectDescription.Length > 0)//给装备加上独特的描述
        {
            sb.AppendLine();
            sb.Append(itemEffectDescription);
        }

        return sb.ToString();
    }

    private void AddItemDescription(int _value, string _name)
    {
        if (_value != 0)
        {
            
            if (_value > 0)
            {
                sb.AppendLine("-"+ _name + ":" + _value);

            }

            descriptionlength++;
        }
    }
}
