using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public float cooldown;
        public float cooldownTimer;

    protected Player player;

    private void Awake()
    {
        player = PlayerManager.instance.player;
    }

    protected virtual void Start()
    {
        
        CheckUnlock();

    }

    protected virtual void Update()
    {
        cooldownTimer=cooldownTimer-Time.deltaTime;
    }

    protected virtual void CheckUnlock()
    {

    }

    public virtual bool CanUseSkill()
    {
        if(cooldownTimer < 0)
        {
            UseSkill();
            cooldownTimer=cooldown;
            return true;
        }
        else
        {
            
            player.fx.CreatePopUpText("¼¼ÄÜÀäÈ´ÖÐ");
            return false;
        }
    }

    public virtual void UseSkill()
    {

    }

    protected virtual Transform FindClosestEnemy(Transform _checkTransform)
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, 100);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy =null;

        foreach (var hit in colliders)
        {
            
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(_checkTransform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                    
                }
            }

        }
        
        return closestEnemy;

    }
}
