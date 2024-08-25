using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dodge_Skill : Skill
{
    [Header("Dodge")]
    [SerializeField] private UI_SkillTreeSlot unlockDoggeButton;
    [SerializeField] private int evasionAmount;
    public bool doggeUnlocked;

    [Header("Mirage dodge")]
    [SerializeField] private UI_SkillTreeSlot unlockMirageDoggeButton;
    public bool dodgemirageUnlocked;

    protected override void Start()
    {
        base.Start();

        unlockDoggeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        unlockMirageDoggeButton.GetComponent<Button>().onClick.AddListener(UnlockMirageDogge);
    }
    protected override void CheckUnlock()
    {
        UnlockDodge();
        UnlockMirageDogge();
    }
    private void UnlockDodge()
    {
        if (unlockDoggeButton.unlocked && !doggeUnlocked)
        {

            if (evasionAmount > 0)
            {
                player.stats.evasion.AddModifier(evasionAmount);
                
            }
                     Inventory.instance.UpdateStatsUI();
            doggeUnlocked = true;

        }
    }
    private void UnlockMirageDogge()
    {
        if (unlockMirageDoggeButton.unlocked)
            dodgemirageUnlocked = true;
    }

    public void CreateMirageOnDoDogge()
    {
        if (dodgemirageUnlocked)
            SkillManager.instance.clone.CreatClone(player.transform, Vector3.zero);
    }




}
