using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill_controller : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;

    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;
    private float blackholeTimer;

    private bool canGrow=true;
    private bool canShrink;
    private bool canCreateHotKeys=true;
    private bool doneAttackReleased;
    private bool playerCanDisappear=true;

    private int attackAmount=4;
    private float cloneAttackCooldown = .3f;
    private float cloneAttackTimer;

    private List<Transform> tragets = new List<Transform>();
    private List<GameObject> createdHotKey =new List<GameObject>();

    public bool playerCanExitState {  get; private set; }

    public void SetupBlackhole(float _maxSize,float _growSpeed,float _shrinkSpeed,int _attackAmount,float _cloneAttackCooldown,float _blackDuration)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        attackAmount = _attackAmount;
        cloneAttackCooldown = _cloneAttackCooldown;
        blackholeTimer = _blackDuration;

        if (SkillManager.instance.clone.crystalInsteadOfClone)
        {
            playerCanDisappear = false;
        }
    }

    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackholeTimer -= Time.deltaTime;

        if (blackholeTimer < 0)
        {
            blackholeTimer=Mathf.Infinity;

            if (tragets.Count > 0)
            {
                ReleaseCloneAttack();
            }
            else
            {
                FinishBlackholeAbility();
            }
        }


        if (Input.GetKeyUp(KeyCode.R))
        {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();

        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), growSpeed * Time.deltaTime);

            if (transform.localScale.x < 0)
            {
                Destroy(gameObject);

            }
        }
    }

    private void ReleaseCloneAttack()
    {
        if (tragets.Count <= 0)
        {
            return;
        }

        DestroyHotKey();
        doneAttackReleased = true;
        canCreateHotKeys = false;

        if (playerCanDisappear)
        {
            playerCanDisappear = false;
            PlayerManager.instance.player.fx.MakeTransprent(true);
        }
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && doneAttackReleased&&attackAmount>0)
        {
            cloneAttackTimer = cloneAttackCooldown;

            int randomIndex = Random.Range(0, tragets.Count);

            float xOffset;

            if (Random.Range(0, 100) > 50)
            {
                xOffset = 2;
            }
            else
            {
                xOffset = -2;
            }

            if (SkillManager.instance.clone.crystalInsteadOfClone)
            {
                SkillManager.instance.crystal.CreateCrystal();
                SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget();
            }
            else
            {

                SkillManager.instance.clone.CreatClone(tragets[randomIndex], new Vector3(xOffset, 0, 0));

            }

            attackAmount--;

            if (attackAmount <= 0)
            {
                Invoke("FinishBlackholeAbility", 1f);
            }
        }
    }

    private void FinishBlackholeAbility()
    {
        DestroyHotKey();
        playerCanExitState = true;
        canShrink = true;
        doneAttackReleased = false;


    }

    private void DestroyHotKey()
    {
        if (createdHotKey.Count < 0)
            return;

        for (int i = 0; i < createdHotKey.Count; i++)
        {
            Destroy(createdHotKey[i]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);
            CreatHotKey(collision);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent <Enemy>().FreezeTime(false);
        }
    }

    private void CreatHotKey(Collider2D collision)
    {
        if (keyCodeList.Count <= 0)
        {
            Debug.LogWarning("没用足够的热键");
            return;
        }

        if (!canCreateHotKeys)
        {
            return;
        }

        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotKey.Add(newHotKey);


        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(choosenKey);

        Blackhole_HotKey_Controller newHotKeyScript = newHotKey.GetComponent<Blackhole_HotKey_Controller>();

        newHotKeyScript.SetupHotKey(choosenKey, collision.transform, this);
    }

    public void AddEnemyToList(Transform _enemyTransform)=>tragets.Add(_enemyTransform);
}
