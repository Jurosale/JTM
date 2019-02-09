using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour {

    public GameObject fireAnim;
    GameObject vivi;
    SpeechRecognition01 speechRecognition;

	// Use this for initialization
	void Start () {
        fireAnim = GameObject.Find("FireAnim");
        vivi = GameObject.Find("Witch character");
        speechRecognition = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();

    }
	
	// Update is called once per frame
	void Update () {
		if (Vector2.Distance(vivi.transform.position, transform.position) <= 5f 
            && speechRecognition.word == "fire")
        {
            fireAnim.transform.position = transform.position;
            gameObject.transform.GetChild(0).GetComponentInChildren<ParticleSystem>().Play();
            gameObject.transform.GetChild(1).GetComponentInChildren<ParticleSystem>().Play();
            gameObject.transform.GetChild(2).GetComponentInChildren<ParticleSystem>().Play();
        }
    }
}
