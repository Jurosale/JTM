using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

class UserInterface : MonoBehaviour
{

    string hint;
    bool talking;

    //UI components
    Text castHint;
    UIHistory uiH;

    //UI Speech Images
    Image speechBubble, castFire, castWater, castWind, castEarth, castIce,
        castRestore, castShrink, castGrow, castAstral, danger;

    //Timer
    float timer;
    float clock;

    //helper functions
    bool resetFlag;
    GameObject avatar;
    SpeechRecognition01 speechRec;

    void Start()
    {
        //set hint timer here
        timer = 2f;

        //assigns hint images and then turns them off at the start of game
        speechBubble = GameObject.Find("CastHint").GetComponent<Image>();
        castFire = GameObject.Find("CastFire").GetComponent<Image>();
        castWater = GameObject.Find("CastWater").GetComponent<Image>();
        castIce = GameObject.Find("CastIce").GetComponent<Image>();
        castWind = GameObject.Find("CastWind").GetComponent<Image>();
        castEarth = GameObject.Find("CastEarth").GetComponent<Image>();
        castRestore = GameObject.Find("CastRestore").GetComponent<Image>();
        castShrink = GameObject.Find("CastShrink").GetComponent<Image>();
        castGrow = GameObject.Find("CastGrow").GetComponent<Image>();
        castAstral = GameObject.Find("CastAstral").GetComponent<Image>();
        danger = GameObject.Find("danger").GetComponent<Image>();

        castHint = GameObject.Find("CastHint");
        castHint.text = "Move next to objects\n or obstacles and \nsay 'Cast hint'.";

        hint = "";

        ResetFlags();
        resetFlag = false;

        //assigns other helper functions
        avatar = GameObject.Find("Witch character");
        uiH = GameObject.Find("UIH1").GetComponent<UIHistory>();
        speechRec = GameObject.Find("SpeechRecognition01").GetComponent<SpeechRecognition01>();

        clock = 0f;
        talking = false;
    }

    void Update()
    {
        //updates time
        if (resetFlag)
        {
            clock += Time.unscaledDeltaTime;
        }

        //turns on hint ui and displays the symbol for the respective spell needed
        if (speechRec.word == "hint" || uiH.isHint)
        {
            int extra = 0;

            //checks avatar's size before displaying hint image in its desig8nated spot
            if (avatar.transform.localScale.y == 1f) { extra = 60; }
            else if (avatar.transform.localScale.y == 1.5f) { extra = 180; }
            else if (avatar.transform.localScale.y == 0.5f) { extra = -60; }
            transform.position = new Vector3(Screen.width / 2, Screen.height / 2 + extra, 1f);

            resetFlag = true;
            ResetFlags();
            speechBubble.enabled = true;

            //turns on the specific hinted spell image
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
            ResetFlags();
            talking = false;
            resetFlag = false;
            uiH.isHint = false;
            clock = 0f;
        }
    }

    //turns off hint ui after a few seconds
    public void ResetFlags()
    {
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