using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainScene";
    [SerializeField] private GameObject continueButton;
    [SerializeField] private UI_FadeScreen fadeScreen;

    private void Start()
    {
        if(SaveManager.instance.HasSaveData() == false)
        {
            continueButton.SetActive(false);
        }
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadSceneWithFadeEffect(1.5f));
    }

    public void NewGame()
    {
        SaveManager.instance.DeleteSaveData();
        StartCoroutine(LoadSceneWithFadeEffect(1.5f));
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadSceneWithFadeEffect(float _delay)//延迟载入显示渐变动画函数
    {
        fadeScreen.FadeOut();

        yield return new WaitForSeconds(_delay);

        SceneManager.LoadScene(sceneName);//通过场景名切换场景函数

        //作为Unity自带的方法，自然是直接可以通过文件名找到Unity内部的文件的
    }

}
