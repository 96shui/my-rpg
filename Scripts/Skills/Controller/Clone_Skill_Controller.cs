using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private Player player;
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colorLoosingSpeed;

    private float cloneTimer;
    private float attackMultipller;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius=.8f;
    private Transform closestEnemy;
    private int facingDir = 1;

    private bool canDuplicateClone;
    private float chanceToDuplicate;


    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        cloneTimer-=Time.deltaTime;

        if (cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime*colorLoosingSpeed));

            if (sr.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetupClone(Transform _newTransform,float _cloneDuration,bool _canAttack,Vector3 _offset,Transform _closestEnemy,bool _canDuplicate,float _chanceToDuplicate,Player _player,float _attackMultipller)
    {
        if (_canAttack)
        {
            anim.SetInteger("AttackNumber", Random.Range(1, 3));
        }

        attackMultipller = _attackMultipller;
        player= _player;
        transform.position= _newTransform.position + _offset;
        cloneTimer = _cloneDuration;

        closestEnemy = _closestEnemy;
        chanceToDuplicate = _chanceToDuplicate;
        canDuplicateClone = _canDuplicate;
        FaceClosestTarget();
    }

    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                //player.stats.DoDamage(hit.GetComponent<CharacterStats>());

                hit.GetComponent<Entity>().SetupKnockbackDir(transform);

                PlayerStats playerStats = player.GetComponent<PlayerStats>();
                EnemyStats enemyStats = hit.GetComponent<EnemyStats>();

                playerStats.CloneDoDamage(enemyStats, attackMultipller);

                if (player.skill.clone.canApplyOnHitEffect)
                {
                    if (Inventory.instance.GetEquipment(EquipmentType.Weapon) != null)//防止武器为空时的报错
                        Inventory.instance.GetEquipment(EquipmentType.Weapon).Effect(hit.transform);
                }

                    if (canDuplicateClone)
                {
                    if (Random.Range(0, 100) < chanceToDuplicate)
                    {
                        SkillManager.instance.clone.CreatClone(hit.transform, new Vector3(.5f*facingDir, 0));
                    }
                }
            }
        }
    }

    private void FaceClosestTarget()
    {
        

        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
