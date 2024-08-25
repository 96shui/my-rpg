using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Parry_Skill : Skill
{
    [Header("Perry")]
    [SerializeField] private UI_SkillTreeSlot parryUnlockButton;
    public bool parryUnlocked {  get; private set; }

    [Header("Restore Health")]
    [SerializeField] private UI_SkillTreeSlot restoreUnlockButton;
    [Range(0f, 1f)]
    [SerializeField] private float restoreHealthPerentage;
    public bool restoreUnlocked {  get; private set; }

    [Header("Parry Mirage")]
    [SerializeField] private UI_SkillTreeSlot parryWithUnlockButton;
    public bool parryWithMirageUnlocked {  get; private set; }

    public override void UseSkill()
    {
        base.UseSkill();

        if (restoreUnlocked)
        {
            int restoreAmount = Mathf.RoundToInt(player.stats.GetMaxHealthValue() * restoreHealthPerentage);
            player.stats.IncreaseHealthBy(restoreAmount);
        }
    }


    protected override void Start()
    {
        base.Start();
        parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        restoreUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryRestore);
        parryWithUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryWithMirage);
    }

    protected override void CheckUnlock()
    {
        UnlockParry();
        UnlockParryRestore();
        UnlockParryWithMirage();
    }
    private void UnlockParry()
    {
        
            if (parryUnlockButton.unlocked == true)
            {
                parryUnlocked = true;

            }
    }
    private void UnlockParryRestore()
    {
        if (restoreUnlockButton.unlocked ==true)
        {
            restoreUnlocked = true;
        }
    }
    private void UnlockParryWithMirage()
    {
        if (parryWithUnlockButton.unlocked == true)
        {
            parryWithMirageUnlocked = true;
        }
    }

    public void MakeMirageOnParry(Transform _respawnTransform)
    {
        if (parryWithMirageUnlocked)
            player.skill.clone.CreatCloneWithDelay(_respawnTransform);
    }


}
