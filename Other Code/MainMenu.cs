//Original code by: Michail Moiropoulos
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;

/* ********************************************
 *      Listens for player's voice and 
 *      let's game know which spell is
 *      being casted
*********************************************** */

public class MainMenu : MonoBehaviour
{

    // Variables for Speech recognition.
    public string[] keywords;

    public ConfidenceLevel confidence = ConfidenceLevel.Low;                     //How well the speech recognition works regarding accents, etc (low = can understand more accents)
    private float timer, clock, uiClock, uiEffect, n;
    
    protected PhraseRecognizer recognizer;                                          //Recognizer (not sure what it does)
    private string word;
    private int stage, level;
    private bool check, trans;

    public Text instruct, option1, option2, option3;
    private SpriteRenderer bg;

    public GameObject musicManagerObj;
    MusicManager musicManager;

    // Use this for initialization
    //sets up the main menu and listens for specific keywords
    void Start()
    {

        musicManager = musicManagerObj.GetComponent<MusicManager>();
        keywords = new string[] {
            "new game", "continue", "scores", "yes", "no", "stats", "forest", "city", "lake", "return"
        };

        timer = 0.5f;

        if (keywords != null)
        {
            recognizer = new KeywordRecognizer(keywords, confidence);
            recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
            recognizer.Start();
        }

        n = 1;
        stage = 0;
        level = 0;
        check = true;
        trans = false;
        bg = GameObject.Find("MainMenu_Ver01").GetComponent<SpriteRenderer>();

        int dimx = 1536;
        int dimy = 963;
        float unityScaleX = dimx / 1536;
        float unityScaleY = dimy / 963;

        GameObject.Find("Instruct").transform.localScale = new Vector3(Screen.width / 1536f * 1.5f * unityScaleX, Screen.height / 963f * 1.5f * unityScaleY, 1f);
        GameObject.Find("Option1").transform.localScale = new Vector3(Screen.width / 1536f * 1.5f * unityScaleX, Screen.height / 963f * 1.5f * unityScaleY, 1f);
        GameObject.Find("Option2").transform.localScale = new Vector3(Screen.width / 1536f * 1.5f * unityScaleX, Screen.height / 963f * 1.5f * unityScaleY, 1f);
        GameObject.Find("Option3").transform.localScale = new Vector3(Screen.width / 1536f * 1.5f * unityScaleX, Screen.height / 963f * 1.5f * unityScaleY, 1f);
        //bg.transform.localScale = new Vector3(Screen.width / 1536f * 1.8f * unityScaleX, Screen.height / 963f * 1.5f * unityScaleY, 1f);
        bg.transform.localScale = new Vector3(Screen.width / 1536f * 1.7f * unityScaleX, Screen.height / 963f * 1.7f * unityScaleY, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        //clock += Time.deltaTime;
        //uiClock += Time.deltaTime*n;
        //if (uiClock >= 1.99f) { n = -1; uiClock = 1.99f; }
        //else if (uiClock <= 0) { n = 1; uiClock = 0f; }

        //uiEffect = (uiClock % 2);
        //instruct.color = new Vector4(instruct.color.r, instruct.color.g, instruct.color.b, uiEffect/2);

        if (clock > timer)
        {
            word = "";
        }

        //at the start, player can say "new game", "continue", and "scores"
        if (stage == 0)
        {
            switch (word)
            {
                case "new game":
                    instruct.text = "Would you like to start a new game?";
                    option1.text = "\"Yes\"";
                    option2.text = "";
                    option3.text = "\"No\"";
                    stage = 1;
                    break;
                case "continue":
                    instruct.text = "Which level would you like to load or say \"return\" to cancel.";
                    option1.text = "\"Forest\"";
                    option2.text = "\"City\"";
                    option3.text = "\"Lake\"";
                    stage = 2;
                    break;
                case "scores":
                    GameObject.Find("High Scores").GetComponent<Highscores>().UpdateScores();
                    instruct.text = "Highest scores. Say \"return\" to cancel.";
                    option1.text = "Score: " + GameObject.Find("High Scores").GetComponent<Highscores>().highestScore.ToString() + " pts";
                    option2.text = "Pages: " + GameObject.Find("High Scores").GetComponent<Highscores>().highestPages.ToString() + " / 5";
                    option3.text = "Spells: " + GameObject.Find("High Scores").GetComponent<Highscores>().highestSpells.ToString() + " / 101";
                    stage = 3;
                    break;
                default:
                    word = "";
                    break;
            }
        }

        //asks player if they would like to start a new game
        else if (stage == 1)
        {
            switch (word)
            {
                case "yes":
                    level = 1;
                    trans = true;
                    musicManager.StartCoroutine("KillMusic");
                    break;
                case "no":
                    instruct.text = "Say one of these below to begin:";
                    option1.text = "\"New Game\"";
                    option2.text = "\"Continue\"";
                    option3.text = "\"Scores\"";
                    stage = 0;
                    break;
                default:
                    word = "";
                    break;
            }
        }

        //asks player which level they would like to continue to
        else if (stage == 2)
        {
            switch (word)
            {
                case "forest":
                    level = 2;
                    trans = true;
                    musicManager.StartCoroutine("KillMusic");
                    break;
                case "city":
                    level = 3;
                    trans = true;
                    musicManager.StartCoroutine("KillMusic");
                    break;
                case "lake":
                    level = 4;
                    trans = true;
                    musicManager.StartCoroutine("KillMusic");
                    break;
                case "return":
                    instruct.text = "Say one of these below to begin:";
                    option1.text = "\"New Game\"";
                    option2.text = "\"Continue\"";
                    option3.text = "\"Scores\"";
                    stage = 0;
                    break;
                default:
                    word = "";
                    break;
            }
        }

        //tells player to "return" after seeing scores 
        else if (stage == 3)
        {
            switch (word)
            {
                case "return":
                    instruct.text = "Say one of these below to begin:";
                    option1.text = "\"New Game\"";
                    option2.text = "\"Continue\"";
                    option3.text = "\"Scores\"";
                    stage = 0;
                    break;
                default:
                    word = "";
                    break;
            }
        }

        //transitions to game
        if (trans)
        {
            instruct.text = "";
            option1.text = "";
            option2.text = "";
            option3.text = "";
            stage = 10;
            if (bg.color.r > 0)
            {
                bg.color = new Vector4(bg.color.r - Time.deltaTime, bg.color.g - Time.deltaTime, bg.color.b - Time.deltaTime, bg.color.a);
            }
            else if (check)
            {
                instruct.text = "Loading...";
                SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + level, LoadSceneMode.Single);
                check = false;
            }
        }
    }

    //Let's the game know which spell is currently being casted
    //Check the microphone volume value to determine effects (Need more tests to find a resonable value, current: 0.015).
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
        word = args.text;
        //Debug.Log(word);
        clock = 0f;
    }

}