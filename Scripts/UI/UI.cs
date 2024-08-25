using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour,ISaveManager
{
    [Header("End screen")]
    [SerializeField] private UI_FadeScreen fadeScreen;
    [SerializeField] private GameObject endText;
    [SerializeField] private GameObject restartButton;

    [Space]

    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject inGameUI;


    public UI_ItemTooltip itemTooltip;
    public UI_StatToolTip statToolTip;
    public UI_SkillToolTip skillToolTip;
    public UI_CraftWindow craftWindow;

    [SerializeField] private UI_VolumeSlider[] volumeSettings;

    private void Awake()
    {
        SwitchTo(skillTreeUI);
        fadeScreen.gameObject.SetActive(true);
    }

    void Start()
    {
        SwitchTo(inGameUI);

        itemTooltip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
        skillToolTip.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchWithKeyTo(characterUI);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            SwitchWithKeyTo(craftUI);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            SwitchWithKeyTo(skillTreeUI);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            SwitchWithKeyTo(optionsUI);
        } 

    }

    public void SwitchTo(GameObject _menu)
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            bool fadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null;//保证存在淡入淡出效果的函数时才会为真，才会使darkScreen保持存在
            if (fadeScreen == false)
                transform.GetChild(i).gameObject.SetActive(false);
        }

        if(_menu != null)
        {
            _menu.SetActive(true);
            
        }

        if(GameManager.instance != null)
        {
            if(_menu == inGameUI)
            {
                GameManager.instance.PauseGame(false);
            }
            else
            {
                GameManager.instance.PauseGame(true);
            }
        }
    }

    public void SwitchWithKeyTo(GameObject _menu)//键盘切换窗口函数
    {
        if (_menu != null && _menu.activeSelf)//通过判断是否传入mune和mune是否激活来决定使设置为可视或不可使
        {
            _menu.SetActive(false);

            CheckForInGameUI();

            return;
        }

        SwitchTo(_menu);
    }

    private void CheckForInGameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<UI_FadeScreen>()==null)
                return;
        }

        SwitchTo(inGameUI);
    }

    public void SwitchOnEndScreen()//死亡综合效果函数
    {
        
        fadeScreen.FadeOut();
        StartCoroutine(EndScreenCorutine());
    }

    IEnumerator EndScreenCorutine()//死亡显示文本函数
    {
        yield return new WaitForSeconds(1);
        endText.SetActive(true);
        yield return new WaitForSeconds(1);
        restartButton.SetActive(true);
    }

    public void RestartGameButton()//场景重开函数
    {
        GameManager.instance.RestratScene();//调用GameManager的重开函数
    }

    public void LoadData(GameData _data)
    {
        foreach(KeyValuePair<string, float> pair in _data.volumeSettings)
        {
            foreach(UI_VolumeSlider item in volumeSettings)
            {
                if(item.parametr ==  pair.Key)
                    item.LoadSlider(pair.Value);
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.volumeSettings.Clear();

        foreach(UI_VolumeSlider item in volumeSettings)
        {
            _data.volumeSettings.Add(item.parametr,item.slider.value);
        }
    }
}
