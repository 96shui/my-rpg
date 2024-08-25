using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManage : MonoBehaviour
{
    public static AudioManage instance;

    [SerializeField] private float sfxMinmumDistance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    public bool playBgm;
    private int bgmIndex;

    private bool canPlaySFX=false;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
            instance = this;

        Invoke("AllowSFX", 1f);//让游戏延迟一秒后才能发出声效

    }

    private void Update()
    {
        if(!playBgm) 
            StopAllBGM();
        else
        {
            if (!bgm[bgmIndex].isPlaying)
            {
                PlayBGM(bgmIndex);
            }
        }
    }

    public void PlaySFX(int  _sfxIndex,Transform _source)
    {
        if (canPlaySFX == false)
            return;

        if (sfx[_sfxIndex].isPlaying)
            return;

        if(_source != null && Vector2.Distance(PlayerManager.instance.player.transform.position,_source.position)>sfxMinmumDistance)
        {
            return;
        }

        if (_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].pitch = Random.Range(0.85f, 1.15f);
            sfx[_sfxIndex].Play();
        }
    }

    public void StopSFX(int _sfxIndex) =>sfx[_sfxIndex].Stop();

    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }

    public void PlayBGM(int _bgmIndex)
    {
        bgmIndex = _bgmIndex;
        StopAllBGM();
        if (bgmIndex < sfx.Length)
        {
            bgm[bgmIndex].Play();
        }
    }

    public void StopAllBGM()
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }

    private void AllowSFX()
    {
        canPlaySFX = true;
    }

    public void StopSFXWithTime(int _index)
    {
        StartCoroutine(DecreaseVolume(sfx[_index]));
    }

    private IEnumerator DecreaseVolume(AudioSource _audio)//退出区域后，声音缓慢减少
    {
        float defaultVolume = _audio.volume;

        while (_audio.volume > .1f)
        {
            _audio.volume -= _audio.volume * .2f;
            yield return new WaitForSeconds(.6f);
            if (_audio.volume <= .1f)
            {
                _audio.Stop();
                _audio.volume = defaultVolume;

                break;
            }
        }

    }



}
