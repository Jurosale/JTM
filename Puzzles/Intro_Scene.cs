using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intro_Scene : MonoBehaviour
{

    public GameObject tree;
    public GameObject shrine;
    public GameObject avatar;
    public GameObject UIicon, UIiconMenu;
    public GameObject puzzleSpirit;
    public Text title;
    private Image bubble;
    private Text tutorial1;
    public Sprite newTree;
    private Vector3 initPos;
    public RuntimeAnimatorController happyAnim, angryAnim;
    public Sprite happySpirit, angrySpirit;
    public GameObject diarything;

    private bool openBook = false, fireTask = false, hintTask = false, restoreTask = false, spellChecker = false, bookcheck = true,
        spoke2spirit = false, restspirit = false, follows = false, stageComplete = false, speakTask = false, followMe = false;

    private float clock = 0f, clock2 = 0f, clock3 = 0f, controls = 0f, jumping = 0f, vines = 0f;
    private float songTimer = 0f;

    //Spirit Stuff
    private string greenSpiritM, greenSpiritT;
    private Vector4 greenSpiritC, viviPortrait;

    //Music
    FMOD.Studio.EventInstance spiritInteraction;
    public string musicPrayer = "event:/BGM/MUS_SpiritPrayer";

    //Found Spellbook SFX
    public string spellBookFound = "event:/UI sounds/SFX_DiscoverSpell_UI";

    SpeechRecognition01 speech;
    UserInterface ui;
    GameObject book, spirit, vivi, thingbuttonthingythingy;
    GameObject bookT, speakT, castT, restoreT, followT, prayerT, diaryT;
    Image viviPor;
    Vector3 bookSize, iconSize, uiSize;
    UIHistory uiH;

    // Use this for initialization
    void Start()
    {
        //these access various gameobjects like Ui stuff, spellbook, speech recognition, etc.
        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
        ui = GameObject.Find("UISpellManager").GetComponent<UserInterface>();
        book = GameObject.Find("SpellBook");
        viviPor = GameObject.Find("ViviPortrait").GetComponent<Image>();
        spirit = GameObject.Find("NPC_Spirit");
        vivi = GameObject.Find("Witch character");
        thingbuttonthingythingy = GameObject.Find("RedButton");
        //these access all the tutorials in intro level
        bookT = GameObject.Find("tutorial_spellbook");
        speakT = GameObject.Find("tutorial_speak");
        castT = GameObject.Find("tutorial_cast");
        restoreT = GameObject.Find("tutorial_restore");
        followT = GameObject.Find("tutorial_follow");
        prayerT = GameObject.Find("tutorial_prayer");
        diaryT = GameObject.Find("tutorial_diary");

        //resets spell animations
        GameObject.Find("UseSpellAnim").GetComponent<UseSpell>().Active = false;
        book.GetComponent<SpellBook>().resetFlags2();

        //turns off the later tutorials untilthey'reneeded
        bookT.GetComponent<SpriteRenderer>().enabled = false;
        bookT.GetComponentInChildren<MeshRenderer>().enabled = false;
        speakT.GetComponent<SpriteRenderer>().enabled = false;
        speakT.GetComponentInChildren<MeshRenderer>().enabled = false;
        castT.GetComponent<SpriteRenderer>().enabled = false;
        castT.GetComponentInChildren<MeshRenderer>().enabled = false;
        restoreT.GetComponent<SpriteRenderer>().enabled = false;
        restoreT.GetComponentInChildren<MeshRenderer>().enabled = false;
        followT.GetComponent<SpriteRenderer>().enabled = false;
        followT.GetComponentInChildren<MeshRenderer>().enabled = false;
        prayerT.GetComponent<SpriteRenderer>().enabled = true;
        prayerT.GetComponentInChildren<MeshRenderer>().enabled = true;
        diaryT.GetComponent<SpriteRenderer>().enabled = false;
        diaryT.GetComponentInChildren<MeshRenderer>().enabled = false;

        //resets a bunch of UI stuff
        ui.hint = "";
        title.color = new Vector4(80f, 255f, 255f, 0f);
        gameObject.GetComponent<SpriteRenderer>().color = new Vector4(255f, 255f, 255f, 255f);
        speech.canSpell = false;
        GameObject.Find("SoundEffects").GetComponent<test2>().enabled = false;
        uiH = GameObject.Find("UIH1").GetComponent<UIHistory>();
        bookSize = book.transform.localScale;
        book.transform.localScale = new Vector3(1f, 0f, 1f);
        initPos = puzzleSpirit.transform.position;
        book.GetComponent<SpellBook>().enabled = false;
        uiSize = ui.transform.localScale;
        ui.transform.localScale = new Vector3(1f, 0f, 1f);
        ui.enabled = false;
        iconSize = UIicon.transform.localScale;
        UIicon.transform.localScale = new Vector3(1f, 0f, 1f);
        UIiconMenu.transform.localScale = new Vector3(1f, 0f, 1f);
        
        //ignore this code below; its old and not used anymore
        greenSpiritT = "Thank You";
        greenSpiritM = puzzleSpirit.GetComponent<NPCSpells>().spiritMessage[0];
        greenSpiritC = puzzleSpirit.GetComponentInChildren<SpriteRenderer>().color;
        viviPortrait = new Vector4(viviPor.color.r, viviPor.color.g, viviPor.color.b, 1f);
    }

    // Update is called once per frame
    // Very convoluted way of controlling the tutorial. Will be updated soon
    void Update()
    {
        //if dairy page grabbed, display diary tutorial
        if (bookcheck && book.GetComponent<SpellBook>().pages % 2 == 0) {
            diaryT.GetComponent<SpriteRenderer>().enabled = true;
            diaryT.GetComponentInChildren<MeshRenderer>().enabled = true;
            bookcheck = false;
        }

        //is spellbook hasnt been grabbed yet, dont allow player to cast spells
        if (GetComponent<SpriteRenderer>().color.a != 0 && speech.castOn) { speech.castOn = false; }

        //when spellbook is first grabbed, add in the default spells to book
        if (spellChecker)
        {
            //insert default words
            if (!speech.discovered.Contains("PRAYER"))
            {
                speech.discovered.Add("PRAYER");
                speech.type.Add("prayer");
                speech.called.Add(1);
            }

            if (!speech.discovered.Contains("HINT"))
            {
                speech.discovered.Add("HINT");
                speech.type.Add("hint");
                speech.called.Add(1);
            }

            if (!speech.discovered.Contains("RESTORE"))
            {
                speech.discovered.Add("RESTORE");
                speech.type.Add("restore");
                speech.called.Add(1);
            }

            if (!speech.discovered.Contains("SPEAK"))
            {
                speech.discovered.Add("SPEAK");
                speech.type.Add("speak");
                speech.called.Add(1);
            }

            if (!speech.discovered.Contains("FOLLOW"))
            {
                speech.discovered.Add("FOLLOW");
                speech.type.Add("follow");
                speech.called.Add(1);
            }

            if (!speech.discovered.Contains("TALK"))
            {
                speech.discovered.Add("TALK");
                speech.type.Add("speak");
                speech.called.Add(1);
            }

            if (!speech.discovered.Contains("COME"))
            {
                speech.discovered.Add("COME");
                speech.type.Add("follow");
                speech.called.Add(1);
            }

            if (!speech.discovered.Contains("PRAY"))
            {
                speech.discovered.Add("PRAY");
                speech.type.Add("prayer");
                speech.called.Add(1);
            }

            ui.transform.localScale = uiSize;
            spellChecker = false;
        }

        //turns on the book icon and spell hints once book is grabbed
        //turns on spellbook tutorial
        else if (openBook)
        {
            UIiconMenu.transform.localScale = iconSize;
            book.GetComponent<SpellBook>().enabled = true;
            book.transform.localScale = bookSize;
            ui.enabled = true;
            FMODUnity.RuntimeManager.PlayOneShot(spellBookFound);
            speakTask = true;
            openBook = false;
            GameObject.Find("UseSpellAnim").GetComponent<UseSpell>().Active = true;
            bookT.GetComponent<SpriteRenderer>().enabled = true;
            bookT.GetComponentInChildren<MeshRenderer>().enabled = true;
        }

        //turns on speak tutorial once player gets close to first spirit
        else if (speakTask)
        {
            if (Vector2.Distance(vivi.transform.position, spirit.transform.position) <= 4.5f)
            {
                speakT.GetComponent<SpriteRenderer>().enabled = true;
                speakT.GetComponentInChildren<MeshRenderer>().enabled = true;
                speakTask = false;
                fireTask = true;
            }
        }

        //turns on hint tutorial if player doesnt solve tree puzzle within 8 seconds
        else if (hintTask)
        {
            if (tree.transform.localScale.y == 0)
            {
                fireTask = false;
                hintTask = false;
                restoreTask = true;
            }
        }

        else if (clock >= 8f && fireTask)
        {
            castT.GetComponent<SpriteRenderer>().enabled = true;
            castT.GetComponentInChildren<MeshRenderer>().enabled = true;
            if (tree.transform.localScale.y == 0)
            {
                fireTask = false;
                restoreTask = true;
            }
        }

        //if player casts hint, reveal fire as hint
        else if (fireTask)
        {
            if (vivi.GetComponent<Movement>().canMove) { spoke2spirit = true; }
            if (speech.word == "hint" || uiH.isHint)
            {
                hintTask = true;
            }
            if (tree.transform.localScale.y == 0)
            {
                fireTask = false;
                restoreTask = true;
            }
        }

        //once tree is burned down, reveal restore tutorial if player tries to leave
        else if (restoreTask)
        {
            clock3 = 0f;
            clock2 = 0f;
            if (speech.word == "follow" || follows)
            {
                follows = true;
            }
            //Debug.Log(puzzleSpirit.transform.position.x);
            if (avatar.transform.position.x > 35f && tree.transform.localScale.y == 0)
            {
                restoreT.GetComponent<SpriteRenderer>().enabled = true;
                restoreT.GetComponentInChildren<MeshRenderer>().enabled = true;
            }

            if (tree.transform.localScale.y != 0)
            {
                restoreTask = false;
                clock = 10f;
                followMe = true;
                clock3 = 0f;
                restoreT.GetComponent<SpriteRenderer>().enabled = false;
                restoreT.GetComponentInChildren<MeshRenderer>().enabled = false;
                
                //ignore this code below; old and not used
                //puzzleSpirit.GetComponentInChildren<SpriteRenderer>().color = greenSpiritC;
                puzzleSpirit.GetComponentInChildren<SpriteRenderer>().sprite = happySpirit;
                puzzleSpirit.GetComponentInChildren<Animator>().runtimeAnimatorController = happyAnim;
                tree.GetComponent<SpriteRenderer>().sprite = newTree;

                //Inkan: takes out the spirit interaction comic script
                puzzleSpirit.GetComponent<Spirit_Interaction>().enabled = false;

                //Inkan: Enables the NPC script again
                puzzleSpirit.GetComponent<NPCSpells>().enabled = true;
                puzzleSpirit.GetComponent<NPCSpells>().isPuzzle = true;
            }
        }

        //checks if book grabbed
        else if (gameObject.GetComponent<SpriteRenderer>().color.a == 0f && clock == 0f)
        {
            openBook = true;
        }

        //checks if tree is ever burned after puzzle is solved
        else if (tree.transform.localScale.y == 0 && clock == 10f)
        {
            restoreTask = true;
            followMe = false;
            restspirit = false;
        }

        //turns on follow tutorial once tree has been restored and turns off once spirit follows player
        else if (followMe)
        {
            clock3 += Time.deltaTime;
            if (speech.word == "follow" || follows)
            {
                follows = true;
                followMe = false;

                restspirit = true;
                followT.GetComponent<SpriteRenderer>().enabled = false;
                followT.GetComponentInChildren<MeshRenderer>().enabled = false;
            }

            if (clock3 > 1f && !follows)
            {
                followT.GetComponent<SpriteRenderer>().enabled = true;
                followT.GetComponentInChildren<MeshRenderer>().enabled = true;
            }
        }
        
        //if player casts prayer near shrine, turns off prayer tutorial
        //lets the game know that the player has now completed the spirit task
        else if (restspirit)
        {

            if (follows && Vector2.Distance(avatar.transform.position, shrine.transform.position) <= 5f)
            {
                if (speech.word == "prayer")
                {
                    //Let the music play
                    spiritInteraction = FMODUnity.RuntimeManager.CreateInstance(musicPrayer);
                    spiritInteraction.start();

                    stageComplete = true;
                    restspirit = false;
                    follows = false;
                    prayerT.GetComponent<SpriteRenderer>().enabled = false;
                    prayerT.GetComponentInChildren<MeshRenderer>().enabled = false;
                }
            }
        }

        //ignore this code below
        else if (stageComplete)
        {
        }

        if (spoke2spirit && clock <= 8f) { clock += Time.deltaTime; }

        if (follows & !stageComplete)
        {
        }

        //if player casts hint, provides the necessary hint
        if (speech.word == "hint" || uiH.isHint)
        {
            //uiH.isHint = false;
            if (Vector2.Distance(avatar.transform.position, diarything.transform.position) <= 5f)
            {
                ui.hint = "grow";
            }
            else if (Vector2.Distance(avatar.transform.position, thingbuttonthingythingy.transform.position) <= 20f)
            {
                ui.hint = "grow";
            }
            else if (tree.transform.localScale.y == 0)
            {
                ui.hint = "restore";
            }

            else if (Vector2.Distance(avatar.transform.position, tree.transform.position) <= 6.5f)
            {
                ui.hint = "fire";
            }


            else
            {
                ui.hint = "";
            }
        }

        //if tree is burned and tutorial hasnt been completed yet, reset the spirit to its inital position and angry
        if (tree.transform.localScale.y == 0 && !stageComplete)
        {
            restoreTask = true;
            speakTask = false;
            followMe = false;
            follows = false;

            //Inkan: Enables the NPC script again
            puzzleSpirit.GetComponent<NPCSpells>().enabled = false;
            puzzleSpirit.GetComponent<NPCSpells>().isPuzzle = false;

            puzzleSpirit.transform.position = initPos;
            restspirit = false;

            //ignore this code below
            puzzleSpirit.GetComponentInChildren<SpriteRenderer>().sprite = angrySpirit;
            puzzleSpirit.GetComponentInChildren<Animator>().runtimeAnimatorController = angryAnim;

            //turns off follow tutorial
            followT.GetComponent<SpriteRenderer>().enabled = false;
            followT.GetComponentInChildren<MeshRenderer>().enabled = false;

            //Inkan: takes out the spirit interaction comic script
            puzzleSpirit.GetComponent<Spirit_Interaction>().enabled = true;

        }

        //ignore this
        else if (vivi.GetComponent<Movement>().canMove == false)
        {
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        //grabs the book once the player touches it
        //turns on the rest of UI, spellbook, and lets player cast spells once the player grabs the book
        if (other.tag == "Player")
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Vector4(0f, 0f, 0f, 0f);
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            speech.canSpell = true;
            GameObject.Find("SoundEffects").GetComponent<test2>().enabled = true;
            UIicon.transform.localScale = iconSize;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            spellChecker = true;
        }
    }
}
