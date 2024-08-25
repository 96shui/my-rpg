using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_FadeScreen : MonoBehaviour
{
    [SerializeField] private Animator anim;


    void Start()
    {
        anim = GetComponent<Animator>();
        
    }

    public void FadeOut()//淡出动画
    {
        anim.SetTrigger("fadeOut");
    }
    public void FadeIn()//淡入动画
    {
        anim.SetTrigger("fadeIn");
    }

}
