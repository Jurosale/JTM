//tell pat to comment this

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    GameObject UIObject;
    StateUI UI;

    GameObject cameraObject;
    MainCamera mainCamera;

    public GameObject speechRecObj;
    SpeechRecognition01 speech;

	// Use this for initialization
	void Start () {

        UIObject = GameObject.Find("State UI");
        UI = UIObject.GetComponent<StateUI>();

        cameraObject = GameObject.Find("Main Camera");
        mainCamera = cameraObject.GetComponent<MainCamera>();

        speechRecObj = GameObject.Find("SpeechRecognition");
        speech = speechRecObj.GetComponent<SpeechRecognition01>();

	}
	
	// Update is called once per frame
	void Update () {

        if (speech.word == "speak0")
        {

            if (UI.stateOn)
            {

                UI.StartCoroutine("WithdrawSigns", UI.stateSigns);
                UI.StartCoroutine("DeploySigns", UI.speakSignsToShow);

                UI.stateOn = false;
                UI.speakState = true;
            }

        }

        if (speech.word == "cast0")
        {

            if (UI.stateOn)
            {

                UI.StartCoroutine("WithdrawSigns", UI.stateSigns);
                UI.StartCoroutine("DeploySigns", UI.castSignsToShow);
                UI.StartCoroutine("RollOutAllSpells");

                UI.stateOn = false;
                UI.castState = true;
            }

        }

        if (speech.word == "back0")
        {

            if (!UI.stateOn)
            {
                if (UI.castState)
                {
                    UI.StartCoroutine("RollInAllSpells");
                }

                UI.StartCoroutine("WithdrawAllSigns");
                UI.StartCoroutine("DeploySigns", UI.stateSigns);

                UI.speakState = false;
                UI.castState = false;
                UI.stateOn = true;
            }

        }

    }

    public void LoadSpeakInfo(List<GameObject> speakObjects)
    {

        for (int i = 0; i < speakObjects.Count; i++)
        {
            UI.speakSigns[i].GetComponent<ObjectSign>().objectSprite.sprite 
                = speakObjects[i].GetComponentInChildren<SpriteRenderer>().sprite;

            UI.speakSigns[i].GetComponent<ObjectSign>().objectName.text
                = "\"" + speakObjects[i].gameObject.name + "\"";

            UI.speakSignsToShow.Add(UI.speakSigns[i]);
        }

    }

    public void LoadCastInfo(List<GameObject> castObjects)
    {

        for (int i = 0; i < castObjects.Count; i++)
        {
            UI.castSigns[i].GetComponent<ObjectSign>().objectSprite.sprite
                = castObjects[i].GetComponentInChildren<SpriteRenderer>().sprite;

            UI.castSigns[i].GetComponent<ObjectSign>().objectName.text
                = "\"" + castObjects[i].gameObject.name + "\"";

            UI.castSignsToShow.Add(UI.castSigns[i]);
        }

    }

    public void StateOn()
    {

        mainCamera.StartCoroutine("ZoomOut");
        UI.StartCoroutine("DeploySigns", UI.stateSigns);
        UI.stateOn = true;

    }

    public void StateOff()
    {
        UI.StartCoroutine("WithdrawAllSigns");

        UI.speakSignsToShow.Clear();
        UI.castSignsToShow.Clear();

        UI.speakState = false;
        UI.castState = false;
        UI.stateOn = false;

        mainCamera.StartCoroutine("ZoomIn");
    }
}
