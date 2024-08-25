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

    IEnumerator LoadSceneWithFadeEffect(float _delay)//�ӳ�������ʾ���䶯������
    {
        fadeScreen.FadeOut();

        yield return new WaitForSeconds(_delay);

        SceneManager.LoadScene(sceneName);//ͨ���������л���������

        //��ΪUnity�Դ��ķ�������Ȼ��ֱ�ӿ���ͨ���ļ����ҵ�Unity�ڲ����ļ���
    }

}
