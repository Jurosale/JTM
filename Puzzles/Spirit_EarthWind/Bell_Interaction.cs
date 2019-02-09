using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bell_Interaction : MonoBehaviour {

    public bool inBellTrigger;
    public bool ringBell;
    public bool atShrine;
    public bool puzzleComplete;

    public float maxAngle = 100.0f;
    public float minAngle = 45.0f;
    public float rotateSpeed = 1000.0f;
    public float changeRotation;

    private float clock;
    public float timer = 4.0f;

    public GameObject nearestShrine;
    public GameObject bell, puzzleSpirit;
    public GameObject bellAni;

    public Sprite happyImage;
    public RuntimeAnimatorController happySpirit;
    SpeechRecognition01 speech;
    //public GameObject shrinePar;        //Shrine Particle System

    //Music
    //FMOD.Studio.EventInstance bellInteraction;
    //public string music = "event:/MUS_ChurchBell";

    // Use this for initialization
    void Start () {
        inBellTrigger = false;
        ringBell = false;
        bellAni.SetActive(false);
        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //bellInteraction = FMODUnity.RuntimeManager.CreateInstance(music);
        if (speech.word == "wind" && inBellTrigger == true)
        {
            ringBell = true;
            puzzleSpirit.GetComponent<Animator>().runtimeAnimatorController = happySpirit;
            puzzleSpirit.GetComponent<SpriteRenderer>().sprite = happyImage;
            puzzleSpirit.GetComponent<Spirit_Interaction>().enabled = false;
            puzzleSpirit.GetComponent<NPCSpells>().enabled = true;
            puzzleSpirit.GetComponent<NPCSpells>().isPuzzle = true;
        }

        //if the bell is ringing
        if (ringBell == true)
        {
            clock += Time.deltaTime;
            bell.GetComponent<BoxCollider2D>().enabled = false;
            //shrinePar.SetActive(true);              //Active Shrine Particle System when the Bell is ringing
            if (clock <= timer)
            {
                //sets the chec boolean to true
                puzzleComplete = true;

                //plays the bell animation
                bellAni.SetActive(true);

                //turns off the still bell
                bell.SetActive(false);

            }

            //once a certain time has passed
            if(clock > timer)
            {
                //turn off animation
                bellAni.SetActive(false);
                //turn on still
                bell.SetActive(true);
                //reset the boolean
                ringBell = false;
                //reset cloc
                clock = 0;
            }
        }


    }

    void Update()
    {
        //Checks if the puzzle is completed
        if (puzzleComplete == true && atShrine == false)
        {
            if (Vector2.Distance(puzzleSpirit.transform.position, nearestShrine.transform.position) < 5f &&
                Vector2.Distance(puzzleSpirit.transform.position, nearestShrine.transform.position) > 3f &&
                puzzleSpirit.transform.position.x > nearestShrine.transform.position.x) { atShrine = true; }
            puzzleSpirit.transform.position = Vector3.Lerp(puzzleSpirit.transform.position,
                nearestShrine.transform.position + new Vector3(2, 0, 0), Time.deltaTime * 0.3f);
            //atShrine = true;
            // houseGone = false;
        }
    }

        //Checs if player has entered
        private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inBellTrigger = true;
        }
    }

    //cehc if you left the trigger
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            inBellTrigger = false;
        }
    }
}
