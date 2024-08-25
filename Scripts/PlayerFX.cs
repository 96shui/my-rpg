using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFX : EntityFX
{
    [Header("After image fx")]
    [SerializeField] private GameObject afterImagePrefab;
    [SerializeField] private float colorLooseRate;
    [SerializeField] private float afterImageCooldown;
    private float afterImageCooldownTimer;


    [Header("Screen shake")]
    private CinemachineImpulseSource screenShake;
    [SerializeField] private float shakeMultipller;
    public Vector3 shakeSwordImpack;
    public Vector3 shakeHighDamage;

    [Space]
    [SerializeField] private ParticleSystem dushFX;

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
        screenShake = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        afterImageCooldownTimer -= Time.deltaTime;
    }

    public void ScreenShake(Vector3 _shakePower)
    {
        screenShake.m_DefaultVelocity = new Vector3(_shakePower.x * player.facingDir, _shakePower.y) * shakeMultipller;
        screenShake.GenerateImpulse();
    }

    public void CreateAfterImage()//创建残影函数
    {
        if (afterImageCooldownTimer < 0)
        {
            afterImageCooldownTimer = afterImageCooldown;
            GameObject newAfterImage = Instantiate(afterImagePrefab, transform.position, transform.rotation);
            newAfterImage.GetComponent<AfterImageFX>().SetupAfterImage(colorLooseRate, sr.sprite);
        }

    }

    public void PlayDustFX()//扔剑产生灰尘
    {
        if (dushFX != null)
            dushFX.Play();
    }
}
