using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class UserInterface : MonoBehaviour {

    //UI Speech Images
    public GameObject speechRec;
    public Image speechBubble;
    public Image castFire;
    public Image castWater;
    public Image castWind;
    public Image castEarth;
    public Image castIce;
    public Image castRestore;
    public Image castShrink;
    public Image castGrow;
    public Image castAstral;
    public Text castHint;
    public Image danger;
    public string hint;
    public bool talking;

    //Timer
    public float timer;
    private float clock, clock2;
    public bool resetFlag;
    GameObject avatar;
    UIHistory uiH;

    // Use this for initialization
    void Start () {
        //Make sure it turns off in beginning of game
        castHint.text = "Move next to objects\n or obstacles and \nsay 'Cast hint'.";
        speechBubble.enabled = false;
        castFire.enabled = false;
        castWater.enabled = false;
        castIce.enabled = false;
        castWind.enabled = false;
        castEarth.enabled = false;
        castRestore.enabled = false;
        castShrink.enabled = false;
        castGrow.enabled = false;
        castAstral.enabled = false;
        castHint.enabled = false;
        danger.enabled = false;
        resetFlag = false;
        avatar = GameObject.Find("Witch character");
        uiH = GameObject.Find("UIH1").GetComponent<UIHistory>();
        clock = 0f;
        talking = false;
    }
	
	// Update is called once per frame
	void Update () {
        //updates time
        if (resetFlag) {
            clock += Time.unscaledDeltaTime;
            //clock2 += Time.unscaledDeltaTime;
        }

        /*
        //Turns on fire
        if (speechRec.GetComponent<SpeechRecognition01>().word == "fire")
        {
            resetFlags();
            speechBubble.enabled = true;
            castFire.enabled = true;
        }
        //Turns on water
        else if(speechRec.GetComponent<SpeechRecognition01>().word == "water")
        {
            resetFlags();
            speechBubble.enabled = true;
            castWater.enabled = true;
        }
        //Turns on ice
        else if (speechRec.GetComponent<SpeechRecognition01>().word == "ice")
        {
            resetFlags();
            speechBubble.enabled = true;
            castIce.enabled = true;
        }
        //Turns on wind
        else if(speechRec.GetComponent<SpeechRecognition01>().word == "wind")
        {
            resetFlags();
            speechBubble.enabled = true;
            castWind.enabled = true;
        }
        //Turns on earth
        else if(speechRec.GetComponent<SpeechRecognition01>().word == "earth")
        {
            resetFlags();
            speechBubble.enabled = true;
            castEarth.enabled = true;
        }
        else if (speechRec.GetComponent<SpeechRecognition01>().word == "restore")
        {
            resetFlags();
            speechBubble.enabled = true;
            castRestore.enabled = true;

        }
        else if (speechRec.GetComponent<SpeechRecognition01>().word == "shrink")
        {
            resetFlags();
            speechBubble.enabled = true;
            castShrink.enabled = true;

        }
        else if (speechRec.GetComponent<SpeechRecognition01>().word == "grow")
        {
            resetFlags();
            speechBubble.enabled = true;
            castGrow.enabled = true;
        }
        else if (speechRec.GetComponent<SpeechRecognition01>().word == "astral")
        {
            
            resetFlags();
            speechBubble.enabled = true;
            castAstral.enabled = true;
        }
        

        else*/
        
        //turns on hint ui and displays the symbol for the respective spell needed
        if (speechRec.GetComponent<SpeechRecognition01>().word == "hint" || uiH.isHint)
        {
            //transform.localPosition = new Vector3(avatar.GetComponent<Collider2D>().bounds.center.x + 15f, avatar.GetComponent<Collider2D>().bounds.min.y, 1f);
            int extra = 0;
            if (avatar.transform.localScale.y == 1f) { extra = 60; }
            else if (avatar.transform.localScale.y == 1.5f) { extra = 180; }
            else if (avatar.transform.localScale.y == 0.5f) { extra = -60; }
            transform.position = new Vector3(Screen.width / 2, Screen.height / 2 + extra, 1f);
            resetFlag = true;
            resetFlags();
            speechBubble.enabled = true;
            switch (hint)
            {
                case "fire":
                    castFire.enabled = true;
                    break;
                case "water":
                    castWater.enabled = true;
                    break;
                case "ice":
                    castIce.enabled = true;
                    break;
                case "earth":
                    castEarth.enabled = true;
                    break;
                case "wind":
                    castWind.enabled = true;
                    break;
                case "restore":
                    castRestore.enabled = true;
                    break;
                case "shrink":
                    castShrink.enabled = true;
                    break;
                case "grow":
                    castGrow.enabled = true;
                    break;
                case "astral":
                    castAstral.enabled = true;
                    break;
                case "danger":
                    danger.enabled = true;
                    break;
                default:
                    castHint.enabled = true;
                    castHint.text = "Move next to objects\n or obstacles and \nsay 'Cast hint'.";
                    break;
            }
        }
        //resets clock and turns off speech
        if ((clock >= timer || talking) && uiH.isHint)
        {
            resetFlags();
            talking = false;
            resetFlag = false;
            uiH.isHint = false;
            clock = 0f;
        }

    }

    //turns off hint ui after a few seconds
    public void resetFlags()
    {
        //clock = 0f;
        speechBubble.enabled = false;
        castFire.enabled = false;
        castWater.enabled = false;
        castIce.enabled = false;
        castWind.enabled = false;
        castEarth.enabled = false;
        castRestore.enabled = false;
        castShrink.enabled = false;
        castGrow.enabled = false;
        castAstral.enabled = false;
        castHint.enabled = false;
        danger.enabled = false;
    }


}
