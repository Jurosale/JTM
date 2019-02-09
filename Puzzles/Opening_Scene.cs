//Ignore this entire script. It's not used in the final version of the game

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Opening_Scene : MonoBehaviour {

    public GameObject tree;
    public GameObject shrine;
    public GameObject avatar;
    public GameObject UIicon;
    public GameObject puzzleSpirit;
    public Text title;
    public Image bubble;
    public Text tutorial1;

    private bool openBook = false, fireTask = false, hintTask = false, restoreTask = false, spellChecker = false,
        spoke2spirit = false, restspirit = false, follows = false, stageComplete = false, speakTask = false, followMe = false;

    private float clock = 0f, clock2 = 0f, clock3 = 0f, controls = 0f, jumping = 0f, vines = 0f;
    private float songTimer = 0f;

    //Spirit Stuff
    private string greenSpiritM, greenSpiritT;
    private Vector4 greenSpiritC, viviPortrait;

    //Music
    FMOD.Studio.EventInstance spiritInteraction;
    public string musicPrayer = "event:/BGM/MUS_SpiritPrayer";

    // Use this for initialization
    void Start () {
        GameObject.Find("UISpellManager").GetComponent<UserInterface>().hint = "fire";
        title.color = new Vector4(80f,255f,255f,0f);
        gameObject.GetComponent<SpriteRenderer>().color = new Vector4(255f, 255f, 255f, 255f);
        GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().enabled = false;
		GameObject.Find("SoundEffects").GetComponent<test2>().enabled = false;
        GameObject.Find("SpellBook").transform.localScale = new Vector3(1f,0f,1f);
        GameObject.Find("SpellBook").GetComponent<SpellBook>().enabled = false;
        GameObject.Find("UISpellManager").transform.localScale = new Vector3(1f, 1f, 1f);
        GameObject.Find("UISpellManager").GetComponent<UserInterface>().enabled = false;
        UIicon.transform.localScale = new Vector3(1f, 0f, 1f);
        bubble.enabled = false;
        tutorial1.enabled = false;
        greenSpiritT = "'Cast Follow'";
        greenSpiritM = puzzleSpirit.GetComponent<NPCSpells>().spiritMessage[0];
        greenSpiritC = puzzleSpirit.GetComponentInChildren<SpriteRenderer>().color;
        viviPortrait = new Vector4(GameObject.Find("ViviPortrait").GetComponent<Image>().color.r,
                    GameObject.Find("ViviPortrait").GetComponent<Image>().color.g, GameObject.Find("ViviPortrait").GetComponent<Image>().color.b, 1f);
    }
	
	// Update is called once per frame
    // Very convoluted way of controlling the tutorial. Will be updated soon
	void Update () {

        if (spellChecker)
        {
            GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().discovered.Add("HINT");
            GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().type.Add("hint");
            GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().called.Add(1);
            GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().discovered.Add("RESTORE");
            GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().type.Add("restore");
            GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().called.Add(1);
            GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().discovered.Add("SPEAK");
            GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().type.Add("speak");
            GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().called.Add(1);
            GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().discovered.Add("FOLLOW");
            GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().type.Add("follow");
            GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().called.Add(1);
            GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().discovered.Add("PRAYER");
            GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().type.Add("prayer");
            GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().called.Add(1);
            spellChecker = false;
        }

        else if (openBook)
        {
            Time.timeScale = 0f;
            GameObject.Find("SpellBook").GetComponent<SpellBook>().enabled = true;
            GameObject.Find("SpellBook").transform.localScale = new Vector3(Screen.width/1000, Screen.height / 1000f, 1f);
            GameObject.Find("SpellBook").transform.localScale = new Vector3(1.7f, 1.7f, 1f);
            GameObject.Find("UISpellManager").GetComponent<UserInterface>().enabled = true;
            GameObject.Find("UISpellManager").transform.localScale = new Vector3(1f, 1f, 1f);
            GameObject.Find("Witch character").GetComponent<Movement>().canMove = false;
            GameObject.Find("ViviPortrait").GetComponent<Image>().enabled = true;
            GameObject.Find("DialogueBox").GetComponent<Image>().enabled = true;
            GameObject.Find("Dialogue").GetComponent<Text>().text = "We found a spellbook! Its full of spells we can use. Press SHIFT to open it!";
            GameObject.Find("ViviPortrait").GetComponent<Image>().color = new Vector4(1f,1f,1f,.75f);
            GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color = new Vector4(GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color.r,
                    GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color.g, GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color.b, .75f);
            //if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) {
            //    openBook = false;
            //    speakTask = true;
            //    GameObject.Find("Witch character").GetComponent<Movement>().canMove = true;
            //    GameObject.Find("ViviPortrait").GetComponent<Image>().color = viviPortrait;
            //    GameObject.Find("ViviPortrait").GetComponent<Image>().enabled = false;
            //    GameObject.Find("Dialogue").GetComponent<Text>().text = "";
            //    GameObject.Find("DialogueBox").GetComponent<Image>().enabled = false;
            //    Time.timeScale = 1f;
            //}
        }

        else if (GameObject.Find("NPC_Spirit").GetComponent<NPCSpells>().shrineAnim && speakTask)
        {
            speakTask = false;
            fireTask = true;
            Time.timeScale = 1f;
            GameObject.Find("ViviPortrait").GetComponent<Image>().color = viviPortrait;

        }

        else if (speakTask)
        {
            if (Vector2.Distance(GameObject.Find("Witch character").transform.position,GameObject.Find("NPC_Spirit").transform.position) <= 3f)
            {
                Time.timeScale = 0f;
                GameObject.Find("Witch character").GetComponent<Movement>().canMove = false;
                GameObject.Find("ViviPortrait").GetComponent<Image>().enabled = true;
                GameObject.Find("DialogueBox").GetComponent<Image>().enabled = true;
                GameObject.Find("Dialogue").GetComponent<Text>().text = "It's a spirit. I should talk to him. Say 'Cast Speak' to talk to ANY spirit.";
                GameObject.Find("ViviPortrait").GetComponent<Image>().color = new Vector4(1f, 1f, 1f, .75f);
                GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color = new Vector4(GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color.r,
                        GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color.g, GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color.b, .75f);
            }
        }

        else if (hintTask)
        {
            tutorial1.enabled = false;
            if (tree.transform.localScale.y == 0)
            {
                fireTask = false;
                hintTask = false;
                restoreTask = true;
            }
        }

        else if (clock >= 8f && fireTask)
        {
            Time.timeScale = 0f;
            GameObject.Find("Witch character").GetComponent<Movement>().canMove = false;
            GameObject.Find("ViviPortrait").GetComponent<Image>().enabled = true;
            GameObject.Find("DialogueBox").GetComponent<Image>().enabled = true;
            GameObject.Find("Dialogue").GetComponent<Text>().text = "To cast a spell, say 'Cast [spellname]'.\nEx. you can say 'Cast hint' whenever you get stuck.";
            GameObject.Find("ViviPortrait").GetComponent<Image>().color = new Vector4(1f, 1f, 1f, .75f);
            GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color = new Vector4(GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color.r,
                    GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color.g, GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color.b, .75f);

            if (GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().word != "" && GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().word != "error")
            {
                hintTask = true;
                GameObject.Find("Witch character").GetComponent<Movement>().canMove = true;
                GameObject.Find("ViviPortrait").GetComponent<Image>().color = viviPortrait;
                GameObject.Find("ViviPortrait").GetComponent<Image>().enabled = false;
                GameObject.Find("Dialogue").GetComponent<Text>().text = "";
                GameObject.Find("DialogueBox").GetComponent<Image>().enabled = false;
                GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color = new Vector4(GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color.r,
                    GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color.g, GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color.b, 0f);
                Time.timeScale = 1f;
            }
            if (tree.transform.localScale.y == 0)
            {
                fireTask = false;
                restoreTask = true;
                GameObject.Find("Witch character").GetComponent<Movement>().canMove = true;
                GameObject.Find("ViviPortrait").GetComponent<Image>().color = viviPortrait;
                GameObject.Find("ViviPortrait").GetComponent<Image>().enabled = false;
                GameObject.Find("Dialogue").GetComponent<Text>().text = "";
                GameObject.Find("DialogueBox").GetComponent<Image>().enabled = false;
                GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color = new Vector4(GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color.r,
                    GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color.g, GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color.b, 0f);
                Time.timeScale = 1f;
            }
        }

        else if (fireTask)
        {
            if (GameObject.Find("Witch character").GetComponent<Movement>().canMove) { spoke2spirit = true; }
            //if (GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().word == "speak") { spoke2spirit = true; }
            if (GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().word == "hint")
            {
                hintTask = true;
            }
            if (tree.transform.localScale.y == 0)
            {
                fireTask = false;
                restoreTask = true;
            }
        }

        else if (restoreTask)
        {
            clock3 = 0f;
            clock2 = 0f;
            if (GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().word == "follow" || follows)
            {
                follows = true;
            }

            if (avatar.transform.position.x > 38f)
            {
                Time.timeScale = 0f;
                GameObject.Find("Witch character").GetComponent<Movement>().canMove = false;
                GameObject.Find("ViviPortrait").GetComponent<Image>().enabled = true;
                GameObject.Find("DialogueBox").GetComponent<Image>().enabled = true;
                GameObject.Find("Dialogue").GetComponent<Text>().text = "Looks like that tree meant a lot to that spirit.\nWe should restore it. Say 'Cast restore'.";
                GameObject.Find("ViviPortrait").GetComponent<Image>().color = new Vector4(1f, 1f, 1f, .75f);
                GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color = new Vector4(GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color.r,
                        GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color.g, GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color.b, .75f);
            }

            if (GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().word == "restore")
            {
                restoreTask = false;
                //obstacle.GetComponent<BoxCollider2D>().enabled = false;
                clock = 10f;
                followMe = true;
                clock3 = 0f;
                GameObject.Find("Witch character").GetComponent<Movement>().canMove = true;
                GameObject.Find("ViviPortrait").GetComponent<Image>().color = viviPortrait;
                GameObject.Find("ViviPortrait").GetComponent<Image>().enabled = false;
                GameObject.Find("Dialogue").GetComponent<Text>().text = "";
                GameObject.Find("DialogueBox").GetComponent<Image>().enabled = false;
                GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color = new Vector4(GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color.r,
                    GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color.g, GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color.b, 0f);
                Time.timeScale = 1f;
            }
        }

		else if (gameObject.GetComponent<SpriteRenderer>().color.a == 0f && clock == 0f)
        {
            openBook = true;
        }

        else if (tree.transform.localScale.y == 0 && clock == 10f)
        {
            restoreTask = true;
            //obstacle.GetComponent<BoxCollider2D>().enabled = true;
            followMe = false;
            restspirit = false;
        }
        else if (followMe)
        {
            clock3 += Time.deltaTime;
            if (GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().word == "follow" || follows) {
                follows = true;
                GameObject.Find("Witch character").GetComponent<Movement>().canMove = true;
                GameObject.Find("ViviPortrait").GetComponent<Image>().color = viviPortrait;
                GameObject.Find("ViviPortrait").GetComponent<Image>().enabled = false;
                GameObject.Find("Dialogue").GetComponent<Text>().text = "";
                GameObject.Find("DialogueBox").GetComponent<Image>().enabled = false;
                GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color = new Vector4(GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color.r,
                    GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color.g, GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color.b, 0f);
                followMe = false;
                restspirit = true;
                Time.timeScale = 1f;
            }

            if (clock3 > 7f && !follows)
            {
                Time.timeScale = 0f;
                GameObject.Find("Witch character").GetComponent<Movement>().canMove = false;
                GameObject.Find("ViviPortrait").GetComponent<Image>().enabled = true;
                GameObject.Find("DialogueBox").GetComponent<Image>().enabled = true;
                GameObject.Find("Dialogue").GetComponent<Text>().text = "The old man wants to go to the shrine.\nSay 'Cast Follow' on any GREEN spirit to lead them to the shrines.";
                GameObject.Find("ViviPortrait").GetComponent<Image>().color = new Vector4(1f, 1f, 1f, .75f);
                GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color = new Vector4(GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color.r,
                        GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color.g, GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color.b, .75f);
            }
        }

        else if (restspirit)
        {
            //clock2 += Time.deltaTime;
            //if (restspirit && clock2 >= 15f)
            //{
            //  bubble.enabled = true;
            //tutorial1.enabled = true;
            //tutorial1.text = "I should take the spirit\nto the shrine and\n'Cast Prayer' to free him.";
            //}

            if (follows && Vector2.Distance(avatar.transform.position,shrine.transform.position) <= 2f)
            {
                //prevent witch from moving
                Time.timeScale = 0f;
                GameObject.Find("Witch character").GetComponent<Movement>().canMove = false;
                GameObject.Find("ViviPortrait").GetComponent<Image>().enabled = true;
                GameObject.Find("DialogueBox").GetComponent<Image>().enabled = true;
                GameObject.Find("Dialogue").GetComponent<Text>().text = "As a witch, I can free GREEN spirits with these shrines. Say 'Cast Prayer' to free the spirit.";
                GameObject.Find("ViviPortrait").GetComponent<Image>().color = new Vector4(1f, 1f, 1f, .75f);
                GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color = new Vector4(GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color.r,
                        GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color.g, GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color.b, .75f);
                if (GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().word == "prayer")
                {
                    //Let the music play
                    spiritInteraction = FMODUnity.RuntimeManager.CreateInstance(musicPrayer);
                    spiritInteraction.start();

                    stageComplete = true;
                    restspirit = false;
                    follows = false;
                    //obstacle2.GetComponent<BoxCollider2D>().enabled = false;

                    GameObject.Find("Witch character").GetComponent<Movement>().canMove = true;
                    GameObject.Find("ViviPortrait").GetComponent<Image>().color = viviPortrait;
                    GameObject.Find("ViviPortrait").GetComponent<Image>().enabled = false;
                    GameObject.Find("Dialogue").GetComponent<Text>().text = "";
                    GameObject.Find("DialogueBox").GetComponent<Image>().enabled = false;
                    GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color = new Vector4(GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color.r,
                        GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color.g, GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>().color.b, 0f);
                    Time.timeScale = 1f;
                }
            }
        }

        else if (stageComplete)
        {
            songTimer += Time.deltaTime;

            //start timer
            if (songTimer >= 22.0f)
            {
                //let's players move and stop the song
                GameObject.Find("Witch Character").GetComponent<Movement>().canMove = true;
                spiritInteraction.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            }
            if (puzzleSpirit.activeInHierarchy)
            {
                puzzleSpirit.GetComponentInChildren<TextMesh>().text = "Thank You!";
                puzzleSpirit.transform.position = new Vector3(puzzleSpirit.transform.position.x,
                    puzzleSpirit.transform.position.y + Time.deltaTime, puzzleSpirit.transform.position.z);
            }
            //puzzleSpirit.GetComponentInChildren<SpriteRenderer>().color = new Vector4(puzzleSpirit.GetComponentInChildren<SpriteRenderer>().color.r,
              //  puzzleSpirit.GetComponentInChildren<SpriteRenderer>().color.g, puzzleSpirit.GetComponentInChildren<SpriteRenderer>().color.b,
                //puzzleSpirit.GetComponentInChildren<SpriteRenderer>().color.a - Time.deltaTime);
        }

        if (spoke2spirit && clock <= 8f) { clock += Time.deltaTime; }

        //if (bubble.enabled == false && clock != 0f) { tutorial1.text = ""; }

        if (follows & !stageComplete) {
            puzzleSpirit.transform.position = new Vector3(avatar.transform.position.x -
                (1.5f * GameObject.Find("Witch character").GetComponent<Movement>().facingRight),
                avatar.transform.position.y + 1f, avatar.transform.position.z);
            puzzleSpirit.GetComponentInChildren<TextMesh>().text = "";
        }

        if (tree.transform.localScale.y == 0 && !stageComplete) {
            restoreTask = true;
            followMe = false;
            restspirit = false;
            puzzleSpirit.GetComponent<NPCSpells>().spiritTitle = "Cast Speak";
            //puzzleSpirit.GetComponentInChildren<TextMesh>().text = "Cast Speak";
            puzzleSpirit.GetComponent<NPCSpells>().spiritMessage[0] = "That tree meant a lot to me.\n Please bring it back to me.";
            puzzleSpirit.GetComponentInChildren<SpriteRenderer>().color = new Vector4(1f, .38f, .38f, 1f);
        }
        else {
            puzzleSpirit.GetComponent<NPCSpells>().spiritTitle = greenSpiritT;
            //puzzleSpirit.GetComponentInChildren<TextMesh>().text = greenSpiritT;
            puzzleSpirit.GetComponent<NPCSpells>().spiritMessage[0] = greenSpiritM;
            puzzleSpirit.GetComponentInChildren<SpriteRenderer>().color = greenSpiritC;
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Vector4(0f,0f,0f,0f);
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().enabled = true;
			GameObject.Find("SoundEffects").GetComponent<test2>().enabled = true;
            UIicon.transform.localScale = new Vector3(1.4f, 1.4f, 1f);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            spellChecker = true;
        }
    }
}
