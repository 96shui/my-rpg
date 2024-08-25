using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill
{
    [Header("Dash")]
    [SerializeField] private UI_SkillTreeSlot dashUnlockButton;
    public bool dashUnlocked{ get;private set; }

    [Header("Dash")]
    [SerializeField] private UI_SkillTreeSlot cloneOnDashUnlockedButton;
    public bool cloneOnDashUnlocked {  get; private set; }

    [Header("Dash")]
    [SerializeField] private UI_SkillTreeSlot cloneOnArrivalUnlockedButton;
    public bool cloneOnArrivalUnlocked {  get; private set; }

    public override void UseSkill()
    {
        base.UseSkill();
    }


    protected override void Start()
    {
        base.Start();
        dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        cloneOnDashUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        cloneOnArrivalUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrival);
        
    }

    protected override void CheckUnlock()
    {
        UnlockDash();
        UnlockCloneOnDash();
        UnlockCloneOnArrival();
    }

    private void UnlockDash()
    {
        if (dashUnlockButton.unlocked)
            dashUnlocked = true;
    }

    private void UnlockCloneOnDash()
    {
        if (cloneOnDashUnlockedButton.unlocked)
            cloneOnDashUnlocked = true;
    }

    private void UnlockCloneOnArrival()
    {
        if (cloneOnArrivalUnlockedButton.unlocked)
            cloneOnArrivalUnlocked = true;
    }
    public void CloneOnDash()
    {
        if (cloneOnDashUnlocked)
        {
            SkillManager.instance.clone.CreatClone(PlayerManager.instance.player.transform, Vector3.zero);
        }
    }

    public void CloneOnArrival()
    {

        if (cloneOnArrivalUnlocked)
        {
            SkillManager.instance.clone.CreatClone(PlayerManager.instance.player.transform, Vector3.zero);
        }
    }

}
