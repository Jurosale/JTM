using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaryTutorial : MonoBehaviour {

    GameObject diary;

    // Use this for initialization
    void Start () {
        diary = GameObject.Find("tutorial_diary");
        diary.GetComponent<SpriteRenderer>().enabled = false;
        diary.GetComponentInChildren<MeshRenderer>().enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            diary.GetComponent<SpriteRenderer>().enabled = true;
            diary.GetComponentInChildren<MeshRenderer>().enabled = true;
        }
    }
}
