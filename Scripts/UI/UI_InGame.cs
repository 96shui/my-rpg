using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;

    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image swordImage;
    [SerializeField] private Image blackholeImage;
    [SerializeField] private Image flaskholeImage;


    [Header("Souls info")]
    [SerializeField] private TextMeshProUGUI currentSouls;
    [SerializeField] private float soulsAmount;//�������
    [SerializeField] private float increaseRate = 100;//��������


    private SkillManager skills;
    private void Start()
    {
        if (playerStats != null)
        {
            playerStats.onHealthChanged += UpdateHealthUI;
        }

        skills = SkillManager.instance;
    }

    private void Update()
    {
        UpdateSoulsUI();//��������ʱ��仯����Ч����


        if (Input.GetKeyDown(KeyCode.LeftShift)&& skills.dash.dashUnlocked)
        {
            SetCooldownOf(dashImage);
        }
        if (Input.GetKeyDown(KeyCode.Q) && skills.parry.parryUnlocked)
        {
            SetCooldownOf(parryImage);
        }
        if (Input.GetKeyDown(KeyCode.F) && skills.crystal.crystalUnlocked)
        {
            SetCooldownOf(crystalImage);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && skills.sword.swordUnlocked)
        {
            SetCooldownOf(swordImage);
        }
        if (Input.GetKeyDown(KeyCode.R) && skills.blackhole.blackeholeUnlocked)
        {
            SetCooldownOf(blackholeImage);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && Inventory.instance.GetEquipment(EquipmentType.Flask) != null)
        {
            SetCooldownOf(flaskholeImage);
        }

        CheckCooldowmOf(dashImage, skills.dash.cooldown);
        CheckCooldowmOf(parryImage, skills.parry.cooldown);
        CheckCooldowmOf(crystalImage, skills.crystal.cooldown);
        CheckCooldowmOf(swordImage, skills.sword.cooldown);
        CheckCooldowmOf(blackholeImage, skills.blackhole.cooldown);
        CheckCooldowmOf(flaskholeImage, Inventory.instance.flaskCooldown);

    }

    private void UpdateSoulsUI()//��������ʱ��仯����Ч����
    {
        //�߼����ж�Ŀǰ��Ǯ����ʵ�ʵ�Ǯ����������*ʱ��ķ�ʽ����ֱ����ͬ���ߴ��ں����ʵ�ʵ�Ǯ
        if (soulsAmount < PlayerManager.instance.GetCurrency())
        {
            soulsAmount += Time.deltaTime * increaseRate;
        }
        else
            soulsAmount = PlayerManager.instance.GetCurrency();

        currentSouls.text = ((int)soulsAmount).ToString();
    }


    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetMaxHealthValue();
        slider.value = playerStats.currentHealth;


    }

    private void SetCooldownOf(Image _image)
    {
        if(_image.fillAmount <= 0)
        {
            _image.fillAmount = 1;
        }
    }

    private void CheckCooldowmOf(Image _image,float _cooldown)
    {
        if (_image.fillAmount > 0) 
            _image.fillAmount -= 1/_cooldown*Time.deltaTime;
    }
}
