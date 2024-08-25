using System.Collections;
using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EntityFX : MonoBehaviour
{
    protected Player player;
    protected SpriteRenderer sr;

    [Header("Pop up Text")]
    [SerializeField] private GameObject popUpTextPrefab;


    [Header("Flash")]
    [SerializeField] private float flashDuartion;
    [SerializeField] private Material hitMat;
     protected Material originalMat;

    [Header("Aliment colors")]
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;

    [Header("Aliment particles")]
    [SerializeField] private ParticleSystem igniteFX;
    [SerializeField] private ParticleSystem chillFX;
    [SerializeField] private ParticleSystem shockFX;

    [Header("HitFX")]
    [SerializeField] private GameObject hitFX;
    [SerializeField] private GameObject criticalHitFX;



    protected virtual void Awake()
    {
        
    }

    protected virtual void Start()
    {
        sr=GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
        player = PlayerManager.instance.player;
        
    }


    public void CreatePopUpText(string _text)
    {
        float randomX = Random.Range(-1, 1);
        float randomY = Random.Range(1, 3);

        Vector3 positionOffset = new Vector3(randomX, randomY,0);

        GameObject newText = Instantiate(popUpTextPrefab, transform.position+positionOffset,Quaternion.identity);

        newText.GetComponent<TextMeshPro>().text = _text;
    }

   

    public void MakeTransprent(bool _transprent)
    {
        if (_transprent)
        {
            sr.color = Color.clear;
        }
        else
        {
            sr.color = Color.white;
        }
    }

    public IEnumerator FlashFX()
    {
        sr.material = hitMat;
        Color currentColor =sr.color;

        sr.color = Color.white;

        yield return new WaitForSeconds(flashDuartion);

        sr.color=currentColor;
        sr.material = originalMat;

    }

    public void RedColorBlink()
    {
        if (sr.color != Color.white)
        {
            sr.color = Color.white;
        }
        else
        {
            sr.color = Color.red;
        }
    }


    public void CancelColorChange()//使角色停止闪烁的函数
    {
        CancelInvoke();//取消该 MonoBehaviour 上的所有 Invoke 调用。
                       //https://docs.unity3d.com/cn/current/ScriptReference/MonoBehaviour.CancelInvoke.html
        sr.color = Color.white;

        igniteFX.Stop();
        chillFX.Stop();
        shockFX.Stop();
    }

    public void ShockFxFor(float _second)
    {
        shockFX.Play();

        InvokeRepeating("ShockColorFx", 0, .3f);
        Invoke("CancelColorChange", _second);
    }

    public void ChillFxFor(float _second)
    {
        chillFX.Play();

        InvokeRepeating("ChillColor", 0, .3f);
        Invoke("CancelColorChange", _second);
    }



    public void IgniteFxFor(float _second)
    {
        igniteFX.Play();

        InvokeRepeating("IgniteColorFX", 0, .3f);
        Invoke("CancelColorChange", _second);
    }

    public void IgniteColorFX()
    {
        if (sr.color != igniteColor[0])
            sr.color = igniteColor[0];
        else
            sr.color = igniteColor[1];
    }

    public void ShockColorFx()
    {
        if (sr.color != shockColor[0])
            sr.color = shockColor[0];
        else
            sr.color = shockColor[1];
    }

    public void ChillColor()
    {
        if (sr.color != chillColor[0])
            sr.color = chillColor[0];
        else
            sr.color = chillColor[1];
    }

    public void CreateHitFX(Transform _target,bool _critical)//在打击后创建实体
    {


        float zRotation = Random.Range(-90, 90);
        float xPosition = Random.Range(-.5f, .5f);
        float yPosition = Random.Range(-.5f, .5f);

        Vector3 hitFXRotation = new Vector3(0, 0, zRotation);

        GameObject hitPrefab = hitFX;

        if (_critical)
        {
            hitPrefab = criticalHitFX;
            float yRotation = 0;
            zRotation = Random.Range(-45, 45);

            if (GetComponent<Entity>().facingDir == -1)
            {
                yRotation = 180;
            }

            hitFXRotation = new Vector3(0, yRotation, zRotation);

        }

        GameObject newhitFX = Instantiate(hitPrefab, _target.position + new Vector3(xPosition, yPosition), Quaternion.identity);


        newhitFX.transform.Rotate(hitFXRotation);

        Destroy(newhitFX, .5f);
    }

    
}
