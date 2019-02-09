using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followVivi : MonoBehaviour {

    GameObject avatar;

	// Use this for initialization
	void Start () {
        avatar = GameObject.Find("ViviAdvice");
    }
	
	// Update is called once per frame
	void Update () {
        //int extra = 0;
        //if (avatar.transform.localScale.y == 1f) { extra = 60; }
        //else if (avatar.transform.localScale.y == 1.5f) { extra = 180; } //180
        //else if (avatar.transform.localScale.y == 0.5f) { extra = -60; }
        //transform.position = new Vector3(Screen.width / 2, Screen.height / 2 + extra, 1f);
        //transform.position = avatar.transform.position;
    }
}
