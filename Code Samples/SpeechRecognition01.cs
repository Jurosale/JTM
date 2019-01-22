//Original code by: Michail Moiropoulos
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

/* ********************************************
 *      Listens for player's voice and 
 *      let's game know which spell is
 *      being casted
*********************************************** */

public class SpeechRecognition01 : MonoBehaviour
{

    // Variables for Speech recognition.
    public string[] keywords;

    public ConfidenceLevel confidence = ConfidenceLevel.Low;                     //How well the speech recognition works regarding accents, etc (low = can understand more accents)
    private float timer;
    public bool canSpell, isNewSpell;
    public bool castOn, moveOn, speaking;                                                     //Enable cast, movement
    private float clock, titleTimer;
    public float castTimer, discoverTimer;

    public float pitch;
    private float value;                                                            //Volume
    public int i;                                                                   //Samples
    public AudioSource aud;                                                         //Audio Clip in 1 sec
    public LineRenderer line;

    public GameObject target, viviAdvice;                                                       //Witch
    public Text results, points;
    public Text newSpell;

    private GameObject viviInput, spellbook, avatar;
    public Sprite viviTalk, viviSilent;

    protected PhraseRecognizer recognizer;                                          //Recognizer (not sure what it does)
    public string word, feedback;
    private string keyword, oldWord;
    private bool pointCheck, typeCheck = false;

    UserInterface uif;
    //Player word points and discovered spells so far
    public int wordPnts, displayPnts;
    public int fontSize, growRate;
    public List<string> discovered;
    public List<string> type;
    public List<int> called;

    //Abbandoned Crafting System
    public string craft1;
    public bool crafted1;

    // Use this for initialization
    void Start()
    {
        //Time.timeScale = .1f;
        keywords = new string[] {
            "earth","rock","mountain","elevation","cliff","raise",
            "water","aqua","fill","rain","flood","shower","wet","ocean","precipitation","puddle","wave",
            "wind","move","gust","blow","push","air","gale","puff","breeze","breath","waft",
            "fire","burn","incinerate","flame","ember","blaze","pyre","light","destroy","ignite","torch","burst","combust",
            "cool","ice","snow","crystal","hail","blizzard","freeze","cold","winter",
            "restore","return","reset","undo","redo","origin", "mend", "fix", "rebuild", "repair", "reconstruct",
            "grow","big","tall","huge","increase","expand","giant","colossal","enormous","jumbo","oversize","massive",
            "shrink","small","mini","narrow","cramp","decrease","short","tiny", "crouch",
            "hint","question","inquiry","curious","help","confuse","puzzle","unclear","lost","clue","idea","tip","advice",
            "prayer", "pray", "speak", "talk", "follow", "come",

            "cute", "adorable", "happy", "delight", "pretty", "beautiful", "pleasant", "cheerful",
            "sheep","whale", "krill",
            "bitch","shit","oh shit","fuck","damn","harlot", "motherfucker", "what the fuck", "wocao",

            "light","tornado", "cast",

            "yes", "no", "quit",

            "Repeat", "Understood", "What?", "War?", "Traveling", "Sure", "Shrine?", "Okay", "Of Course", "Explain", "Lingering?", "Goddess?", "City?", "I am",
            "What city?", "Oh", "Child?", "Say again", "Whatever", "Regret?", "Ring what?", "She what?", "Lost hope?", "Tree?", "Goddess?", "Pond?", "Nothing",
            "Peace?", "Roots?", "Remains?", "Light?", "Detached?", "I won't", "We did?", "Lake?", "Agreed", "Witches?", "Oh no", "revitalize", "home", "teleport",

            "open spellbook", "close spellbook", "next", "back", "open", "close", "main menu", "spells", "diary",

            "Vivi", "Bibi", "VV", "BB","Wiwi","Riri", "Lily", "Bwibwi",
            "move left", "left", "walk left", "run left", "move right", "right", "walk right", "run left", "jump", "climb", "turn"
        };
        //Old version of keyword "cast"
        /*keyword = "cast ";
        for (int i = 0; i < keywords.Length; i++) 
        {
            keywords[i] = keyword + keywords[i];
            if (keywords[i] == keyword) { break; }
        }*/

        canSpell = true;
        speaking = false;
        wordPnts = GameObject.Find("Variables").GetComponent<UniversialVariables>().currWordPnts;
        discovered = GameObject.Find("Variables").GetComponent<UniversialVariables>().currDiscovered;
        type = GameObject.Find("Variables").GetComponent<UniversialVariables>().currType;
        called = GameObject.Find("Variables").GetComponent<UniversialVariables>().currCalled;
        viviInput = GameObject.Find("ViviInput");
        viviAdvice = GameObject.Find("ViviAdvice");
        spellbook = GameObject.Find("SpellBook");
        avatar = GameObject.Find("Witch character");

        timer = 1.0f;
        fontSize = 40;
        growRate = 10;
        if (keywords != null)
        {
            recognizer = new KeywordRecognizer(keywords, confidence);
            recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
            recognizer.Start();
        }
        crafted1 = false;
        //Microphone Set up
        aud = GetComponent<AudioSource>();
        aud.clip = Microphone.Start("Built-in Microphone", true, 1, 3200);
        aud.Play();
        line.positionCount = 64;

        isNewSpell = false;
        discoverTimer = 7f;
        uif = GameObject.Find("UISpellManager").GetComponent<UserInterface>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("cast time: " + castTimer);
        clock += Time.deltaTime;
        if (displayPnts != wordPnts)
        {

            if (points.fontSize != fontSize + 10)
                points.fontSize++;
            if (displayPnts < wordPnts)
            {
                displayPnts += growRate;
            }
            else
            {
                displayPnts -= growRate;
            }
        }
        else if (points.fontSize != fontSize)
        {
            points.fontSize--;
        }

        points.text = displayPnts + "pts";
        //points.text = Screen.width.ToString() + " " + Screen.height.ToString();

        //will check if it's a new word
        //if (oldWord.Contains(keyword) && pointCheck) { discover(oldWord); }
        if (discoverTimer < 7f) { discoverEffect(); }
        if (titleTimer < 14f) { titleEffect(); }

        //Words that sound similar to Vivi
        if (word == "Bibi0" || word == "VV0" || word == "BB0" || word == "Wiwi0" || word == "Riri0" || word == "Lily0" || word == "Bwibwi0") { word = "Vivi0"; }

        //New version of "cast"
        if (word == "cast0" && !castOn && !moveOn) { castOn = true; }
        if (word == "Vivi0" && !moveOn && !castOn) { moveOn = true; }
        if (castOn || moveOn)
        {
            viviAdvice.GetComponent<Image>().enabled = false;
            viviAdvice.GetComponentInChildren<Text>().text = "";
            viviInput.GetComponent<Image>().sprite = viviSilent;
        }
        
        //turns on movement particle system
        if (word == "" && !castOn && !moveOn && viviAdvice.GetComponentInChildren<Text>().text != "\"Vivi\"" && !speaking
            && !spellbook.GetComponent<SpellBook>().isOpen && viviAdvice.GetComponentInChildren<Text>().text != "\"Cast\"")
        {
            viviAdvice.GetComponent<Image>().enabled = true;
            viviAdvice.GetComponentInChildren<Text>().text = "";
            viviInput.GetComponent<Image>().color = new Vector4(1f, 1f, 1f, 1f);
            viviInput.GetComponent<Image>().sprite = viviTalk;
        }

        //updates vivi ui phrase
        if (avatar.GetComponent<Movement>().sayCast && viviAdvice.GetComponentInChildren<Text>().text != "\"Cast\""
            && viviAdvice.GetComponent<Image>().enabled == true)
        {
            viviAdvice.GetComponentInChildren<Text>().text = "\"Cast\"";
        }
        if (!avatar.GetComponent<Movement>().sayCast && viviAdvice.GetComponentInChildren<Text>().text != "\"Vivi\""
            && viviAdvice.GetComponent<Image>().enabled == true)
        {
            viviAdvice.GetComponentInChildren<Text>().text = "\"Vivi\"";
        }

        //resets cast/move animation after 5 seconds
        if (castTimer >= 5.0f)
        {
            castOn = false;
            moveOn = false;
            castTimer = 0;
            viviInput.GetComponent<Image>().color = new Vector4(1f, 1f, 1f, 1f);
        }
        //crafting words (abanddoned)
        /*if (word == keywords[119]) {
			if (crafted1) {
				if (pointCheck)
					PointSystem (5, word);
				word = "fire";
			}
		}
        if (word == keywords[120])
        {
            if (crafted1)
            {
                if (pointCheck)
                    PointSystem(5, word);
                word = "wind";
            }
        }*/
        //assigns points for what world is used and casts the appropriate spell
        //Old version of keyword "cast"
        /*if (!canSpell && word.Contains(keyword)) { word = ""; }
        else if (word == keyword) { word = "error"; }
        else if (word.Contains(keyword)) { word = word.Replace(keyword, ""); }*/

        //if player says "cast", check for spells
        if (castOn == true)
        {

            avatar.GetComponent<Movement>().moveleft = false;
            avatar.GetComponent<Movement>().moveright = false;
            avatar.GetComponent<Movement>().Run = false;
            if (word == "cast0") { word = ""; }
            if (word.Contains("0")) { word = word.Replace("0", ""); }
            castTimer += Time.deltaTime;
            switch (word)
            {
                case "":
                    word = "";
                    break;
                case "error":
                    word = "error";
                    break;
                case "fire":
                    if (pointCheck)
                        PointSystem(1, word);
                    word = "fire";
                    break;
                case "incinerate":
                    if (pointCheck)
                        PointSystem(5, word);
                    word = "fire";
                    break;
                case "burn":
                    if (pointCheck)
                        PointSystem(3, word);
                    word = "fire";
                    break;
                case "flame":
                    if (pointCheck)
                        PointSystem(2, word);
                    word = "fire";
                    break;
                case "water":
                    if (pointCheck)
                        PointSystem(1, word);
                    word = "water";
                    break;
                case "aqua":
                    if (pointCheck)
                        PointSystem(10, word);
                    word = "water";
                    break;
                case "fill":
                    if (pointCheck)
                        PointSystem(3, word);
                    word = "water";
                    break;
                case "wind":
                    if (pointCheck)
                        PointSystem(1, word);
                    word = "wind";
                    break;
                case "move":
                    if (pointCheck)
                        PointSystem(2, word);
                    word = "wind";
                    break;
                case "gust":
                    if (pointCheck)
                        PointSystem(3, word);
                    word = "wind";
                    break;
                case "blow":
                    if (pointCheck)
                        PointSystem(2, word);
                    word = "wind";
                    break;
                case "ice":
                    if (pointCheck)
                        PointSystem(1, word);
                    word = "ice";
                    break;
                case "cool":
                    if (pointCheck)
                        PointSystem(2, word);
                    word = "ice";
                    break;
                case "grow":
                    if (pointCheck)
                        PointSystem(1, word);
                    word = "grow";
                    break;
                case "shrink":
                    if (pointCheck)
                        PointSystem(1, word);
                    word = "shrink";
                    break;
                case "hint":
                    if (pointCheck)
                        PointSystem(1, word);
                    word = "hint";
                    break;
                case "restore":
                    if (pointCheck)
                        PointSystem(1, word);
                    word = "restore";
                    break;
                case "earth":
                    if (pointCheck)
                        PointSystem(1, word);
                    word = "earth";
                    break;
                case "ember":
                    if (pointCheck)
                        PointSystem(4, word);
                    word = "fire";
                    break;
                case "blaze":
                    if (pointCheck)
                        PointSystem(5, word);
                    word = "fire";
                    break;
                case "pyre":
                    if (pointCheck)
                        PointSystem(10, word);
                    word = "fire";
                    break;
                case "light":
                    if (pointCheck)
                        PointSystem(5, word);
                    word = "fire";
                    break;
                case "destroy":
                    if (pointCheck)
                        PointSystem(7, word);
                    word = "fire";
                    break;
                case "ignite":
                    if (pointCheck)
                        PointSystem(4, word);
                    word = "fire";
                    break;
                case "torch":
                    if (pointCheck)
                        PointSystem(8, word);
                    word = "fire";
                    break;
                case "burst":
                    if (pointCheck)
                        PointSystem(6, word);
                    word = "fire";
                    break;
                case "combust":
                    if (pointCheck)
                        PointSystem(7, word);
                    word = "fire";
                    break;
                case "push":
                    if (pointCheck)
                        PointSystem(2, word);
                    word = "wind";
                    break;
                case "air":
                    if (pointCheck)
                        PointSystem(3, word);
                    word = "wind";
                    break;
                case "gale":
                    if (pointCheck)
                        PointSystem(10, word);
                    word = "wind";
                    break;
                case "puff":
                    if (pointCheck)
                        PointSystem(2, word);
                    word = "wind";
                    break;
                case "breeze":
                    if (pointCheck)
                        PointSystem(3, word);
                    word = "wind";
                    break;
                case "breath":
                    if (pointCheck)
                        PointSystem(2, word);
                    word = "wind";
                    break;
                case "waft":
                    if (pointCheck)
                        PointSystem(10, word);
                    word = "wind";
                    break;
                case "rain":
                    if (pointCheck)
                        PointSystem(2, word);
                    word = "water";
                    break;
                case "flood":
                    if (pointCheck)
                        PointSystem(6, word);
                    word = "water";
                    break;
                case "shower":
                    if (pointCheck)
                        PointSystem(7, word);
                    word = "water";
                    break;
                case "wet":
                    if (pointCheck)
                        PointSystem(4, word);
                    word = "water";
                    break;
                case "ocean":
                    if (pointCheck)
                        PointSystem(7, word);
                    word = "water";
                    break;
                case "precipitation":
                    if (pointCheck)
                        PointSystem(10, word);
                    word = "water";
                    break;
                case "puddle":
                    if (pointCheck)
                        PointSystem(3, word);
                    word = "water";
                    break;
                case "wave":
                    if (pointCheck)
                        PointSystem(5, word);
                    word = "water";
                    break;
                case "rock":
                    if (pointCheck)
                        PointSystem(2, word);
                    word = "earth";
                    break;
                case "mountain":
                    if (pointCheck)
                        PointSystem(4, word);
                    word = "earth";
                    break;
                case "elevation":
                    if (pointCheck)
                        PointSystem(10, word);
                    word = "earth";
                    break;
                case "cliff":
                    if (pointCheck)
                        PointSystem(9, word);
                    word = "earth";
                    break;
                case "raise":
                    if (pointCheck)
                        PointSystem(8, word);
                    word = "earth";
                    break;
                case "snow":
                    if (pointCheck)
                        PointSystem(2, word);
                    word = "ice";
                    break;
                case "crystal":
                    if (pointCheck)
                        PointSystem(9, word);
                    word = "ice";
                    break;
                case "hail":
                    if (pointCheck)
                        PointSystem(4, word);
                    word = "ice";
                    break;
                case "blizzard":
                    if (pointCheck)
                        PointSystem(3, word);
                    word = "ice";
                    break;
                case "freeze":
                    if (pointCheck)
                        PointSystem(4, word);
                    word = "ice";
                    break;
                case "cold":
                    if (pointCheck)
                        PointSystem(2, word);
                    word = "ice";
                    break;
                case "winter":
                    if (pointCheck)
                        PointSystem(3, word);
                    word = "ice";
                    break;
                case "return":
                    if (pointCheck)
                        PointSystem(2, word);
                    word = "restore";
                    break;
                case "reset":
                    if (pointCheck)
                        PointSystem(2, word);
                    word = "restore";
                    break;
                case "undo":
                    if (pointCheck)
                        PointSystem(4, word);
                    word = "restore";
                    break;
                case "redo":
                    if (pointCheck)
                        PointSystem(3, word);
                    word = "restore";
                    break;
                case "origin":
                    if (pointCheck)
                        PointSystem(8, word);
                    word = "restore";
                    break;
                case "mend":
                    if (pointCheck)
                        PointSystem(2, word);
                    word = "restore";
                    break;
                case "fix":
                    if (pointCheck)
                        PointSystem(2, word);
                    word = "restore";
                    break;
                case "rebuild":
                    if (pointCheck)
                        PointSystem(4, word);
                    word = "restore";
                    break;
                case "repair":
                    if (pointCheck)
                        PointSystem(4, word);
                    word = "restore";
                    break;
                case "reconstruct":
                    if (pointCheck)
                        PointSystem(8, word);
                    word = "restore";
                    break;
                case "big":
                    if (pointCheck)
                        PointSystem(2, word);
                    word = "grow";
                    break;
                case "tall":
                    if (pointCheck)
                        PointSystem(2, word);
                    word = "grow";
                    break;
                case "huge":
                    if (pointCheck)
                        PointSystem(3, word);
                    word = "grow";
                    break;
                case "increase":
                    if (pointCheck)
                        PointSystem(4, word);
                    word = "grow";
                    break;
                case "expand":
                    if (pointCheck)
                        PointSystem(5, word);
                    word = "grow";
                    break;
                case "giant":
                    if (pointCheck)
                        PointSystem(6, word);
                    word = "grow";
                    break;
                case "colossal":
                    if (pointCheck)
                        PointSystem(10, word);
                    word = "grow";
                    break;
                case "enormous":
                    if (pointCheck)
                        PointSystem(7, word);
                    word = "grow";
                    break;
                case "jumbo":
                    if (pointCheck)
                        PointSystem(8, word);
                    word = "grow";
                    break;
                case "oversize":
                    if (pointCheck)
                        PointSystem(9, word);
                    word = "grow";
                    break;
                case "massive":
                    if (pointCheck)
                        PointSystem(6, word);
                    word = "grow";
                    break;
                case "small":
                    if (pointCheck)
                        PointSystem(2, word);
                    word = "shrink";
                    break;
                case "mini":
                    if (pointCheck)
                        PointSystem(4, word);
                    word = "shrink";
                    break;
                case "narrow":
                    if (pointCheck)
                        PointSystem(5, word);
                    word = "shrink";
                    break;
                case "cramp":
                    if (pointCheck)
                        PointSystem(6, word);
                    word = "shrink";
                    break;
                case "decrease":
                    if (pointCheck)
                        PointSystem(3, word);
                    word = "shrink";
                    break;
                case "short":
                    if (pointCheck)
                        PointSystem(2, word);
                    word = "shrink";
                    break;
                case "tiny":
                    if (pointCheck)
                        PointSystem(2, word);
                    break;
                case "crouch":
                    if (pointCheck)
                        PointSystem(2, word);
                    break;
                case "question":
                    if (pointCheck)
                        PointSystem(2, word);
                    word = "hint";
                    break;
                case "inquiry":
                    if (pointCheck)
                        PointSystem(5, word);
                    word = "hint";
                    break;
                case "curious":
                    if (pointCheck)
                        PointSystem(5, word);
                    word = "hint";
                    break;
                case "help":
                    if (pointCheck)
                        PointSystem(2, word);
                    word = "hint";
                    break;
                case "confuse":
                    if (pointCheck)
                        PointSystem(2, word);
                    word = "hint";
                    break;
                case "puzzle":
                    if (pointCheck)
                        PointSystem(4, word);
                    word = "hint";
                    break;
                case "unclear":
                    if (pointCheck)
                        PointSystem(4, word);
                    word = "hint";
                    break;
                case "lost":
                    if (pointCheck)
                        PointSystem(3, word);
                    word = "hint";
                    break;
                case "clue":
                    if (pointCheck)
                        PointSystem(5, word);
                    word = "hint";
                    break;
                case "idea":
                    if (pointCheck)
                        PointSystem(8, word);
                    word = "hint";
                    break;
                case "tip":
                    if (pointCheck)
                        PointSystem(2, word);
                    word = "hint";
                    break;
                case "advice":
                    if (pointCheck)
                        PointSystem(4, word);
                    word = "hint";
                    break;
                case "prayer":
                    word = "prayer";
                    break;
                case "pray":
                    word = "prayer";
                    break;
                case "speak":
                    word = "speak";
                    break;
                case "talk":
                    word = "speak";
                    break;
                case "follow":
                    word = "follow";
                    break;
                case "come":
                    word = "follow";
                    break;
                case "yes":
                    word = "yes";
                    break;
                case "no":
                    word = "no";
                    break;
                case "cute":
                    word = "cute";
                    break;
                case "adorable":
                    word = "cute";
                    break;
                case "happy":
                    word = "cute";
                    break;
                case "pretty":
                    word = "cute";
                    break;
                case "beautiful":
                    word = "cute";
                    break;
                case "pleasant":
                    word = "cute";
                    break;
                case "revitalize":
                    word = "revitalize";
                    break;
                case "sheep":
                    word = "sheep";
                    break;
                case "whale":
                    word = "whale";
                    break;
                case "krill":
                    word = "krill";
                    break;
                case "bitch":
                    word = "easter";
                    break;
                case "shit":
                    word = "easter";
                    break;
                case "oh shit":
                    word = "easter";
                    break;
                case "fuck":
                    word = "easter";
                    break;
                case "damn":
                    word = "easter";
                    break;
                case "harlot":
                    word = "easter";
                    break;
                case "motherfucker":
                    word = "easter";
                    break;
                case "what the fuck":
                    word = "easter";
                    break;
                case "wocao":
                    word = "easter";
                    break;
                case "home":
                    break;
                case "teleport":
                    word = "teleport";
                    break;
                default:
                    word = "";
                    break;
            }
            /* if (castTimer > 0.6f && (word == "earth" || word == "water" || word == "wind"))
             {
                 castOn = false;
                 castTimer = 0;
             }*/
            if (castTimer > 0.5f && word != "")
            {
                castOn = false;
                castTimer = 0;
            }
        }

        //if player says "vivi", check for movement
        else if (moveOn)
        {
            if (word == "Vivi0" || word == "Bibi0" || word == "VV0" || word == "BB0" || word == "Wiwi0"
                || word == "Riri0" || word == "Lily0" || word == "Bwibwi0") { word = ""; }
            if (word.Contains("0")) { word = word.Replace("0", ""); }
            castTimer += Time.deltaTime;
            switch (word)
            {
                case "move left":
                    break;
                case "left":
                    break;
                case "walk left":
                    break;
                case "run left":
                    break;
                case "move right":
                    break;
                case "right":
                    break;
                case "walk right":
                    break;
                case "run right":
                    break;
                case "jump":
                    break;
                case "climb":
                    break;
                case "turn":
                    break;
                case "bitch":
                    word = "easter";
                    break;
                case "shit":
                    word = "easter";
                    break;
                case "oh shit":
                    word = "easter";
                    break;
                case "fuck":
                    word = "easter";
                    break;
                case "damn":
                    word = "easter";
                    break;
                case "harlot":
                    word = "easter";
                    break;
                case "motherfucker":
                    word = "easter";
                    break;
                case "what the fuck":
                    word = "easter";
                    break;
                case "wocao":
                    word = "easter";
                    break;
                default:
                    word = "";
                    break;
            }
            if (castTimer > .5f && word != "")
            {
                moveOn = false;
                castTimer = 0;
            }
        }

        //checks if player said spellbook stuff
        else if (!speaking && !isNewSpell)
        {
            if (word.Contains("0")) { word = word.Replace("0", ""); }
            //castTimer += Time.deltaTime;
            switch (word)
            {
                case "open spellbook":
                    viviAdvice.GetComponent<Image>().enabled = false;
                    viviAdvice.GetComponentInChildren<Text>().text = "";
                    viviInput.GetComponent<Image>().sprite = viviSilent;
                    break;
                case "close spellbook":
                    //viviAdvice.GetComponent<Image>().enabled = true;
                    break;
                case "open":
                    word = "open spellbook";
                    viviAdvice.GetComponent<Image>().enabled = false;
                    viviAdvice.GetComponentInChildren<Text>().text = "";
                    viviInput.GetComponent<Image>().sprite = viviSilent;
                    break;
                case "close":
                    word = "close spellbook";
                    //viviAdvice.GetComponent<Image>().enabled = true;
                    break;
                case "next":
                    break;
                case "back":
                    break;
                case "main menu":
                    break;
                case "spells":
                    break;
                case "diary":
                    break;
                case "quit":
                    break;
                case "yes":
                    break;
                case "no":
                    break;
                default:
                    word = "";
                    break;
            }
        }

        //Debug.Log(word);
        //Get the Volume number.
        //float[] samples = new float[aud.clip.samples];
        //aud.clip.GetData(samples, 0);
        //Vector3[] posx = new Vector3[line.positionCount];
        //i = 1;
        //value = 0;
        //int divide = ((samples.Length - 1) / 64) + 1;

        //while (i < samples.Length - 1)
        //{
        //    if (i % 200 != 0)
        //    {
        //        posx[i / divide] = new Vector3((0.0005f * i), samples[i], 0);
        //        line.SetPositions(posx);

        //    }
        //    value += (samples[i] * samples[i]);
        //    ++i;
        //}

        //value = Mathf.Sqrt(value / i) * 25 + 0.5f;

        //Let's the game know which spell is currently being casted
        //Check the microphone volume value to determine effects (Need more tests to find a resonable value, current: 0.015).

        //Testing purposes; input spells with keyboard
        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    word = "wind";
        //    pitch = 1f;
        //}

        //if (Input.GetKeyDown(KeyCode.KeypadPlus))
        //{
        //    wordPnts += 100;
        //}

        //if (Input.GetKeyDown(KeyCode.KeypadMinus))
        //{
        //    wordPnts -= 100;
        //}

        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    word = "fire";
        //    pitch = 1f;
        //}

        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    word = "earth";
        //    pitch = 1f;
        //}

        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    word = "restore";
        //    pitch = 1f;
        //}

        //if (Input.GetKeyDown(KeyCode.V))
        //{
        //    word = "water";
        //    pitch = 1f;
        //}

        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    word = "ice";
        //    pitch = 1f;
        //}

        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    word = "shrink";
        //    pitch = 1f;
        //}

        //if (Input.GetKeyDown(KeyCode.M))
        //{
        //    word = "grow";
        //    pitch = 1f;
        //}

        //if (Input.GetKeyDown(KeyCode.J))
        //{
        //    word = "astral";
        //    pitch = 1f;
        //}

        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    word = "hint";
        //    pitch = 1f;
        //}
        //if (Input.GetKeyDown(KeyCode.Y))
        //{
        //    //word = "yes";
        //    word = "speak";
        //}

        //turns on new spell animation
        if (typeCheck)
        {
            type.Add(word);
            isNewSpell = true;
            typeCheck = false;
        }

        //When timer runs out, turn off current spell
        if (clock >= timer)
        {
            clock = 0.0f;
            word = "";
        }

    }

    private void OnApplicationQuit()
    {
        if (recognizer != null && recognizer.IsRunning)
        {
            recognizer.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
            recognizer.Stop();
        }
    }
    //Prompts text on what the player said
    //   Original code by: Michail Moiropoulos
    private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        if (args.text == "cast" || args.text == "Vivi" || castOn || moveOn || word == "" || speaking)
        {
            if (speaking) { word = args.text; }
            else { word = args.text + "0"; }
            feedback = word;
            Debug.Log(word);
            pitch = value;
            if (pitch > 2f) { pitch = 2f; }
            else if (pitch < 1f) { pitch = 1f; }
            //results.text = "You casted: <b>" + word + "</b>";
            clock = 0f;
            pointCheck = true;
        }
        //Debug.Log (value);
    }

    //displays the title of level in the beginning for a few seconds 
    private void titleEffect()
    {
        titleTimer += Time.deltaTime;
        if (titleTimer >= 8)
        {
            results.color = new Vector4(results.color.r, results.color.g, results.color.b, results.color.a - Time.deltaTime / 5);
        }

        else if (titleTimer >= 2)
        {
            results.color = new Vector4(results.color.r, results.color.g, results.color.b, results.color.a + Time.deltaTime / 5);
        }

        else
        {
            results.color = new Vector4(.91f, .62f, .11f, 0f);
        }
    }

    //These 2 functions will inform the player of new spells as they discover them
    private void discover(string spell)
    {
        //string[] words = spell.Split(' ');
        for (int i = 0; i < discovered.Count; i++)
        {
            if (spell.ToUpper() == discovered[i]) {
                isNewSpell = false;
                return;
            }
        }
        
        discovered.Add(spell.ToUpper());
        called.Add(0);
        typeCheck = true;
    }

    private void discoverEffect()
    {
        discoverTimer += Time.unscaledDeltaTime;
        if (discoverTimer >= 4)
        {
            newSpell.color = new Vector4(newSpell.color.r, newSpell.color.g, newSpell.color.b, newSpell.color.a - Time.unscaledDeltaTime);
        }

        else if (discoverTimer >= 1)
        {
            newSpell.color = new Vector4(newSpell.color.r, newSpell.color.g, newSpell.color.b, newSpell.color.a + Time.unscaledDeltaTime);
        }

        else
        {
            newSpell.color = new Vector4(.3f, 0f, 1f, 0f);
        }
    }

    //handles all the distribution of word points and decrements word values overtime
    public void PointSystem(int worth, string spell)
    {
        //worth++;
        if (spell != "page")
        {
            discover(spell);
            viviInput.GetComponent<Image>().color = new Vector4(.25f, .25f, .25f, 1f);
            castTimer = 0.0f;
            int p = 0;
            for (int i = 0; i < discovered.Count; i++)
            {
                if (spell.ToUpper() == discovered[i])
                {
                    called[i] += 1;
                    p = called[i] - 1;
                }
            }

            worth -= p;

            if (worth <= 0)
            {
                wordPnts += 0;
                if (typeCheck) { newSpell.text = "You discovered a new spell: " + spell + "!"; }
                else
                {
                    newSpell.text = "You cast: " + spell;
                }
            }
            else
            {
                wordPnts += worth * 100;
                if (typeCheck) { newSpell.text = "You discovered a new spell: " + spell + "!"; }
                else
                {
                    newSpell.text = "You cast: " + spell + ", worth: +" + worth * 100 + " points";
                }
            }
            discoverTimer = 0f;
            castOn = false;
            pointCheck = false;
        }

        else
        {
            discoverTimer = 0f;
            newSpell.text = "You found a diary page!";
        }
    }
}
