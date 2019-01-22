using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreSpell : MonoBehaviour {

    public GameObject whitePic, restoreBird, birdParticles;
    private float clock, timer;

    SpeechRecognition01 speech;

	// Use this for initialization
	void Start () {
        clock = 0f;
        timer = 0f;
        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();

    }
	
	// Update is called once per frame
	void Update () {
        if (whitePic.activeInHierarchy == true)
        {
            clock += Time.deltaTime;
            timer += Time.deltaTime;
        }
        
        if (speech.word == "restore")
        {
            whitePic.SetActive(true);
            restoreBird.SetActive(true);
        }

        if (timer >= 0.1f)
        {
            GameObject tmp = Instantiate(birdParticles, restoreBird.transform.position, restoreBird.transform.rotation);
            Destroy(tmp, 2f);
            timer = 0;
        }
        if (clock >= 1.25f)
        {
            restoreBird.SetActive(false);
            whitePic.SetActive(false);
            clock = 0;
        }

    }
}
