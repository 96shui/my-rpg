using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;//关于场景的操作

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance;

    private Transform player;

    [SerializeField] private Checkpoint[] checkpoints;
    [SerializeField] private string closestCheckpointId;

    [Header("Lost currency")]
    [SerializeField] private GameObject lostCurrencyPerfab;
    public int lostCurrencyAmount;
    [SerializeField] private float lostCurrencyX;
    [SerializeField] private float lostCurrencyY;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
            instance = this;

  
    }

    private void Start()
    {

        checkpoints = FindObjectsOfType<Checkpoint>();
        player = PlayerManager.instance.player.transform;
    }
    public void RestratScene()//场景重开函数
    {
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();//获得初始场景
        SceneManager.LoadScene(scene.name);//获取的场景必须通过字符串载入
    }

    public void LoadData(GameData _data)
    {

        
        StartCoroutine(LoadWithDelay(_data));
        
    }

    private void LoadCheckpoints(GameData _data)
    {
        foreach (KeyValuePair<string, bool> pair in _data.checkpoints)
        {

            foreach (Checkpoint checkpoint in checkpoints)
            {

                if (checkpoint.id == pair.Key && pair.Value == true)
                {
                    checkpoint.ActivateCheckpoint();
                    
                }
            }
        }
        
    }

    private void LoadLostCurrency(GameData _data)//产生可以捡到的钱尸体函数
    {
        lostCurrencyAmount = _data.lostCurrencyAmount;
        lostCurrencyX = _data.lostCurrencyX;
        lostCurrencyY = _data.lostCurrencyY;

        if (lostCurrencyAmount > 0)
        {

            GameObject newLostCurrency = Instantiate(lostCurrencyPerfab, new Vector3(lostCurrencyX, lostCurrencyY), Quaternion.identity);
            newLostCurrency.GetComponent<LostCurrencyController>().currency = lostCurrencyAmount;
        }

        lostCurrencyAmount = 0;
    }


    private IEnumerator LoadWithDelay(GameData _data)
    {
        yield return new WaitForSeconds(.1f);

        LoadCheckpoints(_data);
        LoadLostCurrency(_data);
        PlacePlayerAtClosestCheckpoint(_data); 
    }
 
    private void PlacePlayerAtClosestCheckpoint(GameData _data)//传送至最近检查点函数
    {
        if (_data.closestCheckpointId == null)
            return;
        
        closestCheckpointId = _data.closestCheckpointId;

        foreach (Checkpoint checkpoint in checkpoints)
        {
            
            if (closestCheckpointId == checkpoint.id && checkpoint.activationStatus==true)
            {
                
                PlayerManager.instance.player.transform.position = checkpoint.transform.position;
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.lostCurrencyAmount = lostCurrencyAmount;//此地方的钱是player死后stats赋值的
        _data.lostCurrencyX = player.position.x;
        _data.lostCurrencyY = player.position.y;

        if (FindClosestCheckpoint() != null)
            _data.closestCheckpointId = FindClosestCheckpoint().id;//Save后调用
        _data.checkpoints.Clear();
        foreach (Checkpoint checkpoint in checkpoints)
        {
            
            _data.checkpoints.Add(checkpoint.id, checkpoint.activationStatus);
        }

    }

    private Checkpoint FindClosestCheckpoint()//寻找最近检查点的函数
    {
        float closetDistance = Mathf.Infinity;
        Checkpoint closestCheckpoint = null;

        foreach (var checkpoint in checkpoints)//遍历检查点比较距离寻找最近的检查点
        {
            float distanceToCheckpoint = Vector2.Distance(PlayerManager.instance.player.transform.position, checkpoint.transform.position);
            if (distanceToCheckpoint < closetDistance && checkpoint.activationStatus == true)
            {
                closetDistance = distanceToCheckpoint;
                closestCheckpoint = checkpoint;
                closestCheckpoint.activationStatus = true;
            }
        }

        return closestCheckpoint;

    }

    public void PauseGame(bool _pause)
    {
        if (_pause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}