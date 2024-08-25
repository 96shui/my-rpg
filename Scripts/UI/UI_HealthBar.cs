using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private Entity entity  ;
    private CharacterStats mystats;
    private RectTransform myTransform;
    private Slider slider;

    private void Start()
    {
        myTransform = GetComponent<RectTransform>();
       entity = GetComponentInParent<Entity>();
        slider = GetComponentInChildren<Slider>();
      
        mystats = GetComponentInParent<CharacterStats>();
        

        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = mystats.GetMaxHealthValue();
        slider.value = mystats.currentHealth;

        entity.onFlipped += FlipUI;
        mystats.onHealthChanged += UpdateHealthUI;
    }

    private void Update()
    {
        
    }


    private void FlipUI()//让UI不随着角色翻转
    {
        myTransform.Rotate(0, 180, 0);
    }

    private void OnDisable()
    {
        entity.onFlipped -= FlipUI;
        mystats.onHealthChanged -= UpdateHealthUI;
    }
}
