//this is how all the spell scripts know whether or not the player called a specific spell

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class UIHistory : MonoBehaviour
{

    Sprite right, left, jump, climb, fire, water, wind, ice, earth, grow, shrink, hint, restore, spirit, temp;
    Image his, his2, his3, his4;

    SpeechRecognition01 speech;
    Movement mov;
    AvatarSpells ava;

    bool check, canSpell;

    void Start()
    {
        //assigns the UI components that will display the spell history
        his = GetComponent<Image>();
        his2 = GameObject.Find("UIH2").GetComponent<Image>();
        his3 = GameObject.Find("UIH3").GetComponent<Image>();
        his4 = GameObject.Find("UIH4").GetComponent<Image>();

        //assigns UI components for each type of player action
        right = GameObject.Find("rightUI").GetComponent<SpriteRenderer>();
        left = GameObject.Find("leftUI").GetComponent<SpriteRenderer>();
        jump = GameObject.Find("jumpUI").GetComponent<SpriteRenderer>();
        climb = GameObject.Find("climbUI").GetComponent<SpriteRenderer>();
        fire = GameObject.Find("fireUI").GetComponent<SpriteRenderer>();
        water = GameObject.Find("waterUI").GetComponent<SpriteRenderer>();
        wind = GameObject.Find("windUI").GetComponent<SpriteRenderer>();
        ice = GameObject.Find("iceUI").GetComponent<SpriteRenderer>();
        earth = GameObject.Find("earthUI").GetComponent<SpriteRenderer>();
        grow = GameObject.Find("growUI").GetComponent<SpriteRenderer>();
        shrink = GameObject.Find("shrinkUI").GetComponent<SpriteRenderer>();
        hint = GameObject.Find("hintUI").GetComponent<SpriteRenderer>();
        restore = GameObject.Find("restoreUI").GetComponent<SpriteRenderer>();
        spirit = GameObject.Find("spiritUI").GetComponent<SpriteRenderer>();

        //helper functions
        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
        mov = GameObject.Find("Witch character").GetComponent<Movement>();
        ava = GameObject.Find("Witch character").GetComponent<AvatarSpells>();

        check = true;
        canSpell = true;

        //initializes an empty UI history box
        Sprite dummy = GameObject.Find("emptyUI").GetComponent<SpriteRenderer>();
        his.sprite = dummy;
        his2.sprite = dummy;
        his3.sprite = dummy;
        his4.sprite = dummy;

    }

    void Update()
    {
        if (speech.word == "" && !check)
            check = true;

        if (speech.word != "" && check && canSpell)
        {
            //when player says a specific spell, turn on the switch for that spell
            //switches are eventually turned off in other spell scripts
            switch (speech.word)
            {
                case "fire":
                    temp = fire;
                    check = false;
                    break;

                case "water":
                    temp = water;
                    check = false;
                    break;

                case "wind":
                    temp = wind;
                    check = false;
                    break;

                case "ice":
                    temp = ice;
                    check = false;
                    break;

                case "earth":
                    temp = earth;
                    check = false;
                    break;

                case "hint":
                    temp = hint;
                    check = false;
                    break;

                case "restore":
                    temp = restore;
                    check = false;
                    break;

                case "grow":
                    temp = grow;
                    ava.isGrow = true;
                    check = false;
                    break;

                case "shrink":
                    temp = shrink;
                    ava.isShrink = true;
                    check = false;
                    break;

                case "prayer":
                case "pray":
                case "follow":
                case "come":
                    temp = spirit;
                    check = false;
                    break;

                case "speak":
                    temp = spirit;
                    check = false;
                    canSpell = false;
                    break;

                case "left":
                case "move left":
                case "walk left":
                case "run left":
                    temp = left;
                    mov.moveleft = true;
                    check = false;
                    break;

                case "right":
                case "move right":
                case "walk right":
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

                default:
                    break;
            }

            //updates UI history after saying a spell
            if (!check)
            {
                his4.sprite = his3.sprite;
                his3.sprite = his2.sprite;
                his2.sprite = his.sprite;
                his.sprite = temp;
            }
        }
    }
}