using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Hints : MonoBehaviour {

    public GameObject avatar;
    public GameObject tree;
    public GameObject log;
    public GameObject water1;
    public GameObject bridge;
    public GameObject growCheck;
    public GameObject water2;
    public GameObject ice2;
    public GameObject water3;
    public GameObject debris;
    public GameObject house;
    public GameObject water4;
    public GameObject hedge;
    public GameObject boiler;
    public bool ShitsOnFire;
    public bool ShitsOnWater;
    public GameObject earthCheck; //For Puzzle 4
    public GameObject earthCheck2;  //For Puzzle 4
    public GameObject log2; //For Puzzle 4
    public GameObject bell; //For Puzzle 4

    UserInterface ui;
    SpeechRecognition01 speech;
    private GameObject[] shrines;

    // Use this for initialization
    void Start () {
        ui = GameObject.Find("UISpellManager").GetComponent<UserInterface>();
        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
        shrines = GameObject.FindGameObjectsWithTag("shrine");
        ShitsOnFire = false;
    }
	
	// Update is called once per frame
	void Update () {

        //Keep these incase of future testing purposes

        //Debug.Log("tree: " + Vector2.Distance(avatar.transform.position, tree.transform.position));
        //Debug.Log("log: " + Vector2.Distance(avatar.transform.position, log.transform.position));
        //Debug.Log("water1: " + Vector2.Distance(avatar.transform.position, water1.transform.position));
        //Debug.Log("grow: " + Vector2.Distance(avatar.transform.position, growCheck.transform.position));
        //Debug.Log("water2: " + Vector2.Distance(avatar.transform.position, water2.transform.position));
        //Debug.Log("water3: " + Vector2.Distance(avatar.transform.position, water3.transform.position));
        //Debug.Log("block: " + Vector2.Distance(avatar.transform.position, debris.transform.position));
        //Debug.Log("house: " + Vector2.Distance(avatar.transform.position, house.transform.position));
        //Debug.Log("water4: " + Vector2.Distance(avatar.transform.position, water4.transform.position));
        //Debug.Log("earthCheck: " + Vector2.Distance(avatar.transform.position, earthCheck.transform.position));

        //basically, if the player is within distance of a specific in game object, it's respective hint will be displayed to the player
        if (speech.word == "hint")
        {
            if (Vector2.Distance(avatar.transform.position, tree.transform.position) < 6.5f && tree.transform.localScale.y != 0) { ui.hint = "fire"; }
            else if (Vector2.Distance(avatar.transform.position, water1.transform.position) < 12f && bridge.GetComponent<SpriteRenderer>().enabled)
            { ui.hint = "restore"; }
            else if (Vector2.Distance(avatar.transform.position, water1.transform.position) < 12f) { ui.hint = "water"; }
            else if (Vector2.Distance(avatar.transform.position, hedge.transform.position) < 12f && ShitsOnFire) { ui.hint = "wind"; }
            else if (Vector2.Distance(avatar.transform.position, hedge.transform.position) < 12f) { ui.hint = "fire"; }
            else if (Vector2.Distance(avatar.transform.position, growCheck.transform.position) < 5f) { ui.hint = "grow"; }
            else if (Vector2.Distance(avatar.transform.position, earthCheck2.transform.position) < 5.5f) { ui.hint = "earth"; }
            else if (Vector2.Distance(avatar.transform.position, log.transform.position) < 3f) { ui.hint = "wind"; }
            else if (Vector2.Distance(avatar.transform.position, boiler.transform.position) < 12f && ShitsOnWater) { ui.hint = "fire"; }
            else if (Vector2.Distance(avatar.transform.position, boiler.transform.position) < 12f) { ui.hint = "water"; }
            else if (Vector2.Distance(avatar.transform.position, water2.transform.position) < 11f && log.transform.localScale.y == 0)
            { ui.hint = "restore"; }
            else if (Vector2.Distance(avatar.transform.position, water2.transform.position) < 11f && ice2.transform.localScale.y == 0)
            { ui.hint = "ice"; }
            else if (Vector2.Distance(avatar.transform.position, water2.transform.position) < 11f) { ui.hint = "water"; }
            else if (Vector2.Distance(avatar.transform.position, water3.transform.position) < 3.2f) { ui.hint = "water"; }
            else if (Vector2.Distance(avatar.transform.position, debris.transform.position) < 15f) { ui.hint = "shrink"; }
            else if (Vector2.Distance(avatar.transform.position, house.transform.position) < 14f && house.transform.localScale.y != 0) { ui.hint = "fire"; }
            else if (Vector2.Distance(avatar.transform.position, water4.transform.position) < 17f && house.transform.localScale.y == 0) { ui.hint = "water"; }
            else if (Vector2.Distance(avatar.transform.position, earthCheck.transform.position) < 5.5f) { ui.hint = "earth"; }
            else if (Vector2.Distance(avatar.transform.position, earthCheck2.transform.position) < 4.5f && log2.transform.localScale.y == 0)
            { ui.hint = "restore"; }
            else if (Vector2.Distance(avatar.transform.position, log2.transform.position) < 3f) { ui.hint = "wind"; }
            else if (Vector2.Distance(avatar.transform.position, bell.transform.position) < 8f) { ui.hint = "wind"; }
            else { ui.hint = ""; }
            foreach (GameObject shrine in shrines)
            {
                if (Vector2.Distance(avatar.transform.position, shrine.transform.position) < 6f)
                {
                    ui.hint = "astral";
                }
            }
        }
    }
}
