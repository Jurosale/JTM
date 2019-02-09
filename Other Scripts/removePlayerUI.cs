using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class removePlayerUI : MonoBehaviour {

    bool newScene;
    float clock;

	// Use this for initialization
	void Start () {
        newScene = true;	
	}
	
	// Update is called once per frame
	void Update () {
        if (clock < 2f) { clock += Time.deltaTime; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (clock >= 2f && newScene)
        {
            newScene = false;
            GameObject.Find("PlayerUI").transform.localScale = new Vector3(0f, 0f, 0f);
        }
    }
}
