using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_NextScene : MonoBehaviour
{
    public Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));

    }
    IEnumerator LoadLevel(int levelIndex)
    {
        anim.SetTrigger("Start");

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(levelIndex);
    }
}
