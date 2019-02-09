using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* ********************************************
 *      Introductory Cutscene
 *          This script manages it
*********************************************** */

public class CS_Intro : MonoBehaviour
{
    private Animator anim;
    bool introPlaying;
    float timer;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = 0;
        introPlaying = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (introPlaying == true)
        {
            Debug.Log("introPlaying: " + introPlaying);
            anim.speed = 1f;
            anim.Play("cutscene01", 0, 0);
            introPlaying = false;
        }

        //timer += Time.deltaTime;
        //if (anim.GetCurrentAnimatorStateInfo(0).IsName("cutscene01"))
        //{
          //  Debug.Log("To next scene");
        //}

    }

    public void StartGame()
    {
        GameObject.Find("Music Manager").GetComponent<MusicManager>().StartCoroutine("KillMusic");
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }
}