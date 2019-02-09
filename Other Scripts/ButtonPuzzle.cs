using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPuzzle : MonoBehaviour {

    GameObject speechRecObj, avatar;
    SpeechRecognition01 speech;
   
    public GameObject Growblock;
    public GameObject button;
    public GameObject bridge;
    public GameObject gear1;
    public GameObject gear2;
    float jiggle;

    bool buttonDownStarted;

    FMOD.Studio.EventInstance buttonDownEvent;
    public string buttonDownPath = "event:/Other SFX/ButtonPress";

    FMOD.Studio.EventInstance bridgeDownEvent;
    public string bridgeDownPath = "event:/Other SFX/BridgeDropping";

    // Use this for initialization
    void Start () {

        speechRecObj = GameObject.Find("SpeechRecognition");
        avatar = GameObject.Find("Witch character");
        speech = speechRecObj.GetComponent<SpeechRecognition01>();

        jiggle = 0;

        buttonDownEvent = FMODUnity.RuntimeManager.CreateInstance(buttonDownPath);
        bridgeDownEvent = FMODUnity.RuntimeManager.CreateInstance(bridgeDownPath);

        buttonDownStarted = false;
		
	}
	
	// Update is called once per frame
	void Update () {
        gear1.transform.Rotate(0, 0, 80 * Time.deltaTime);
        gear2.transform.Rotate(0, 0, -80 * Time.deltaTime);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //if (avatar.transform.localScale.y > 1f)
            //{
                print("pressing");
            if (!buttonDownStarted)
            {
                StartCoroutine("ButtonPress");
                Growblock.SetActive(false);
                buttonDownStarted = true;
            }
            //}
        }
    }

    IEnumerator ButtonPress()
    {
        buttonDownEvent.start();

        //while (button.transform.position.y >= -1.85)
        //{
        //    print(button.transform.position.y);
        //    jiggle = Random.Range(-.005f, .005f);
        //    button.transform.position = new Vector3(button.transform.position.x + jiggle, button.transform.position.y - .002f, 0);
        //    yield return null;
        //}
        int buttonCounter = 0;
        while (buttonCounter <= 8)
        {
            // button.GetComponent<SpriteRenderer>().sortingOrder = 1;
            print(button.transform.position.y);
            jiggle = Random.Range(-.005f, .005f);
            button.transform.position = new Vector3(button.transform.position.x + jiggle, button.transform.position.y - 0.01f, 0);
            buttonCounter++;
            yield return null;
        }

        StartCoroutine("LowerBridge");


    }

    IEnumerator LowerBridge()
    {
        buttonDownEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        bridgeDownEvent.start();

        while (bridge.transform.rotation.z <= 0)
        {
            bridge.transform.Rotate(new Vector3(0, 0, 1f));
            yield return null;
        }

        bridgeDownEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}
