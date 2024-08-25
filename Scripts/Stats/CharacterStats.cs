using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public enum StatType
{

    strength,
    agility,
    intelligence,
    vitality,
    damage,
    critChance,
    critPower,
    health,
    armor,
    evasion,
    magicResistance,
    fireDamage,
    iceDamage,
    lightingDamage
}
public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;

    [Header("Major stats")]
    public Stat strength; // ���� ����1�� �ٽ�ֵpower 1% �￹
    public Stat agility;// ���� ���� 1% �ٽ�chance 1%
    public Stat intellgence;// 1 �� ħ���˺� 1��ħ�� 
    public Stat vitality;//��Ѫ��

    [Header("Offensive stats")]
    public Stat damage;
    public Stat critChance;      // ������
    public Stat critPower;       //150% ����

    [Header("Defensive stats")]
    public Stat maxHealth;
    public Stat armor;//����
    public Stat evasion;//����ֵ
    public Stat magicResistance;//ħ��

    [Header("Magic stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightingDamage;

    public bool isIgnited;// ��������
    public bool isChilded;//����20%���ף�����
    public bool isShocked;//����20%������

    [SerializeField] private float allmentsDuration=4;
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;


    private float igniteDamageCooldown = .3f;
    private float ignitedDamageTimer;
    [SerializeField] private GameObject shockStrikePrefab;
    private int igniteDamage;

    private int shockDamage;


    public int currentHealth;

    public System.Action onHealthChanged;
    public bool isDead { get; private set; }
    public bool isInvincible {  get; private set; }
    private bool isVulnerable;


    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);

        currentHealth = GetMaxHealthValue();

        fx = GetComponent<EntityFX>();


    }

    protected virtual void Update()
    {

        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;
        ignitedDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
            isIgnited = false;
        if (chilledTimer < 0)
            isChilded = false;
        if (shockedTimer < 0)
            isShocked = false;

        if(isIgnited)
        ApplyIgnitedDamage();
    }

    public void MakeVulnerableFor(float _duration) => StartCoroutine(VulnerableCorutine(_duration));//����Ч������
    private IEnumerator VulnerableCorutine(float _duration)
    {
        isVulnerable = true;
        yield return new WaitForSeconds(_duration);
        isVulnerable = false;
    }


    public virtual void IncreaseStatBy(int _modifier, float _duration, Stat _statToModify)
    {
        StartCoroutine(StatModCoroutine(_modifier, _duration, _statToModify));
    }
    private IEnumerator StatModCoroutine(int _modifier, float _duration, Stat _statToModify)
    {
        _statToModify.AddModifier(_modifier);
        yield return new WaitForSeconds(_duration);
        _statToModify.RemoveModifier(_modifier);
    }


    public virtual void DoDamage(CharacterStats _targetStats)
    {
        bool criticalStrike = false;

        if (TargetCanAvoidAttack(_targetStats))
        {
            return;
        }

        _targetStats.GetComponent<Entity>().SetupKnockbackDir(transform);

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
        {
            totalDamage =CalculateCriticalDamage(totalDamage);
            criticalStrike = true;
        }

        fx.CreateHitFX(_targetStats.transform,criticalStrike);

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        _targetStats.TakeDamage(totalDamage);

        DoMagicalDamage(_targetStats);

    }

    #region Magical damage and allments

    public virtual void DoMagicalDamage(CharacterStats _targetStats )
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();

        int totalMagicalDamage = _fireDamage + _iceDamage + _lightingDamage + intellgence.GetValue();
        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);

        _targetStats.TakeDamage(totalMagicalDamage);


        //��ֹѭ��������Ԫ���˺�Ϊ0ʱ������ѭ��
        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
            return;

        bool canApplyIgnite, canApplyChill, canApplyShock;
        AttemptyToApplyAilements(_targetStats, _fireDamage, _iceDamage, _lightingDamage, out canApplyIgnite, out canApplyChill, out canApplyShock);

        //����ȼ�˺���ֵ
        if (canApplyIgnite)
        {
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));
        }

        if (canApplyShock)
            _targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(_lightingDamage * .1f));

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);

    }

    private  void AttemptyToApplyAilements(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightingDamage, out bool canApplyIgnite, out bool canApplyChill, out bool canApplyShock)
    {
        canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        canApplyChill = _iceDamage > _lightingDamage && _iceDamage > _fireDamage;
        canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;


        //Ϊ�˷�ֹ����Ԫ���˺�һ�¶������޷�����Ԫ��Ч��
        //ѭ���жϴ���ĳ��Ԫ��Ч��
        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (Random.value < .25f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                Debug.Log("Ignited");
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if (Random.value < .35f && _iceDamage > 0)
            {
                canApplyChill = true;
                Debug.Log("Chilled");
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if (Random.value < .55f && _lightingDamage > 0)
            {
                canApplyShock = true;
                Debug.Log("Shocked");
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }




        }
    }


    public void ApplyAilments(bool _ignite,bool _chill,bool _shock)
    {
        bool canApplyIgnite = !isIgnited && !isChilded && !isShocked;
        bool canApplyChill = !isIgnited && !isChilded && !isShocked;
        bool canApplyShock = !isIgnited && !isChilded ;

        if (_ignite &&canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedTimer = allmentsDuration;

            fx.IgniteFxFor(allmentsDuration);
        }
        if (_chill&&canApplyChill)
        {
            isChilded = _chill;
            chilledTimer = allmentsDuration;

            float slowPercentage = .2f;
            GetComponent<Entity>().SlowEntityBy(slowPercentage,allmentsDuration);
            fx.ChillFxFor(allmentsDuration);
        }
        if (_shock && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(_shock);
            }
            else
            {
                if (GetComponent<Player>() != null)//��ֹ���ֵ���ʹ��ҽ���shock״̬��Ҳ��������
                    return;
                HitNearestTargetWithShockStrike();
            }//isShockΪ��ʱ����ִ�еĺ���ΪѰ������ĵ��ˣ���������ʵ������������
 

        }


            
    }

    public void ApplyShock(bool _shock)
    {
        if (isShocked)
            return;

        isShocked = _shock;
        shockedTimer = allmentsDuration;

        fx.ShockFxFor(allmentsDuration);
    }

    private void HitNearestTargetWithShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);//�ҵ������Լ���������ײ��

        float closestDistance = Mathf.Infinity;//�������ı�ʾ��ʽ��ֻ����
        Transform closestEnemy = null;


        //https://docs.unity3d.com/cn/current/ScriptReference/Mathf.Infinity.html
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)// ��ֹ����ĵ��˾���Shock״̬�����Լ�
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);//�õ������֮��ľ���
                if (distanceToEnemy < closestDistance)//�ȽϾ��룬�����ø���������������˵�λ��,�����������
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }

            if (closestEnemy == null)
                closestEnemy = transform;
        }

        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
            newShockStrike.GetComponent<ShockStrike_Controller>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());
        }
    }
    private void ApplyIgnitedDamage()
    {
        if (ignitedDamageTimer < 0 )
        {

            DecreaseHealthBy(igniteDamage);

            if (currentHealth < 0 && !isDead)
            {
                Die();
            }

            ignitedDamageTimer = igniteDamageCooldown;
        }
    }

    #endregion


    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;//����ȼ�˺���ֵ

    public void SetupShockStrikeDamage(int _damage) => shockDamage = _damage;


    public virtual void TakeDamage(int _damage)
    {
        if (isInvincible)
            return;

        DecreaseHealthBy(_damage);

        GetComponent<Entity>().DamageImpact();
        fx.StartCoroutine("FlashFX");

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }

    }

    public virtual void IncreaseHealthBy(int _amount)//��ӻ�Ѫ����
    {
        currentHealth += _amount;

        if (currentHealth > GetMaxHealthValue())
            currentHealth = GetMaxHealthValue();

        if (onHealthChanged != null)
            onHealthChanged();
    }


    protected virtual void DecreaseHealthBy(int _damage)//�˺��������ı䵱ǰ����ֵ����������Ч
    {
        if (isVulnerable)
            _damage =Mathf.RoundToInt( _damage * 1.2f);

        currentHealth -= _damage;

        if(_damage > 0)
        {
            fx.CreatePopUpText(_damage.ToString());
        }

        if (onHealthChanged != null)
        {
            onHealthChanged();
        }
    }


    protected virtual void Die()
    {
        isDead = true;
    }

    public void KillEntity()
    {
        if(!isDead)
        Die();
    }

    public void MakeInvincible(bool _invincible)
    {
        isInvincible = _invincible;
    }

    #region Stat calaulations
    protected static int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilded)
        {
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f);
        }
        else
        {
            totalDamage -= _targetStats.armor.GetValue();

        }
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }
    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intellgence.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    public virtual void OnEvasion()
    {

    }//�ɼ̳гɹ����ܴ����ĺ���

    protected  bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
        {
            totalEvasion += 20;
        }

        if (Random.Range(0, 100) < totalEvasion)
        {
            _targetStats.OnEvasion();
            return true;

        }
        return false;
    }

    protected bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue()+agility.GetValue();

        if(Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }
        return false;
    }

    protected int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue())*.01f;
        float totalCritDamage = _damage*totalCritPower;

        return Mathf.RoundToInt(totalCritPower);
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }

    #endregion

    public Stat GetStat(StatType _startType)
    {
        if (_startType == StatType.strength) return strength;
        else if (_startType == StatType.agility) return agility;
        else if (_startType == StatType.intelligence) return intellgence;
        else if (_startType == StatType.vitality) return vitality;
        else if (_startType == StatType.damage) return damage;
        else if (_startType == StatType.critChance) return critChance;
        else if (_startType == StatType.critPower) return critPower;
        else if (_startType == StatType.health) return maxHealth;
        else if (_startType == StatType.armor) return armor;
        else if (_startType == StatType.evasion) return evasion;
        else if (_startType == StatType.magicResistance) return magicResistance;
        else if (_startType == StatType.fireDamage) return fireDamage;
        else if (_startType == StatType.iceDamage) return iceDamage;
        else if (_startType == StatType.lightingDamage) return lightingDamage;

        return null;
    }
}
