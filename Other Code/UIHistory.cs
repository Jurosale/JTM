//this is how all the spell scripts know whether or not the player called a specific spell

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHistory : MonoBehaviour {

    public Sprite right, left, jump, climb, fire, water, wind, ice, grow, shrink, hint, restore, earth, spirit, dummy;
    private Image his, his2, his3, his4, his5;
    Sprite temp;
    SpeechRecognition01 speech;
    Movement mov;
    AvatarSpells ava;
    private bool check;
    public bool isFire, isRestore, isHint, isWind, isWater, isEarth, isIce, isSpell, canSpell, isFollow, isSound;

    // Use this for initialization
    void Start () {
        his = GetComponent<Image>();
        his2 = GameObject.Find("UIH2").GetComponent<Image>();
        his3 = GameObject.Find("UIH3").GetComponent<Image>();
        his4 = GameObject.Find("UIH4").GetComponent<Image>();
        //his5 = GameObject.Find("UIH5").GetComponent<Image>();
        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
        mov = GameObject.Find("Witch character").GetComponent<Movement>();
        ava = GameObject.Find("Witch character").GetComponent<AvatarSpells>();
        check = true;

        his.sprite = dummy;
        his2.sprite = dummy;
        his3.sprite = dummy;
        his4.sprite = dummy;
        //his5.sprite = dummy;

        isFire = false;
        isRestore = false;
        isHint = false;
        isWind = false;
        isWater = false;
        isEarth = false;
        isIce = false;
        isSpell = false;
        canSpell = true;
        isFollow = false;
        isSound = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (speech.word == "" && !check) { check = true; }
		if (speech.word != "" && check && canSpell) {

            //when player says a specific spell, turn on the switch for that spell
            //switches are eventually turned off in other spell scripts
            switch (speech.word)
            {
                case "fire":
                    temp = fire;
                    isFire = true;
                    check = false;
                    isSound = true;
                    break;
                case "water":
                    temp = water;
                    isWater = true;
                    check = false;
                    isSound = true;
                    break;
                case "wind":
                    temp = wind;
                    isWind = true;
                    check = false;
                    isSound = true;
                    break;
                case "ice":
                    temp = ice;
                    isIce = true;
                    check = false;
                    isSound = true;
                    break;
                case "earth":
                    temp = earth;
                    isEarth = true;
                    check = false;
                    isSound = true;
                    break;
                case "hint":
                    temp = hint;
                    isHint = true;
                    check = false;
                    isSound = true;
                    break;
                case "restore":
                    temp = restore;
                    isRestore = true;
                    check = false;
                    isSound = true;
                    break;
                case "grow":
                    temp = grow;
                    ava.isGrow = true;
                    check = false;
                    isSound = true;
                    break;
                case "shrink":
                    temp = shrink;
                    ava.isShrink = true;
                    check = false;
                    isSound = true;
                    break;

                case "prayer":
                    temp = spirit;
                    check = false;
                    break;
                case "pray":
                    temp = spirit;
                    check = false;
                    break;
                case "speak":
                    temp = spirit;
                    check = false;
                    canSpell = false;
                    break;
                case "come":
                    temp = spirit;
                    check = false;
                    isFollow = true;
                    break;
                case "follow":
                    temp = spirit;
                    check = false;
                    isFollow = true;
                    break;

                case "left":
                    temp = left;
                    mov.moveleft = true;
                    check = false;
                    break;
                case "move left":
                    temp = left;
                    mov.moveleft = true;
                    check = false;
                    break;
                case "walk left":
                    temp = left;
                    mov.moveleft = true;
                    check = false;
                    break;
                case "run left":
                    temp = left;
                    mov.moveleft = true;
                    check = false;
                    break;

                case "right":
                    temp = right;
                    mov.moveright = true;
                    check = false;
                    break;
                case "move right":
                    temp = right;
                    mov.moveright = true;
                    check = false;
                    break;
                case "walk right":
                    temp = right;
                    mov.moveright = true;
                    check = false;
                    break;
                case "run right":
                    temp = right;
                    mov.moveright = true;
                    check = false;
                    break;

                case "jump":
                    temp = jump;
                    mov.Jump = true;
                    speech.word = "";
                    check = false;
                    break;
                case "climb":
                    temp = climb;
                    mov.climbUp = true;
                    check = false;
                    break;
                case "turn":
                    isSpell = true;
                    break;
                default:
                    break;
            }

            //updates UI history after saying a spell
            if (!check)
            {
                isSpell = true;
                //his5.sprite = his4.sprite;
                his4.sprite = his3.sprite;
                his3.sprite = his2.sprite;
                his2.sprite = his.sprite;
                his.sprite = temp;
            }
        }
	}
}
