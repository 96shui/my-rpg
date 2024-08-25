using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blackhole_Skill : Skill
{

    [SerializeField] private UI_SkillTreeSlot blackeholeUnlockButton;
    public bool blackeholeUnlocked {  get; private set; }
    [SerializeField] private int attackAmount;
    [SerializeField] private float cloneAttackCooldown;
    [SerializeField] private float blackholeDuration;
    [Space] 
    [SerializeField] private GameObject blackholePrefab;
    [SerializeField] private  float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private  float shrinkSpeed;

    Blackhole_Skill_controller currentBlackhole;


    private void UnlockBlackhole()
    {
        if(blackeholeUnlockButton.unlocked) 
            blackeholeUnlocked = true;
    }

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackhole=Instantiate(blackholePrefab,player.transform.position,Quaternion.identity);

        currentBlackhole =newBlackhole.GetComponent<Blackhole_Skill_controller>();

        currentBlackhole.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, attackAmount, cloneAttackCooldown,blackholeDuration);
    }

    protected override void Start()
    {
        base.Start();

        blackeholeUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBlackhole);
    }

    protected override void Update()
    {
        base.Update();
    }

    public bool SkillCompleted()
    {
        if (!currentBlackhole)
        {
            return false;
        }

        if (currentBlackhole.playerCanExitState)
        {
            currentBlackhole = null;
            return true;
        }

        return false;
    }


    public float GetBlackholeRadius()
    {
        return maxSize / 2;
    }

    protected override void CheckUnlock()
    {
        base.CheckUnlock();

        UnlockBlackhole();
    }
}
