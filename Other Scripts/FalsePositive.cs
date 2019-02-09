using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class FalsePositive : MonoBehaviour
{

    string[] Fakekeywords;
    ConfidenceLevel Fakeconfidence = ConfidenceLevel.Low;                     //How well the speech recognition works regarding accents, etc (low = can understand more accents)
    PhraseRecognizer Fakerecognizer;

    SpeechRecognition01 speech;
    SpellBook spellbook;
    SpriteRenderer book;
    Movement moves;
    UIHistory uiH;

    string issue, command, Fakeword;
    float clock, timer;
    bool tip, fake;

    public Image playerbubble;
    public Text advice;

    // Use this for initialization
    void Start()
    {
        //listens for additional fake words alongside the current keywords
        Fakekeywords = new string[] {
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

            "move left", "left", "walk left", "run left", "move right", "right", "walk right", "run left", "jump", "climb", "turn"
        };

        for (int i = 0; i < Fakekeywords.Length - 11; i++)
        {
            Fakekeywords[i] = "CAST " + Fakekeywords[i];
        }

        for (int i = 0; i < 11; i++)
        {
            Fakekeywords[Fakekeywords.Length - (1 + i)] = "VIVI " + Fakekeywords[Fakekeywords.Length - (1 + i)];
        }

        if (Fakekeywords != null)
        {
            Fakerecognizer = new KeywordRecognizer(Fakekeywords, Fakeconfidence);
            Fakerecognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
            Fakerecognizer.Start();
        }
        spellbook = GameObject.Find("SpellBook").GetComponent<SpellBook>();
        book = GameObject.Find("spellbook").GetComponent<SpriteRenderer>();
        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
        moves = GameObject.Find("Witch character").GetComponent<Movement>();
        uiH = GameObject.Find("UIH1").GetComponent<UIHistory>();

        tip = false;
        fake = false;
        playerbubble.enabled = false;
        command = "";
        timer = 0f;

        int dimx = 1536;
        int dimy = 963;
        float unityScaleX = dimx / 1536F;
        float unityScaleY = dimy / 963F;

        GameObject.Find("PlayerUI").transform.localScale = new Vector3(Screen.width / 1536f * -0.65f * unityScaleX, Screen.height / 963f * 0.87f * unityScaleY, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < 5f)
        {
            timer += Time.deltaTime;
        }

        //if player says either a real word or a fake word, check to see what vivi's ui thing should say
        if (speech.feedback != "" || Fakeword != "")
        {
            if (speech.feedback.Contains("0"))
            {
                command = speech.feedback.Replace("0", "");
                speech.feedback = "";
                issue = Check(command);
                DoTheThing(issue);
            }
            else if (Fakeword != "")
            {
                command = Fakeword;
                Fakeword = "";
                issue = Check(command);
                DoTheThing(issue);
            }
        }

        //once tip is displayed, turn off after a few seconds or if a specific condition is met
        if (tip)
        {
            clock += Time.deltaTime;
            if (clock >= 5f || uiH.isSpell || (book.color.a == 0 && spellbook.isOpen) || !speech.castOn && !speech.moveOn && !fake)
            {
                uiH.isSpell = false;
                tip = false;
                fake = false;
                clock = 0f;
                playerbubble.enabled = false;
                advice.text = "";
            }
        }
    }

    //checks to see which word was said
    private string Check(string word)
    {
        if (timer < 2f) { return "none"; }

        if (word.Contains("CAST ") && book.color.a == 0 && !speech.moveOn) { return "pauseCast"; }
        
        if (word.Contains("VIVI ") && !speech.castOn) { return "pauseVivi"; }

        if (word == "cast" && book.color.a == 0 && !speech.moveOn)
        {
            return "nowCast";
        }

        for (int i = 11; i < 19; i++)
        {
            if (word == speech.keywords[speech.keywords.Length - (1 + i)] && !speech.castOn)
            {
                return "nowVivi";
            }
        }

        for (int i = 0; i < 101; i++)
        {
            if (word == speech.keywords[i] && book.color.a == 0 && !speech.moveOn)
            {
                return "cast";
            }
        }

        for (int i = 0; i < 11; i++)
        {
            if (word == speech.keywords[speech.keywords.Length - (1 + i)] && !speech.castOn)
            {
                return "vivi";
            }
        }

        return "none";
    }

    //if word was...
    void DoTheThing(string action)
    {
        tip = true;
        clock = 0f;

        switch (action)
        {
            //...a spell, say this...
            case "cast":
                advice.text = "need to say\n\"cast\" first";
                playerbubble.enabled = true;
                fake = true;
                break;
            //...a movement, say this...
            case "vivi":
                advice.text = "need to say\n\"Vivi\" first";
                playerbubble.enabled = true;
                fake = true;
                break;
            //..."cast", say this...
            case "nowCast":
                advice.text = "now say\na spell";
                playerbubble.enabled = true;
                break;
            //..."vivi", say this...
            case "nowVivi":
                advice.text = "now say\nan action";
                playerbubble.enabled = true;
                break;
            //..."cast" but said too fast, say this...
            case "pauseCast":
                advice.text = "need to say\n\"cast\" then pause\nfor a moment";
                playerbubble.enabled = true;
                fake = true;
                break;
            //..."vivi" but said too fast, say this...
            case "pauseVivi":
                advice.text = "need to say\n\"vivi\" then pause\nfor a moment";
                playerbubble.enabled = true;
                fake = true;
                break;
            case "none":
                break;
        }
    }

    private void OnApplicationQuit()
    {
        if (Fakerecognizer != null && Fakerecognizer.IsRunning)
        {
            Fakerecognizer.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
            Fakerecognizer.Stop();
        }
    }

    //Prompts text on what the player said
    private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        Fakeword = args.text;
    }
}
