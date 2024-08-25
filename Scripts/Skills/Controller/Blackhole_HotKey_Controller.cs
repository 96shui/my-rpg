using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Blackhole_HotKey_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode myHotKey;
    private TextMeshProUGUI myText;
    private Transform myEnemy;
    private Blackhole_Skill_controller blackhole;

    public void SetupHotKey(KeyCode _myNewHotKey,Transform _myEnemy,Blackhole_Skill_controller _myBlackhole)
    {
        sr= GetComponent<SpriteRenderer>();
        myText=GetComponentInChildren<TextMeshProUGUI>();

        myEnemy = _myEnemy;
        blackhole = _myBlackhole;

        myHotKey = _myNewHotKey;
        myText.text = _myNewHotKey.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyUp(myHotKey))
        {
            blackhole.AddEnemyToList(myEnemy);

            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}
