using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3Hints : MonoBehaviour
{

    public GameObject avatar;
    //public GameObject IceShit;
    public GameObject Ice;
    public GameObject RuinedBoat;
    //public GameObject Storm;
    public GameObject Buoy;
    public GameObject Plant1;
    public GameObject Plant2;
    public GameObject Plant3;
    public GameObject Plant4;
    public GameObject Lamp1;
    public GameObject Lamp2;
    public GameObject FixedBoat;
    public GameObject FixedBoat1;
    public GameObject FixedBoat2;
    public GameObject IceBerg;
    public GameObject Food;
    public GameObject shrinkArea;

    UserInterface ui;
    SpeechRecognition01 speech;
    //UIHistory uiH;
    private GameObject[] shrines, roots;

    // Use this for initialization
    void Start()
    {
        ui = GameObject.Find("UISpellManager").GetComponent<UserInterface>();
        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
        //uiH = GameObject.Find("UIH1").GetComponent<UIHistory>();
        shrines = GameObject.FindGameObjectsWithTag("shrine");
        roots = GameObject.FindGameObjectsWithTag("root");
    }

    // Update is called once per frame
    void Update()
    {

        //Keep these incase of future testing purposes

        //Debug.Log("IceShit: " + Vector2.Distance(avatar.transform.position, IceShit.transform.position));
        //Debug.Log("Ice: " + Vector2.Distance(avatar.transform.position, Ice.transform.position));
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
            //if (Vector2.Distance(avatar.transform.position, IceShit.transform.position) < 8f && IceShit.transform.localScale.y != 0) { ui.hint = "fire"; }
            if ((Vector2.Distance(avatar.transform.position, Ice.transform.position) < 12f) && Ice.transform.localScale.y != 0) { ui.hint = "fire"; }
            else if ((Vector2.Distance(avatar.transform.position, RuinedBoat.transform.position) < 12f) && RuinedBoat.activeSelf) { ui.hint = "restore"; }
            //else if (Vector2.Distance(avatar.transform.position, Storm.transform.position) < 6f) { ui.hint = "grow"; }
            else if (Plant1 != null && Vector2.Distance(avatar.transform.position, Plant1.transform.position) < 6f) { ui.hint = "water"; }
            else if (Plant2 != null && Vector2.Distance(avatar.transform.position, Plant2.transform.position) < 6f) { ui.hint = "water"; }
            else if (Plant3 != null && Vector2.Distance(avatar.transform.position, Plant3.transform.position) < 6f) { ui.hint = "water"; }
            else if (Plant4.activeSelf == true && Vector2.Distance(avatar.transform.position, Plant4.transform.position) < 6f) { ui.hint = "water"; }
            else if (Vector2.Distance(avatar.transform.position, IceBerg.transform.position) < 6f) { ui.hint = "earth"; }
            else if (Food != null && Vector2.Distance(avatar.transform.position, Food.transform.position) < 6f) { ui.hint = "grow"; }
            else
            {
                ui.hint = "";
                foreach (GameObject shrine in shrines)
                {
                    if (Vector2.Distance(avatar.transform.position, shrine.transform.position) < 6f)
                    {
                        ui.hint = "astral";
                    }
                }

                if (Vector2.Distance(avatar.transform.position, Lamp1.transform.position) < 6f) { ui.hint = "fire"; }
                else if (Vector2.Distance(avatar.transform.position, Lamp2.transform.position) < 6f) { ui.hint = "fire"; }
                else if (Vector2.Distance(avatar.transform.position, Buoy.transform.position) < 12f) { ui.hint = "wind"; }
                else if (Vector2.Distance(avatar.transform.position, FixedBoat.transform.position) < 6f) { ui.hint = "wind"; }
                else if (Vector2.Distance(avatar.transform.position, FixedBoat1.transform.position) < 6f) { ui.hint = "wind"; }
                else if (Vector2.Distance(avatar.transform.position, FixedBoat2.transform.position) < 6f) { ui.hint = "wind"; }
                else if (Vector2.Distance(avatar.transform.position, shrinkArea.transform.position) < 6f) { ui.hint = "shrink"; }

                foreach (GameObject root in roots)
                {
                    //Debug.Log(Vector2.Distance(avatar.transform.position, root.transform.position));
                    if (Vector2.Distance(avatar.transform.position, root.transform.position) < 6f)
                    {
                        ui.hint = "restore";
                    }
                }
            }
        }
    }
}
