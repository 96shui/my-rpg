using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator anim;
    public string id;
    public bool activationStatus;

    private void Start()
    {
        anim = GetComponent<Animator>();

    }

    [ContextMenu("Generate checkpoint id")]
    private void GenerateId()//制作ID函数
    {
        id = System.Guid.NewGuid().ToString();//不根据path，直接生成id
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            ActivateCheckpoint();
        }
    }

    public void ActivateCheckpoint()//激活检查点函数
    {
        anim.SetBool("active", true);
        activationStatus = true;
    }
}