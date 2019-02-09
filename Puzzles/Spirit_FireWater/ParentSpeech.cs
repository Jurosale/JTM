using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentSpeech : MonoBehaviour {

    SpeechRecognition01 speech;
    NPCSpells child;

	// Use this for initialization
	void Start () {
        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
        child = GameObject.Find("Child_Spirit").GetComponent<NPCSpells>();

    }
	
	// Update is called once per frame
	void Update () {

	}

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && transform.position.x >= -30f)
        {
            //if (speech.word != "prayer0")
            //{
            //    child.enabled = false;
            //}
            //else { child.enabled = true; }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" && transform.position.x >= -30f)
        {
           //child.enabled = true;
        }
    }
}
