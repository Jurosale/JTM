using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixBridge : MonoBehaviour {

    SpeechRecognition01 speech;
    public GameObject bridgeBack, bridgeFront;
    public bool isBridgeRestored;

	// Use this for initialization
	void Start () {
        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
        bridgeBack.SetActive(false);
        bridgeFront.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //if player casts restore, fixes the bridge and turns off puzzle manager
        if (collision.tag == "Player")
        {
            if (speech.word == "restore")
            {
                bridgeBack.SetActive(true);
                bridgeFront.SetActive(true);
                isBridgeRestored = true;
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<PuzzleComponent>().conditionMet = true;
            }
        }
    }
}
