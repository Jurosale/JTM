//Most code here is my own contribution. Code that isn't mine will be stated in comment
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Spirit_Interaction : MonoBehaviour {


    /*----------------------------*/
    /*This script is for puzzle spirits. 
     * Displats cutscene, music, and when you meet them at the shrine they can be prayed.*/
    /*--------------------------*/

    public bool canTalk;
    public bool playScene;
    public bool prayerAni;
    private string speech;

    public bool Pray;

    //For the cutscenes
    public GameObject[] cutscene;

    //Let the prayer animation light play at the shrine
    public GameObject prayerAnimation;

    public string surprisedSound = "event:/Voice Overs/VO_Vivi_Surprise01";
    public float timer;
    private int counter;

    //FMOD related code contributed by another teammate
    //Reverb for the spirit interaction song
    FMOD.Studio.EventInstance reverbInteraction;
    public string snapshotSpirit = "snapshot:/SpiritInteraction";

    //Spirit Interaction music
    FMOD.Studio.EventInstance spiritInteraction;
    public string spiritSong = "event:/BGM/MUS_SpiritInteraction";

    SpeechRecognition01 SPEECH;
    UIHistory uiH;
    GameObject vivi, ui, mmc, sb, borders, hist;

    Text PorText_Next, PorText_Back;
    Image Por_next, Por_back;
    SpellBook book;

    Vector3 initHis, initUIH;

    // Use this for initialization
    void Start () {
        canTalk = false;
        playScene = false;
        //load up the cutscnes and turn off activity
        for (int i = 0; i < cutscene.Length; i++)
        {
            cutscene[i].SetActive(false);
        }
        prayerAnimation.SetActive(false);
        timer = 0f;
        Pray = false;
        SPEECH = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
        vivi = GameObject.Find("Witch character");
        ui = GameObject.Find("UISpellManager");
        mmc = GameObject.Find("MainMenuColor");
        sb = GameObject.Find("SpellBook");
        book = sb.GetComponent<SpellBook>();
        borders = GameObject.Find("comicBorder");
        borders.GetComponent<SpriteRenderer>().enabled = false;

        Por_next = GameObject.Find("Por_Next2").GetComponent<Image>();
        Por_back = GameObject.Find("Por_Back2").GetComponent<Image>();
        PorText_Next = GameObject.Find("PorText_Next2").GetComponent<Text>();
        PorText_Back = GameObject.Find("PorText_Back2").GetComponent<Text>();
        uiH = GameObject.Find("UIH1").GetComponent<UIHistory>();
        hist = GameObject.Find("HistoryBubble");
        initUIH = GameObject.Find("UIH1").transform.localScale;
        initHis = GameObject.Find("HistoryBubble").transform.localScale;
    }
	
	// Update is called once per frame
	void Update () {

        //For the Spirit Puzzle 
        if (SPEECH.word == "speak")
        {
            //If you converse with a spirit
            if (canTalk == true && playScene == false)
            {
                //Prevent Vivi from moving
                vivi.GetComponent<Movement>().canMove = false;

                //Prevent the bacground from moving
                vivi.GetComponent<Movement>().bgMove = false;

                //FMOD related code contributed by another teammate
                //Play music
                spiritInteraction = FMODUnity.RuntimeManager.CreateInstance(spiritSong);
                spiritInteraction.start();

                //Turn on Reverb
                reverbInteraction = FMODUnity.RuntimeManager.CreateInstance(snapshotSpirit);
                reverbInteraction.start();

                //Load up the cutscenes
                for (int i = 0; i < cutscene.Length; i++)
                {
                    cutscene[i].SetActive(true);
                    cutscene[i].transform.position = new Vector3(vivi.transform.position.x,
                        vivi.GetComponent<Collider2D>().bounds.min.y + 2f, vivi.transform.position.z);
                }
                borders.GetComponent<SpriteRenderer>().enabled = true;
                borders.transform.position = new Vector3(vivi.transform.position.x,
                        vivi.GetComponent<Collider2D>().bounds.min.y + 2f, vivi.transform.position.z);

                //play the scene
                playScene = true;
                Por_next.enabled = true;
                Por_back.enabled = true;
                PorText_Next.enabled = true;
                PorText_Back.enabled = true;
                uiH.isSpell = true;
                SPEECH.castOn = false;
                PorText_Back.text = "\"Back\"";
                PorText_Next.text = "\"Next\"";

                GameObject.Find("Points").GetComponent<Text>().enabled = false;
                GameObject.Find("Title Scene").GetComponent<Text>().enabled = false;
                GameObject.Find("UIIconMenu").GetComponent<Image>().enabled = false;
                GameObject.Find("UIIcon").GetComponent<Image>().enabled = false;
                GameObject.Find("ViviInput").GetComponent<Image>().enabled = false;
                GameObject.Find("ViviAdvice").GetComponent<Image>().enabled = false;
                GameObject.Find("SayWord").GetComponent<Text>().enabled = false;
                GameObject.Find("UIH1").GetComponent<RectTransform>().localScale = new Vector3(0f,0f,0f);
                GameObject.Find("HistoryBubble").GetComponent<RectTransform>().localScale = new Vector3(0f,0f,0f);

                SPEECH.speaking = true;
                SPEECH.word = "";
                book.TurnOffDiscoverAnimation();

                ui.transform.localScale = new Vector3(1f, 0f, 1f);
                counter = 0;
            }
        }

        //Plays the scene
        if (playScene == true)
        {
            Time.timeScale = 0f;
            mmc.GetComponent<SpriteRenderer>().color = new Vector4(mmc.GetComponent<SpriteRenderer>().color.r,
                   mmc.GetComponent<SpriteRenderer>().color.g, mmc.GetComponent<SpriteRenderer>().color.b, .75f);

            //Left arrow & A key scrools comic book backwards; or saying the back keyword
            if (!sb.GetComponent<SpellBook>().isOpen && SPEECH.word == "back") {
                SPEECH.word = "";
                counter--;
                if (counter >= 0)
                {
                    for (int i = 0; i < counter; i++)
                    {
                        cutscene[i].SetActive(false);
                    }
                    for (int i = counter; i < cutscene.Length; i++)
                    {
                        cutscene[i].SetActive(true);
                    }
                }
            }

            //Right arrow & D key scrools comic book forwards; or saying anything but the back keyword
            else if (!sb.GetComponent<SpellBook>().isOpen && SPEECH.word != "")
            {
                SPEECH.word = "";
                counter++;
                for (int i = 0; i < counter; i++)
                {
                    cutscene[i].SetActive(false);
                }
                for (int i = counter; i < cutscene.Length; i++)
                {
                    cutscene[i].SetActive(true);
                }
            }

            //Once player scrolls through whole cutscene
            if (counter >= cutscene.Length || counter < 0)
            {
                //Let's the player move again
                vivi.GetComponent<Movement>().canMove = true;

                //Let the bacground move again
                vivi.GetComponent<Movement>().bgMove = true;

                //stop reverb
                reverbInteraction.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

                //stop spirit interaction song
                spiritInteraction.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

                for (int i = 0; i < cutscene.Length; i++)
                {
                    cutscene[i].SetActive(false);
                }
                borders.GetComponent<SpriteRenderer>().enabled = false;
                Por_next.enabled = false;
                Por_back.enabled = false;
                PorText_Next.enabled = false;
                PorText_Back.enabled = false;
                GameObject.Find("Points").GetComponent<Text>().enabled = true;
                GameObject.Find("Title Scene").GetComponent<Text>().enabled = true;
                GameObject.Find("UIIconMenu").GetComponent<Image>().enabled = false;
                GameObject.Find("UIIcon").GetComponent<Image>().enabled = true;
                GameObject.Find("ViviInput").GetComponent<Image>().enabled = true;
                GameObject.Find("ViviAdvice").GetComponent<Image>().enabled = true;
                GameObject.Find("SayWord").GetComponent<Text>().enabled = true;
                GameObject.Find("UIH1").GetComponent<RectTransform>().localScale = initUIH;
                GameObject.Find("HistoryBubble").GetComponent<RectTransform>().localScale = initHis;

                canTalk = false;
                SPEECH.speaking = false;
                uiH.isSpell = false;
                uiH.canSpell = true;
                playScene = false;
                ui.transform.localScale = new Vector3(1f,1f,1f);
                Time.timeScale = 1f;
                mmc.GetComponent<SpriteRenderer>().color = new Vector4(mmc.GetComponent<SpriteRenderer>().color.r,
                   mmc.GetComponent<SpriteRenderer>().color.g, mmc.GetComponent<SpriteRenderer>().color.b, 0f);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //allows the player to talk to the spirit
        if(collision.tag == "Player")
        {
            canTalk = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Makes the spitit unable to talk to.
        if(collision.tag == "Player")
        {
            canTalk = false;
        }
    }
}