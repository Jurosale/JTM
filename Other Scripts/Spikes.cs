//ignore this

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour {

    GameObject ui;

	// Use this for initialization
	void Start () {
        ui = GameObject.Find("UISpellManager");

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay2D (Collider2D other)
    {
        if (other.tag == "Player")
        {
            ui.GetComponent<UserInterface>().resetFlags();
            ui.GetComponent<UserInterface>().speechBubble.enabled = true;
            ui.GetComponent<UserInterface>().danger.enabled = true;
        }
    }
}
