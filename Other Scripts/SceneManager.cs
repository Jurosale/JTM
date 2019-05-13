using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

class SceneManager : MonoBehaviour
{

    float clock;
    bool fadeOut, newScene;
    Text loading;
    SpriteRenderer transition;
    GameObject LoadAnim, music;

    void Start()
    {
        //sets the height and width all screens will use
        int dimx = 1536;
        int dimy = 963;
        float unityScaleX = dimx / 1536f;
        float unityScaleY = dimy / 963f;

        //Spirit Interaction stuff
        fadeOut = true;
        newScene = true;
        clock = 0f;

        //this and everything below will adjust the screen resolution according to the user's device
        Screen.SetResolution(dimx, dimy, true, 60);

        //initalizes the universial dimensions for all ui and objects in the scene
        //resets all the ui stuff for all scenes
        GameObject.Find("SpellBook").transform.localScale = new Vector3(Screen.width / 1536f * 1.6f * unityScaleX,
            Screen.height / 963f * 2.4f * unityScaleY, 1f);
        GameObject.Find("voice").transform.localScale = new Vector3(0f, 0f, 0f);
        GameObject.Find("ViviInput").transform.localScale = new Vector3(Screen.width / 1536f * 2f * unityScaleX,
            Screen.height / 963f * 2.15f * unityScaleY, 1f);
        GameObject.Find("ViviPortrait").transform.localScale = new Vector3(Screen.width / 320f * unityScaleX,
            Screen.height / 150f * unityScaleY, 1f);
        GameObject.Find("NPCPortrait").transform.localScale = new Vector3(Screen.width / 320f * unityScaleX,
            Screen.height / 150f * unityScaleY, 1f);
        GameObject.Find("DialogueBox").transform.localScale = new Vector3(Screen.width / 75f * unityScaleX,
            Screen.height / 180f * unityScaleY, 1f);
        GameObject.Find("Points").transform.localScale = new Vector3(Screen.width / 1536f * 1.5f * unityScaleX,
            Screen.height / 963f * 1.5f * unityScaleY, 1f);
        GameObject.Find("New Spell").transform.localScale = new Vector3(Screen.width / 1536f * 1.5f * unityScaleX,
            Screen.height / 963f * 1.5f * unityScaleY, 1f);
        GameObject.Find("Title Scene").transform.localScale = new Vector3(Screen.width / 1536f * 1.5f * unityScaleX,
            Screen.height / 963f * 1.5f * unityScaleY, 1f);
        GameObject.Find("HistoryBubble").transform.localScale = new Vector3(Screen.width / 1536f * 4f * unityScaleX,
            Screen.height / 963f * 2.5f * unityScaleY, 1f);
        GameObject.Find("UIH1").transform.localScale = new Vector3(Screen.width / 1536f * .9375f * unityScaleX,
            Screen.height / 963f * 1.5f * unityScaleY, 1f);
        GameObject.Find("ViviKeyword_UI").transform.localScale = new Vector3(Screen.width / 1536f * 1.5f * unityScaleX,
            Screen.height / 963f * 1.5f * unityScaleY, 1f);
        GameObject.Find("UISpellManager").transform.localScale = new Vector3(Screen.width / 1536f * 1.2f * unityScaleX,
            Screen.height / 963f * 1.5f * unityScaleY, 1f);
        GameObject.Find("DiscoverSpells").transform.localScale = new Vector3(Screen.width / 1536f * 1.7f * unityScaleX,
            Screen.height / 963f * 1.7f * unityScaleY, 1f);
        GameObject.Find("UIIcon").transform.localScale = new Vector3(Screen.width / 1536f * 2.1f * unityScaleX,
            Screen.height / 963f * 2.625f * unityScaleY, 1f);
        GameObject.Find("UIIconMenu").transform.localScale = new Vector3(Screen.width / 1536f * 2.1f * unityScaleX,
            Screen.height / 963f * 2.625f * unityScaleY, 1f);
        GameObject.Find("Por_Back2").transform.localScale = new Vector3(Screen.width / 1536f * -24f * unityScaleX,
            Screen.height / 963f * 0f * unityScaleY, 1f);
        GameObject.Find("Por_Next2").transform.localScale = new Vector3(Screen.width / 1536f * 24f * unityScaleX,
            Screen.height / 963f * 0f * unityScaleY, 1f);

        //Turns off most UI components when game starts
        GameObject.Find("ViviPortrait").GetComponent<Image>().enabled = false;
        GameObject.Find("NPCPortrait").GetComponent<Image>().enabled = false;
        GameObject.Find("DialogueBox").GetComponent<Image>().enabled = false;

        GameObject.Find("Por_Next2").GetComponent<Image>().enabled = false;
        GameObject.Find("Por_Back2").GetComponent<Image>().enabled = false;
        GameObject.Find("PorText_Next2").GetComponent<Text>().enabled = false;
        GameObject.Find("PorText_Back2").GetComponent<Text>().enabled = false;

        //Turns off Loading screen
        transition = GameObject.Find("MainMenuColor").GetComponent<SpriteRenderer>();
        loading = GameObject.Find("New Spell").GetComponent<Text>();
        LoadAnim = GameObject.Find("Load Animation");
        transition.color = new Vector4(transition.color.r, transition.color.g, transition.color.b, 1f);
        loading.text = "";
        loading.color = new Vector4(.3f, 0f, 1f, 0f);
        LoadAnim.GetComponent<SpriteRenderer>().color = new Vector4(LoadAnim.GetComponent<SpriteRenderer>().color.r,
            LoadAnim.GetComponent<SpriteRenderer>().color.g, LoadAnim.GetComponent<SpriteRenderer>().color.b, 0f);
        LoadAnim.SetActive(false);

        //other non UI stuff
        music = GameObject.Find("Music Manager");
    }

    void Update()
    {
        //screen goes from black to normal when scene loads
        if (clock < 2f) { clock += Time.deltaTime; }
        if (fadeOut)
        {
            transition.color = new Vector4(transition.color.r, transition.color.g, transition.color.b, transition.color.a - Time.deltaTime / 2);
            if (transition.color.a <= 0)
            {
                transition.color = new Vector4(transition.color.r, transition.color.g, transition.color.b, 0f);
                fadeOut = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //saves current scores to next scene while loading next scene
        if (clock >= 2f && newScene)
        {
            newScene = false;
            GameObject.Find("Variables").GetComponent<UniversialVariables>().GetVariables();
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        //when player reaches end of level, new level loads while the scene blackens and all ui stuff disappears
        if (collision.tag == "Player")
        {
            if (clock >= 2f && transition.color.a < 1f)
            {
                //screen blackens
                transition.color = new Vector4(transition.color.r, transition.color.g, transition.color.b, transition.color.a + Time.deltaTime / 2);

                //UI disappears
                if (transition.color.a >= 1f)
                {
                    GameObject.Find("Witch character").transform.localScale = new Vector3(1f, 1f, 1f);
                    LoadAnim.SetActive(true);
                    GameObject.Find("Points").GetComponent<Text>().color = new Vector4(0f, 0f, 0f, 0f);
                    GameObject.Find("Title Scene").GetComponent<Text>().color = new Vector4(0f, 0f, 0f, 0f);
                    GameObject.Find("UIIconMenu").GetComponent<Image>().color = new Vector4(0f, 0f, 0f, 0f);
                    GameObject.Find("UIIcon").GetComponent<Image>().color = new Vector4(0f, 0f, 0f, 0f);
                    GameObject.Find("ViviInput").GetComponent<Image>().color = new Vector4(0f, 0f, 0f, 0f);
                    GameObject.Find("ViviAdvice").GetComponent<Image>().color = new Vector4(0f, 0f, 0f, 0f);
                    GameObject.Find("SayWord").GetComponent<Text>().color = new Vector4(0f, 0f, 0f, 0f);
                    GameObject.Find("HistoryBubble").GetComponent<Image>().color = new Vector4(0f, 0f, 0f, 0f);
                    GameObject.Find("UISpellManager").GetComponent<RectTransform>().localScale = new Vector3(0f, 0f, 0f);
                    GameObject.Find("UIH1").GetComponent<RectTransform>().localScale = new Vector3(0f, 0f, 0f);
                    GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().enabled = false;
                }
            }

            //UI text changes to indicate next level is loading
            if (transition.color.a > 1f && loading.color.a < 1f)
            {
                music.GetComponent<MusicManager>().StartCoroutine("KillMusic");
                loading.text = "Loading...               ";
                loading.color = new Vector4(loading.color.r, loading.color.g, loading.color.b, loading.color.a + Time.deltaTime / 2);
            }

            //Plays loading screen
            if (transition.color.a > 1f && LoadAnim.GetComponent<SpriteRenderer>().color.a < 1f)
            {
                LoadAnim.GetComponent<SpriteRenderer>().color = new Vector4(LoadAnim.GetComponent<SpriteRenderer>().color.r, LoadAnim.GetComponent<SpriteRenderer>().color.g,
                    LoadAnim.GetComponent<SpriteRenderer>().color.b, LoadAnim.GetComponent<SpriteRenderer>().color.a + Time.deltaTime / 2);
            }
        }
    }
}