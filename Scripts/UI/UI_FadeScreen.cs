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

    public void FadeOut()//��������
    {
        anim.SetTrigger("fadeOut");
    }
    public void FadeIn()//���붯��
    {
        anim.SetTrigger("fadeIn");
    }

}
