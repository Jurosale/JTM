using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

public class NPCSpells : MonoBehaviour
{

    public GameObject musicManagerObj;
    MusicManager musicManager;

    public GameObject spiritspeech;
    public GameObject shrineParticles;
    public string spiritTitle;
    public List<string> spiritMessage;
    public List<string> commandMessage;

    //****If true, then unique "back" & "next" method. If false, autoscroll method
    private bool systemMode;
    private int messageCount, textsize, thisWord;
    private float autoscroll, newbox, oldbox;
    public bool isFlipped, canPray, isPuzzle, spokeTo;

    //Let the prayer animation light play at the shrine
    public GameObject prayerAnimation;

    private FMOD.Studio.PLAYBACK_STATE musicPlaybackState;
    private FMOD.Studio.PLAYBACK_STATE snapShotPlaybackState;

    //surprised sound for vivi
    public string surprisedSound = "event:/Voice Overs/VO_Vivi_Surprise02";

    private Vector3 initPos;
    private Vector4 colorVines, colorFruit;
    public bool shrineAnim, followAnim, prayAnim;
    private float shrineTimer, prayTimer, wordClock;
    private GameObject[] shrines;
    private Animator vivi_anim;

    SpriteRenderer sprite;
    Text text, PorText_Next, PorText_Back;
    Image viviPor, spiritPor, Por_next, Por_back;
    SpeechRecognition01 speech;
    GameObject vivi, box;
    UIHistory uiH;
    UserInterface user;
    SpellBook sb;

    // Use this for initialization
    void Start()
    {
        systemMode = true;
        canPray = false;
        shrineAnim = false;
        followAnim = false;
        prayAnim = false;
        shrineTimer = 0f;
        shrines = GameObject.FindGameObjectsWithTag("shrine");
        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
        user = GameObject.Find("UISpellManager").GetComponent<UserInterface>();
        vivi = GameObject.Find("Witch character");
        viviPor = GameObject.Find("ViviPortrait").GetComponent<Image>();
        spiritPor = GameObject.Find("NPCPortrait").GetComponent<Image>();
        Por_next = GameObject.Find("Por_Next").GetComponent<Image>();
        Por_back = GameObject.Find("Por_Back").GetComponent<Image>();
        PorText_Next = GameObject.Find("PorText_Next").GetComponent<Text>();
        PorText_Back = GameObject.Find("PorText_Back").GetComponent<Text>();
        box = GameObject.Find("DialogueBox");
        text = GameObject.Find("Dialogue").GetComponent<Text>();
        sprite = GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>();
        vivi_anim = vivi.GetComponent<Animator>();
        uiH = GameObject.Find("UIH1").GetComponent<UIHistory>();
        sb = GameObject.Find("SpellBook").GetComponent<SpellBook>();

        Por_next.enabled = false;
        Por_back.enabled = false;
        PorText_Next.enabled = false;
        PorText_Back.enabled = false;

        musicManagerObj = GameObject.FindGameObjectWithTag("Music Manager");
        musicManager = musicManagerObj.GetComponent<MusicManager>();
        initPos = transform.position;
        textsize = text.fontSize;
        spokeTo = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if player casts speak to a spirit, talking cutscene stuff activates
        if (speech.word == "speak" && canPray && !followAnim)
        {
            speech.word = "";
            speech.castOn = false;
            uiH.isSpell = true;
            shrineAnim = true;
            user.talking = true;
            text.text = "";
            thisWord = 0;
            wordClock = 0f;
            speech.speaking = true;
            shrineTimer = 0f;
            Time.timeScale = 0f;
            vivi.GetComponent<Movement>().canMove = false;
            viviPor.enabled = true;
            spiritPor.enabled = true;
            sb.TurnOffDiscoverAnimation();
            if (systemMode)
            {
                Por_next.enabled = true;
                Por_back.enabled = true;
                PorText_Next.enabled = true;
                PorText_Back.enabled = true;
            }
            spiritPor.sprite = GetComponentInChildren<SpriteRenderer>().sprite;
            if (isFlipped) { spiritPor.transform.localScale = new Vector3(Mathf.Abs(spiritPor.transform.localScale.x) * -1, spiritPor.transform.localScale.y, spiritPor.transform.localScale.z); }
            else { spiritPor.transform.localScale = new Vector3(Mathf.Abs(spiritPor.transform.localScale.x), spiritPor.transform.localScale.y, spiritPor.transform.localScale.z); }
            box.GetComponent<Image>().enabled = true;
            box.transform.localScale = new Vector3(Screen.width / 75, Screen.height / 180f, 1f);
            messageCount = 0;
            sprite.color = new Vector4(sprite.color.r,
                    sprite.color.g, sprite.color.b, .95f);
        }

        //while talking is happening, let the player scroll forward and backwards through conversation
        if (shrineAnim)
        {
            if (systemMode)
            {
                //if talking hasnt reached end of dialogue, move forward/backward one at a time
                if (text.text != spiritMessage[messageCount] && thisWord < spiritMessage[messageCount].Length)
                {
                    wordClock += Time.unscaledDeltaTime;
                    PorText_Back.text = "\"" + commandMessage[messageCount * 2] + "\"";
                    PorText_Next.text = "\"" + commandMessage[(messageCount * 2) + 1] + "\"";

                    //makes letters appear one at a time
                    if (wordClock > .02f) {
                        wordClock = 0f;
                        text.text += spiritMessage[messageCount][thisWord];
                        thisWord++;
                    }
                }
                if (speech.word == commandMessage[messageCount * 2]) { messageCount--; speech.word = ""; thisWord = 0; wordClock = 0f; text.text = ""; }
                else if (speech.word != "") { messageCount++; speech.word = ""; thisWord = 0; wordClock = 0f; text.text = ""; }
            }

            //ignore this part of code; we dont use it in final version
            else if (!systemMode && messageCount < spiritMessage.Count)
            {
                text.text = spiritMessage[messageCount];
                autoscroll += Time.unscaledDeltaTime;
                if (autoscroll > 5f) {
                    messageCount++;
                    autoscroll = 0f;
                    if (messageCount >= spiritMessage.Count)
                    {
                        text.text = "";
                        for (int i = 0; i < spiritMessage.Count; i++)
                        {
                            text.text += spiritMessage[i] + " ";
                        }
                        oldbox = box.transform.localScale.y;
                        newbox = box.transform.localScale.y * 2f;
                    }
                }
            }
            
            //if player presses a button, says a special keyword, or finishes conversation, turn off talking scene
            if (systemMode && ((speech.word != "" && speech.word != "next0" && speech.word != "back0") 
                || messageCount >= spiritMessage.Count || messageCount < 0))
            {
                shrineAnim = false;
                speech.speaking = false;
                uiH.isSpell = false;
                uiH.canSpell = true;
                user.talking = false;
                spokeTo = true;
                Time.timeScale = 1f;
                vivi.GetComponent<Movement>().canMove = true;
                viviPor.enabled = false;
                spiritPor.enabled = false;
                Por_next.enabled = false;
                Por_back.enabled = false;
                PorText_Next.enabled = false;
                PorText_Back.enabled = false;
                box.GetComponent<Image>().enabled = false;
                text.text = "";
                sprite.color = new Vector4(sprite.color.r,
                    sprite.color.g, sprite.color.b, 0f);
            }

            //ignore this part of code; we dont use it in final version
            else if (!systemMode && messageCount >= spiritMessage.Count)
            {
                text.fontSize = textsize / spiritMessage.Count * 2;
                Por_next.enabled = true;
                Por_back.enabled = true;
                PorText_Next.enabled = true;
                PorText_Back.enabled = true;
                PorText_Back.text = "\"Repeat\"";
                PorText_Next.text = "\"Understood\"";
                if (speech.word == "repeat") {
                    messageCount = 0;
                    speech.word = "";
                    text.fontSize = textsize;
                    Por_next.enabled = false;
                    Por_back.enabled = false;
                    PorText_Next.enabled = false;
                    PorText_Back.enabled = false;
                }
                else if (speech.word != "") {
                    shrineAnim = false;
                    speech.speaking = false;
                    Time.timeScale = 1f;
                    vivi.GetComponent<Movement>().canMove = true;
                    viviPor.enabled = false;
                    spiritPor.enabled = false;
                    Por_next.enabled = false;
                    Por_back.enabled = false;
                    PorText_Next.enabled = false;
                    PorText_Back.enabled = false;
                    box.GetComponent<Image>().enabled = false;
                    text.text = "";
                    text.fontSize = textsize;
                    sprite.color = new Vector4(sprite.color.r,
                        sprite.color.g, sprite.color.b, 0f);
                }
            }
        }

        //if player cast follow, spirit will follow player
        if (speech.word == "follow" && canPray && isPuzzle)
        {
            //play anim
            followAnim = true;
        }

        //spirit positions themselves relative to player
        if (followAnim)
        {
            GetComponentInChildren<Transform>().position = new Vector3(vivi.GetComponent<Collider2D>().bounds.center.x -
                (transform.localScale.x * 3f * vivi.GetComponent<Movement>().facingRight),
                vivi.GetComponent<Collider2D>().bounds.max.y + .5f, vivi.transform.position.z);
            GetComponentInChildren<TextMesh>().text = "";
            if (!isPuzzle)
            {
                followAnim = false;
                GetComponentInChildren<Transform>().position = initPos;
            }
        }

        //if within a shrine and player casts prayer, prayer animation stuff players
        if (speech.word == "prayer" && canPray && isPuzzle)
        {
            foreach (GameObject shrine in shrines)
            {
                if (Vector2.Distance(transform.position, shrine.transform.position) < 20f)
                {
                    vivi_anim.SetBool("Pray", true);
                    prayAnim = true;
                    followAnim = false;
                    shrineParticles.SetActive(true);
                    uiH.canSpell = true;
                }
            }
        }

        if (prayAnim)
        {
            speech.castOn = false;
            speech.moveOn = false;
            prayTimer += Time.unscaledDeltaTime;
            GetComponentInChildren<TextMesh>().text = spiritTitle;
            transform.position += new Vector3(0, 0.04f, 0);

            //Prevents the player from moving
            vivi.GetComponent<Movement>().canMove = false;

            //Prevent the bacground from moving
            vivi.GetComponent<Movement>().bgMove = false;

            //Plays the Prayer Song
            musicManager.PrayerMusic();

            //Play the light animation
            prayerAnimation.SetActive(true);
            if (!isPuzzle)
            {
                prayAnim = false;
                GetComponentInChildren<Transform>().position = initPos;
                vivi.GetComponent<Movement>().canMove = true;
                vivi.GetComponent<Movement>().bgMove = true;
                prayerAnimation.SetActive(false);
                prayTimer = 0f;
                GetComponentInChildren<TextMesh>().text = "";
            }
        }




        //Stops prayer animation stuff after 10 seconds
        if (prayTimer > 10.0f)
        {
            uiH.isSpell = false;
            Destroy(gameObject);
            //}
            vivi.GetComponent<AvatarSpells>().tasks++;

            //Lets the player move again
            vivi.GetComponent<Movement>().canMove = true;

            //Let the bacground move again
            vivi.GetComponent<Movement>().bgMove = true;

            //stop the light animation
            prayerAnimation.SetActive(false);

            //turns off the prayer animation
            prayAnim = false;
            prayTimer = 0f;
            vivi_anim.SetBool("Pray", false);
            uiH.canSpell = true;
        }
    }

    //set flag
    private void OnTriggerStay2D(Collider2D collision)
    {
        //makes spirit flip if player is behind them
        if (collision.tag == "Player")
        {
            canPray = true;
            if (!isFlipped)
            {
                if (collision.transform.position.x > transform.position.x) { GetComponentInChildren<SpriteRenderer>().flipX = true; }
                else { GetComponentInChildren<SpriteRenderer>().flipX = false; }
            }
            else
            {
                if (collision.transform.position.x < transform.position.x) { GetComponentInChildren<SpriteRenderer>().flipX = true; }
                else { GetComponentInChildren<SpriteRenderer>().flipX = false; }
            }
        }
    }

    //undo flag
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !followAnim)
        {
            canPray = false;
        }
    }

}
