using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTutorial : MonoBehaviour {

    SpeechRecognition01 speech;
    Movement move;
    Vector4 initColor, newColor;
    int stage;
    float timer;

    // Use this for initialization
    void Start () {
        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
        move = GameObject.Find("Witch character").GetComponent<Movement>();
        stage = 1;
        GetComponent<TextMesh>().text = "\n        Say \"Vivi\"";
        initColor = GetComponent<TextMesh>().color;
        newColor = new Vector4(.25f,.25f,.75f,1f);
    }
	
	// Update is called once per frame
	void Update () {

        //once player says "vivi", this is what the tutorial will say...
		if (stage == 1 && speech.moveOn) {
            GetComponent<TextMesh>().text = "Vivi's listening.\nSay \"left\", \"right\"\nor \"turn\" to move";
            stage++;
        }

        //this is what the tutorial will initially say...
        if (stage == 1 && !speech.moveOn)
        {
            GetComponent<TextMesh>().text = "\n        Say \"Vivi\"";
        }

        //grabs the players attention by changing color and will change text depending on situation
        if (stage == 2)
        {
            timer += Time.deltaTime;
            if (timer % 2 < 1f) { GetComponent<TextMesh>().color = initColor; }
            else { GetComponent<TextMesh>().color = newColor; }
            if (move.moveleft || move.moveright) {
                stage++;
            }
            else if (speech.moveOn == false && speech.word == "") { stage--; }
        }

        //once player has moved for the first time, text will say...
        if (stage > 2)
        {
            GetComponent<TextMesh>().text = "    Say \"Vivi...Left\"\n    or \"Vivi...right\"\nto move Vivi";
            GetComponent<TextMesh>().color = initColor;
            stage++;
        }
	}
}
