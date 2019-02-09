using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.Speech;

public class SpellBook : MonoBehaviour
{

    //UI Speech Images
    public GameObject speechRec;
    public Image castFire;
    public Image castWater;
    public Image castWind;
    public Image castEarth;
    public Image castIce;
    public Image castRestore;
    public Image castShrink;
    public Image castGrow;
    public Image castAstral;
    public Image castHint;
    public Image castSpirit;

    public Image castFire2;
    public Image castWater2;
    public Image castWind2;
    public Image castEarth2;
    public Image castIce2;
    public Image castRestore2;
    public Image castShrink2;
    public Image castGrow2;
    public Image castHint2;

    public Text spells;
    public Text spellTotal;
    public Text description;
    public Text mainMenu;
    private int numSpells, counter;

    private List<string> discovered, type;
    private List<int> called;

    //Spellbook icon and book
    public Image bookIcon;
    public Image bookIconMenu;
    public Image Back;
    public Image Next;
    public GameObject spellbookMenu;
    public GameObject flip_obj;
    public GameObject MainMenuColor;

    public Sprite Spellbook1;
    public Sprite Spellbook2;
    public Sprite Spellbook3;

    //number of different words for each spell
    private int numFire, numWater, numIce, numWind, numEarth, numShrink, numGrow, numAstral, numHint, numRestore, numSpirit;
    public bool isOpen;
    public int pages;
    public int numPages;
    private int diaryPage;
    private int lastSpell;
    private bool isQuit;
    private float addTimer;

    Vector2 initPos, bookPos;
    SpeechRecognition01 speech;
    GameObject se, DS, avatar;

    //Discover Animation
    Animator discoverAnim;
    Image discoverImage;

    // Use this for initialization
    void Start()
    {
        //discover Animation
        discoverAnim = GameObject.Find("DiscoverPS").GetComponent<Animator>();
        discoverImage = GameObject.Find("DiscoverPS").GetComponent<Image>();
        discoverAnim.enabled = false;
        discoverImage.enabled = false;

        numPages = GameObject.Find("Variables").GetComponent<UniversialVariables>().currPages;
        pages = GameObject.Find("Variables").GetComponent<UniversialVariables>().pages;
        if (pages == 0) { pages = 1; }
        lastSpell = 0;

        //Make sure it turns off in beginning of game
        castFire.enabled = false;
        castWater.enabled = false;
        castIce.enabled = false;
        castWind.enabled = false;
        castEarth.enabled = false;
        castRestore.enabled = false;
        castShrink.enabled = false;
        castGrow.enabled = false;
        castAstral.enabled = false;
        castHint.enabled = false;
        castSpirit.enabled = false;
        Back.GetComponentInChildren<Text>().text = "";
        Back.enabled = false;
        Next.GetComponentInChildren<Text>().text = "";
        Next.enabled = false;
        resetFlags2();

        //turns off menu screen
        MainMenuColor.GetComponent<SpriteRenderer>().color = new Vector4(MainMenuColor.GetComponent<SpriteRenderer>().color.r,
            MainMenuColor.GetComponent<SpriteRenderer>().color.g, MainMenuColor.GetComponent<SpriteRenderer>().color.b, 0f);
        spellbookMenu.SetActive(false);
        bookIconMenu.enabled = false;
        spells.text = "";
        spellTotal.text = "";
        numSpells = 16;

        //for UI pirposes
        numFire = 13; numWater = 11; numIce = 9; numWind = 11; numEarth = 6;
        numShrink = 9; numGrow = 12; numAstral = 15; numHint = 13; numRestore = 11; numSpirit = 6;

        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
        se = GameObject.Find("SoundEffects");
        DS = GameObject.Find("DiscoverSpells");
        avatar = GameObject.Find("Witch character");
        isOpen = false;
        isQuit = false;
        addTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (addTimer < 2f)
        {
            addTimer += Time.deltaTime;
            if (addTimer >= 2f)
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
            }
        }

        //asks the player if they want to quit game
        if (isOpen && speech.word == "quit")
        {
            speech.word = "";
            resetFlags();
            isQuit = true;
            mainMenu.text = "";
            spells.fontSize = 24;
            spellTotal.fontSize = 24;
            description.fontSize = 24;
            spells.text = "\n\n\n\"Yes\" or \"No\"?\n";
            description.text = "\n\n\nAre you sure you want to quit the game?\n";

        }

        if (isQuit && (speech.word == "yes" || speech.word == "no"))
        {
            if (speech.word == "no")
            {
                resetFlags();
                speech.word = "main menu";
            }

            //this is here to reset the game for sammy showcase
            else if (speech.word == "yes")
            {
                speech.word = "";
                Time.timeScale = 1f;
                if (GameObject.Find("Music Manager").GetComponent<MusicManager>().levelThree)
                    GameObject.Find("Music Manager").GetComponent<MusicManager>().FadeWaves();

                GameObject.Find("Music Manager").GetComponent<MusicManager>().StartCoroutine("KillMusic");
                GameObject.Find("Variables").GetComponent<UniversialVariables>().GetVariables();
                GameObject.Find("Variables").GetComponent<Highscores>().UpdateScores();
                GameObject.Find("Variables").GetComponent<Scores>().keepScores();
                SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
            }
        }

        //Opens Menu
        if (speech.castOn && isOpen) { speech.castOn = false; }
        if (speech.moveOn && isOpen) { speech.moveOn = false; }

        if (speechRec.GetComponent<SpeechRecognition01>().isNewSpell)
        {
            speech.castOn = false;
            speech.moveOn = false;

            //plays discover new spell animation
            if (speechRec.GetComponent<SpeechRecognition01>().discoverTimer < 1f)
            {
                resetFlags2();
                discoverAnim.enabled = true;
                discoverImage.enabled = true;
                discoverAnim.Play("DiscoverPS");
                DS.GetComponent<RectTransform>().anchoredPosition = initPos;
                initPos = DS.GetComponent<RectTransform>().anchoredPosition;
                bookPos = GameObject.Find("UIPath").GetComponent<RectTransform>().anchoredPosition;
                switch (GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>()
                    .type[GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().type.Count - 1])
                {
                    case "fire":
                        castFire2.enabled = true;
                        lastSpell = 2;
                        break;
                    case "water":
                        castWater2.enabled = true;
                        lastSpell = 3;
                        break;
                    case "ice":
                        castIce2.enabled = true;
                        lastSpell = 1;
                        break;
                    case "earth":
                        castEarth2.enabled = true;
                        lastSpell = 4;
                        break;
                    case "wind":
                        castWind2.enabled = true;
                        lastSpell = 5;
                        break;
                    case "restore":
                        castRestore2.enabled = true;
                        lastSpell = 0;
                        break;
                    case "shrink":
                        castShrink2.enabled = true;
                        lastSpell = 6;
                        break;
                    case "grow":
                        castGrow2.enabled = true;
                        lastSpell = 7;
                        break;
                    case "hint":
                        castHint2.enabled = true;
                        lastSpell = 9;
                        break;
                    default:
                        break;
                }
            }

            //new spell symbol appears for a few seconds
            else if (speechRec.GetComponent<SpeechRecognition01>().discoverTimer < 4f)
            {
                DS.GetComponent<RectTransform>().anchoredPosition = Vector3.MoveTowards(DS.GetComponent<RectTransform>().anchoredPosition, bookPos, Time.unscaledDeltaTime * 1000f);
                if (Vector2.Distance(DS.GetComponent<RectTransform>().anchoredPosition, bookPos) <= .1f)
                {
                    resetFlags2();
                }
            }

            //new spell symbol travels to book icon
            else if (speechRec.GetComponent<SpeechRecognition01>().discoverTimer > 4f)
            {
                speechRec.GetComponent<SpeechRecognition01>().isNewSpell = false;
                resetFlags2();
            }
        }

        if (speech.word == "open spellbook" || speech.word == "close spellbook")
        {
            type = speech.type;
            discovered = speech.discovered;
            called = speech.called;

            //if close then open it
            if (speech.word == "open spellbook")
            {
                discoverAnim.enabled = false;
                discoverImage.enabled = false;
                numPages = 0;
                diaryPage = 11;
                if (pages % 11 == 0) { numPages++; diaryPage = 15; }
                if (pages % 7 == 0) { numPages++; diaryPage = 14; }
                if (pages % 5 == 0) { numPages++; diaryPage = 13; }
                if (pages % 3 == 0) { numPages++; diaryPage = 12; }
                if (pages % 2 == 0) { numPages++; }

                MainMenuColor.GetComponent<SpriteRenderer>().color = new Vector4(MainMenuColor.GetComponent<SpriteRenderer>().color.r,
                    MainMenuColor.GetComponent<SpriteRenderer>().color.g, MainMenuColor.GetComponent<SpriteRenderer>().color.b, .95f);

                //pauses game
                Time.timeScale = 0f;
                bookIcon.enabled = false;
                bookIconMenu.enabled = true;

                //turns on menu
                isOpen = true;

                transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 1f);


                spellbookMenu.SetActive(true);
                Back.enabled = true;
                Back.GetComponentInChildren<Text>().text = "\"Back\"";
                Next.enabled = true;
                Next.GetComponentInChildren<Text>().text = "\"Next\"";
                mainMenu.fontSize = 32;
                spells.fontSize = 24;
                spellTotal.fontSize = 24;
                description.fontSize = 24;
                spells.text = "\n\n" + (discovered.Count) + " / 101"
                    + " Spells Learned\n\n" + numPages + " / 5 Diary Pages Found";
                description.text = "\n\nSay 'Quit' to exit game\n\nSay 'Close' to close book";
                mainMenu.text = "      Main Menu          In Game Progress ";
                counter = -1;
            }
            //if open then close it
            else if (speech.word == "close spellbook")
            {
                MainMenuColor.GetComponent<SpriteRenderer>().color = new Vector4(MainMenuColor.GetComponent<SpriteRenderer>().color.r,
                    MainMenuColor.GetComponent<SpriteRenderer>().color.g, MainMenuColor.GetComponent<SpriteRenderer>().color.b, 0f);
             
                //pauses game
                Time.timeScale = 1f;
                bookIcon.enabled = true;
                bookIconMenu.enabled = false;
                
                //turns off menu
                isOpen = false;
                spellbookMenu.SetActive(false);
                Back.GetComponentInChildren<Text>().text = "";
                Back.enabled = false;
                Next.GetComponentInChildren<Text>().text = "";
                Next.enabled = false;
                resetFlags();
                mainMenu.text = "";
            }
            speech.word = "";
        }

        //turns page forward and displays the necessary information if available
        if (spellbookMenu.activeInHierarchy && (speech.word == "next" || speech.word == "main menu" 
            || speech.word == "spells" || speech.word == "diary"))
        {
            //moves spellbook to the page the player asked for
            if (speech.word == "main menu") { counter = 16; speech.word = ""; }
            else if (speech.word == "spells") { counter = lastSpell; lastSpell = 0; speech.word = ""; }
            else if (speech.word == "diary") { counter = diaryPage; speech.word = ""; }
            else { counter++; }

            counter = counter % (numSpells + 1);
            flip_obj.SetActive(true);
            flip_obj.SetActive(false);
            speech.word = "";

            if (counter == 0)
            {
                if (type.Contains("restore"))
                {
                    getBook(counter);
                    resetFlags();
                    spells.lineSpacing = .9f;
                    castRestore.enabled = true;
                    int n = 1;
                    for (int i = 0; i < type.Count; i++)
                    {
                        if (type[i] == "restore")
                        {
                            if (called[i] == 1) { spells.text += "" + n++ + ". " + discovered[i] + " (NEW SPELL) \n"; }
                            else { spells.text += "" + n++ + ". " + discovered[i] + "\n"; }
                        }
                    }
                    if ((n - 1) < numRestore) { spellTotal.text += "" + (n - 1) + " / " + numRestore + " spells discovered"; }
                    else { spellTotal.text += " 11 / 11 spells discovered!"; }
                    description.text = "\n\n\n\n\n\nRestores all objects to their inital position & state.";
                }
                else { counter++; }
            }

            if (counter == 1)
            {
                if (type.Contains("ice"))
                {
                    getBook(counter);
                    resetFlags();
                    castIce.enabled = true;
                    int n = 1;
                    for (int i = 0; i < type.Count; i++)
                    {
                        if (type[i] == "ice")
                        {
                            if (called[i] == 1) { spells.text += "" + n++ + ". " + discovered[i] + " (NEW SPELL) \n"; }
                            else { spells.text += "" + n++ + ". " + discovered[i] + "\n"; }
                        }
                    }
                    if ((n - 1) < numIce) { spellTotal.text += "" + (n - 1) + " / " + numIce + " spells discovered"; }
                    else { spellTotal.text += " 9 / 9 spells discovered!"; }
                    description.text = "\n\n\n\n\n\nFreezes nearby bodies of water into solid ice.";
                }
                else { counter++; }
            }
            
            if (counter == 2)
            {
                if (type.Contains("fire"))
                {
                    getBook(counter);
                    resetFlags();
                    spells.lineSpacing = .7f;
                    castFire.enabled = true;
                    int n = 1;
                    for (int i = 0; i < type.Count; i++)
                    {
                        if (type[i] == "fire")
                        {
                            if (called[i] == 1) { spells.text += "" + n++ + ". " + discovered[i] + " (NEW SPELL) \n"; }
                            else { spells.text += "" + n++ + ". " + discovered[i] + "\n"; }
                        }
                    }
                    if ((n - 1) < numFire) { spellTotal.text += "" + (n - 1) + " / " + numFire + " spells discovered"; }
                    else { spellTotal.text += " 13 / 13 spells discovered!"; }
                    description.text = "\n\n\n\n\n\nSets objects on fire and destroys them.";
                }
                else { counter++; }
            }

            if (counter == 3)
            {
                if (type.Contains("water"))
                {
                    getBook(counter);
                    resetFlags();
                    spells.lineSpacing = .9f;
                    castWater.enabled = true;
                    int n = 1;
                    for (int i = 0; i < type.Count; i++)
                    {
                        if (type[i] == "water")
                        {
                            if (called[i] == 1) { spells.text += "" + n++ + ". " + discovered[i] + " (NEW SPELL) \n"; }
                            else { spells.text += "" + n++ + ". " + discovered[i] + "\n"; }
                        }
                    }
                    if ((n - 1) < numWater) { spellTotal.text += "" + (n - 1) + " / " + numWater + " spells discovered"; }
                    else { spellTotal.text += " 11 / 11 spells discovered!"; }
                    description.text = "\n\n\n\n\n\nFills small gaps with a body of water.";
                }
                else { counter++; }
            }

            if (counter == 4)
            {
                if (type.Contains("earth"))
                {
                    getBook(counter);
                    resetFlags();
                    castEarth.enabled = true;
                    int n = 1;
                    for (int i = 0; i < type.Count; i++)
                    {
                        if (type[i] == "earth")
                        {
                            if (called[i] == 1) { spells.text += "" + n++ + ". " + discovered[i] + " (NEW SPELL) \n"; }
                            else { spells.text += "" + n++ + ". " + discovered[i] + "\n"; }
                        }
                    }
                    if ((n - 1) < numEarth) { spellTotal.text += "" + (n - 1) + " / " + numEarth + " spells discovered"; }
                    else { spellTotal.text += " 6 / 6 spells discovered!"; }
                    description.text = "\n\n\n\n\n\nSummon a mountain of rocks for extra reach.";
                }
                else { counter++; }
            }

            if (counter == 5)
            {
                if (type.Contains("wind"))
                {
                    getBook(counter);
                    resetFlags();
                    spells.lineSpacing = .9f;
                    castWind.enabled = true;
                    int n = 1;
                    for (int i = 0; i < type.Count; i++)
                    {
                        if (type[i] == "wind")
                        {
                            if (called[i] == 1) { spells.text += "" + n++ + ". " + discovered[i] + " (NEW SPELL) \n"; }
                            else { spells.text += "" + n++ + ". " + discovered[i] + "\n"; }
                        }
                    }
                    if ((n - 1) < numWind) { spellTotal.text += "" + (n - 1) + " / " + numWind + " spells discovered"; }
                    else { spellTotal.text += " 11 / 11 spells discovered!"; }
                    description.text = "\n\n\n\n\n\nSummon a gust of wind to move objects.";
                }
                else { counter++; }
            }

            if (counter == 6)
            {
                if (type.Contains("shrink"))
                {
                    getBook(counter);
                    resetFlags();
                    castShrink.enabled = true;
                    int n = 1;
                    for (int i = 0; i < type.Count; i++)
                    {
                        if (type[i] == "shrink")
                        {
                            if (called[i] == 1) { spells.text += "" + n++ + ". " + discovered[i] + " (NEW SPELL) \n"; }
                            else { spells.text += "" + n++ + ". " + discovered[i] + "\n"; }
                        }
                    }
                    if ((n - 1) < numShrink) { spellTotal.text += "" + (n - 1) + " / " + numShrink + " spells discovered"; }
                    else { spellTotal.text += " 9 / 9 spells discovered!"; }
                    description.text = "\n\n\n\n\n\nShrink yourself to fit into smaller spaces.";
                }
                else { counter++; }
            }

            if (counter == 7)
            {
                if (type.Contains("grow"))
                {
                    getBook(counter);
                    resetFlags();
                    spells.lineSpacing = .8f;
                    castGrow.enabled = true;
                    int n = 1;
                    for (int i = 0; i < type.Count; i++)
                    {
                        if (type[i] == "grow")
                        {
                            if (called[i] == 1) { spells.text += "" + n++ + ". " + discovered[i] + " (NEW SPELL) \n"; }
                            else { spells.text += "" + n++ + ". " + discovered[i] + "\n"; }
                        }
                    }
                    if ((n - 1) < numGrow) { spellTotal.text += "" + (n - 1) + " / " + numGrow + " spells discovered"; }
                    else { spellTotal.text += " 12 / 12 spells discovered!"; }
                    description.text = "\n\n\n\n\n\nGrow yourself to reach higher places.";
                }
                else { counter++; }
            }

            if (counter == 8)
            {
                if (type.Contains("astral"))
                {
                    getBook(counter);
                    resetFlags();
                    castAstral.enabled = true;
                    int n = 1;
                    for (int i = 0; i < type.Count; i++)
                    {
                        if (type[i] == "astral")
                        {
                            if (called[i] == 1) { spells.text += "" + n++ + ". " + discovered[i] + " (NEW SPELL) \n"; }
                            else { spells.text += "" + n++ + ". " + discovered[i] + "\n"; }
                        }
                    }
                    if ((n - 1) < numAstral) { spellTotal.text += "" + (n - 1) + " / " + numAstral + " spells discovered"; }
                    else { spellTotal.text += " all spells discovered!"; }
                    description.text = "\n\n\n\n\n\nSurvey the area around you.";
                }
                else { counter++; }
            }

            if (counter == 9)
            {
                if (type.Contains("hint"))
                {
                    getBook(counter);
                    resetFlags();
                    spells.lineSpacing = .7f;
                    castHint.enabled = true;
                    int n = 1;
                    for (int i = 0; i < type.Count; i++)
                    {
                        if (type[i] == "hint")
                        {
                            if (called[i] == 1) { spells.text += "" + n++ + ". " + discovered[i] + " (NEW SPELL) \n"; }
                            else { spells.text += "" + n++ + ". " + discovered[i] + "\n"; }
                        }
                    }
                    if ((n - 1) < numHint) { spellTotal.text += "" + (n - 1) + " / " + numHint + " spells discovered"; }
                    else { spellTotal.text += " 13 / 13 spells discovered!"; }
                    description.text = "\n\n\n\n\n\nReceive some helpful advice when you're stuck or confused.";
                }
                else { counter++; }
            }

            if (counter == 10)
            {
                getBook(counter);
                resetFlags();
                castSpirit.enabled = true;
                spells.text += "1. SPEAK\n2. TALK\n3. FOLLOW\n4. COME\n5. PRAY\n6. PRAYER\n";
                spellTotal.text += " 6 / 6 spells discovered!";
                description.text = "\n\n\n\n\n\nInteract with spirits.";
            }

            if (counter == 11)
            {
                getBook(counter);
                resetFlags();
                spells.fontSize = 14;
                description.fontSize = 14;
                if (pages % 2 != 0)
                {
                    mainMenu.text = "                             Entry #1 Missing";
                    description.text = "";
                    spellTotal.text = "";
                    spells.text = "";

                }

                else
                {
                    mainMenu.text = "  Dairy Entry: Page 1        The Apple Tree     ";
                    description.text += "Mother told me about the forest filled with delicious fruits." +
                        " I wanted to get some for Sophie since she just loves apples! When I stepped outside the gates I saw the most" +
                        " beautiful tree, with bright red apples that I knew Sophie would enjoy. But as I tried to get some, an older" +
                        " gentleman shouted at me. It caught me by surprise! I apologized profusely to him, and he laugh when I told him I" +
                        " just wanted to get some fresh fruits for my daughter. He introduced himself as Neil, and he lived in the edge of" +
                        " the forest since the soil was fertile enough to plant his apple trees. He invited me for a cup of tea, told me a bit";
                    spellTotal.text += "";
                    spells.text = "about his family. His grandson planted him that apple tree before he left to become a soldier for our city." +
                        " They would spend the day making apple pies together. He seemed so sad… I suppose I would feel the same if Sophie had" +
                        " to leave me too. Ah...but to think we are already sending our men to war… is the threat of the witches really coming?" +
                        " I couldn’t help but feel a sense of doom as we spoke of it. Neil told me every thing will be okay, everyone will come" +
                        " home and how he will be making pies soon with his grandson again. But I noticed his gaze seemed distant… and filled with dread.";
                }
            }

            if (counter == 12)
            {
                if (pages % 3 == 0)
                {
                    getBook(counter);
                    resetFlags();
                    spells.fontSize = 14;
                    description.fontSize = 14;
                    mainMenu.text = "  Dairy Entry: Page 2     Intuitive Suspicion  ";
                    description.text += "Tonight we had a lovely ceremony by the shrine’s bell to celebrate Her Goddess. For the potluck," +
                        " I brought in my Sophie’s favorite snack, chocolate cookies. Everyone seemed to love it which made me so happy!" +
                        " The cookies and the creamy milk that my friend brought is just the right thing to have during this hot" +
                        " weather. The Holy Head Priestess preached about forgiveness and told me something that I would never forget:" +
                        " “Darkness cannot defeat darkness for only the light can penetrate through”. I always loved that line, especially during ";
                    spellTotal.text += "";
                    spells.text = "these frightening times. Pateran and I discussed about what we would do if" +
                        " the witches came.... And what if one of us died. I wish we don’t need to have this kind of conversation" +
                        " but… it seems like the witches will come eventually. As I thought of these things, I came upon these" +
                        " beautiful vines that I never really noticed before.They were beautiful underneath the moonlight. I always loved" +
                        " plantlife so I wonder why I haven’t seen these before..? Ah… oh well. I can’t wait for the next coming festival" +
                        " for Her Goddess.";
                }
                else
                {
                    mainMenu.text = "                             Entry #2 Missing";
                    description.text = "";
                    spellTotal.text = "";
                    spells.text = "";
                }
            }

            if (counter == 13)
            {
                if (pages % 5 == 0)
                {
                    getBook(counter);
                    resetFlags();
                    spells.fontSize = 14;
                    description.fontSize = 14;
                    mainMenu.text = "  Dairy Entry: Page 3         Hidden Talent      ";
                    description.text += "I can’t believe I didn’t notice this before, but my dear Sophie is starting to have an interest" +
                        " in creating machinery! She’s been tinkering with all kinds of gadgets in our home, from the tiny little moving" +
                        " toys to the huge boiler right outside our door. It’s wonderful to see her so excited! She even made her" +
                        " favorite plush bird tweet for a bit. I’m just so proud of her! But...I’m honestly a little worried. I was" +
                        " discussing with Pateran about this and he assured me she wouldn’t be taken away. The last thing I want is" +
                        " for my young daughter to start creating...weapons. Weapons for ";
                    spellTotal.text += "";
                    spells.text = "the war we all know that is going to happen. Tensions between our people" +
                        " and the witches has been increasing lately. Honestly I’m so scared. The" +
                        " witches are so powerful with their magic… I’m not sure we stand a chance. I know I know. We have tons of great" +
                        " fighters who are able to fight off the witches using their swords and wits. But… is that enough? I’m not sure." +
                        " They have been increasingly calling for more soldiers to be trained and more research for the machines… no" +
                        " matter what the age. What has our life come to?";
                }
                else
                {
                    mainMenu.text = "                             Entry #3 Missing";
                    description.text = "";
                    spellTotal.text = "";
                    spells.text = "";
                }
            }

            if (counter == 14)
            {
                if (pages % 7 == 0)
                {
                    getBook(counter);
                    resetFlags();
                    spells.fontSize = 14;
                    description.fontSize = 14;
                    mainMenu.text = "  Dairy Entry: Page 4      When Time Froze   ";
                    description.text += "Pateran took Sophie and I to the Lake of the Goddess today. It was so beautiful, the water was" +
                        " covered in a thick blanket ice and it reflected the sun quite nicely. It has been cold recently and there was" +
                        " a lot of snow in the last couple of days. Sophie always wanted to learn how to ice skate so we bought her new" +
                        " skates as a present for her birthday. Sophie was quite clumsy at first,  she kept tripping and falling! I was" +
                        " worried she would hurt herself and stop, but she never gave up and she got the hang of it. In fact, she grew" +
                        " so confident that she";
                    spellTotal.text += "";
                    spells.text = "challenged Pateran to a race to see who can reach the buoy that was stuck in the ice!" +
                        " I laughed as they dashed off in lightning speed! I heard my daughter yell “ I won mommy!” and I felt" +
                        " so proud of her! Their laughs filled the air and I can’t help but smile. But how long will these happy days last?" +
                        " I wish it would be forever, but with all the rumors of an impending war coming closer… no I cannot lose hope. It" +
                        " will be alright. That’s what everyone is saying. And I need to believe that.";
                }
                else
                {
                    mainMenu.text = "                             Entry #4 Missing";
                    description.text = "";
                    spellTotal.text = "";
                    spells.text = "";
                }
            }

            if (counter == 15)
            {
                if (pages % 11 == 0)
                {
                    getBook(counter);
                    resetFlags();
                    spells.fontSize = 14;
                    description.fontSize = 14;
                    mainMenu.text = "  Dairy Entry: Page 5    Whisper in the Dark";
                    description.text += "The witches are here. They’ve been here. Everything is covered in ice and if it’s not in ice," +
                        " it’s on fire. Or destroyed. It’s getting harder to write. I’m sorry Pateran. I tried to protect Sophie. I didn’t" +
                        " want your death to be in vain. She is here with me and I can barely move myself. I thought that if I made it to" +
                        " Her Goddess’s sacred tree… everything will be alright. Sophie and I would be safe in her protection. But it’s" +
                        " not. Her beautiful rose garden is in ashes, the tree withering as our city crumbles…My daughter isn’t breathing " +
                        "any more. I can’t see anything. My ";
                    spellTotal.text += "";
                    spells.text = "Goddess… why would this happen? I just wanted to be with my husband and" +
                        "daughter...To anyone reading this please… don’t forget us. I suppose it is my time now to return to my" +
                        " Goddess’s side.";
                }
                else
                {
                    mainMenu.text = "                             Entry #5 Missing";
                    description.text = "";
                    spellTotal.text = "";
                    spells.text = "";
                }
            }

            if (counter == 16)
            {
                getBook(counter);
                resetFlags();
                spells.fontSize = 24;
                spellTotal.fontSize = 24;
                description.fontSize = 24;
                spells.text = "\n\n" + (discovered.Count) + " / 101"
                    + " Spells Learned\n\n" + numPages + " / 5 Diary Pages Found";
                description.text = "\n\nSay 'Quit' to exit game\n\nSay 'Close' to close book";
                mainMenu.text = "      Main Menu          In Game Progress ";
            }
        }

        //turns page backward and displays the necessary information if available
        if (spellbookMenu.activeInHierarchy && speech.word == "back")
        {
            counter--;
            flip_obj.SetActive(true);
            flip_obj.SetActive(false);
            speech.word = "";
            if (counter == -2) { counter = numSpells - 1; }
            if (counter < 0) { counter = numSpells; }

            if (counter == 16)
            {
                getBook(counter);
                resetFlags();
                spells.fontSize = 24;
                spellTotal.fontSize = 24;
                description.fontSize = 24;
                spells.text = "\n\n" + (discovered.Count) + " / 101"
                    + " Spells Learned\n\n" + numPages + " / 5 Diary Pages Found";
                description.text = "\n\nSay 'Quit' to exit game\n\nSay 'Close' to close book";
                mainMenu.text = "      Main Menu          In Game Progress ";
            }

            if (counter == 15)
            {
                if (pages % 11 == 0)
                {
                    getBook(counter);
                    resetFlags();
                    spells.fontSize = 14;
                    description.fontSize = 14;
                    mainMenu.text = "  Dairy Entry: Page 5    Whisper in the Dark";
                    description.text += "The witches are here. They’ve been here. Everything is covered in ice and if it’s not in ice," +
                        " it’s on fire. Or destroyed. It’s getting harder to write. I’m sorry Pateran. I tried to protect Sophie. I didn’t" +
                        " want your death to be in vain. She is here with me and I can barely move myself. I thought that if I made it to" +
                        " Her Goddess’s sacred tree… everything will be alright. Sophie and I would be safe in her protection. But it’s" +
                        " not. Her beautiful rose garden is in ashes, the tree withering as our city crumbles…My daughter isn’t breathing " +
                        "any more. I can’t see anything. My ";
                    spellTotal.text += "";
                    spells.text = "Goddess… why would this happen? I just wanted to be with my husband and" +
                        "daughter...To anyone reading this please… don’t forget us. I suppose it is my time now to return to my" +
                        " Goddess’s side.";
                }
                else
                {
                    mainMenu.text = "                             Entry #5 Missing";
                    description.text = "";
                    spellTotal.text = "";
                    spells.text = "";
                }
            }

            if (counter == 14)
            {
                if (pages % 7 == 0)
                {
                    getBook(counter);
                    resetFlags();
                    spells.fontSize = 14;
                    description.fontSize = 14;
                    mainMenu.text = "  Dairy Entry: Page 4      When Time Froze   ";
                    description.text += "Pateran took Sophie and I to the Lake of the Goddess today. It was so beautiful, the water was" +
                        " covered in a thick blanket ice and it reflected the sun quite nicely. It has been cold recently and there was" +
                        " a lot of snow in the last couple of days. Sophie always wanted to learn how to ice skate so we bought her new" +
                        " skates as a present for her birthday. Sophie was quite clumsy at first,  she kept tripping and falling! I was" +
                        " worried she would hurt herself and stop, but she never gave up and she got the hang of it. In fact, she grew" +
                        " so confident that she";
                    spellTotal.text += "";
                    spells.text = "challenged Pateran to a race to see who can reach the buoy that was stuck in the ice!" +
                        " I laughed as they dashed off in lightning speed! I heard my daughter yell “ I won mommy!” and I felt" +
                        " so proud of her! Their laughs filled the air and I can’t help but smile. But how long will these happy days last?" +
                        " I wish it would be forever, but with all the rumors of an impending war coming closer… no I cannot lose hope. It" +
                        " will be alright. That’s what everyone is saying. And I need to believe that.";
                }
                else
                {
                    mainMenu.text = "                             Entry #4 Missing";
                    description.text = "";
                    spellTotal.text = "";
                    spells.text = "";
                }
            }


            if (counter == 13)
            {
                if (pages % 5 == 0)
                {
                    getBook(counter);
                    resetFlags();
                    spells.fontSize = 14;
                    description.fontSize = 14;
                    mainMenu.text = "  Dairy Entry: Page 3         Hidden Talent      ";
                    description.text += "I can’t believe I didn’t notice this before, but my dear Sophie is starting to have an interest" +
                        " in creating machinery! She’s been tinkering with all kinds of gadgets in our home, from the tiny little moving" +
                        " toys to the huge boiler right outside our door. It’s wonderful to see her so excited! She even made her" +
                        " favorite plush bird tweet for a bit. I’m just so proud of her! But...I’m honestly a little worried. I was" +
                        " discussing with Pateran about this and he assured me she wouldn’t be taken away. The last thing I want is" +
                        " for my young daughter to start creating...weapons. Weapons for "; 
                    spellTotal.text += "";
                    spells.text = "the war we all know that is going to happen. Tensions between our people" +
                        " and the witches has been increasing lately. Honestly I’m so scared. The" +
                        " witches are so powerful with their magic… I’m not sure we stand a chance. I know I know. We have tons of great" +
                        " fighters who are able to fight off the witches using their swords and wits. But… is that enough? I’m not sure." +
                        " They have been increasingly calling for more soldiers to be trained and more research for the machines… no" +
                        " matter what the age. What has our life come to?";
                }
                else
                {
                    mainMenu.text = "                             Entry #3 Missing";
                    description.text = "";
                    spellTotal.text = "";
                    spells.text = "";
                }
            }

            if (counter == 12)
            {
                if (pages % 3 == 0)
                {
                    getBook(counter);
                    resetFlags();
                    spells.fontSize = 14;
                    description.fontSize = 14;
                    mainMenu.text = "  Dairy Entry: Page 2     Intuitive Suspicion  ";
                    description.text += "Tonight we had a lovely ceremony by the shrine’s bell to celebrate Her Goddess. For the potluck," +
                        " I brought in my Sophie’s favorite snack, chocolate cookies. Everyone seemed to love it which made me so happy!" +
                        " The cookies and the creamy milk that my friend brought is just the right thing to have during this hot" +
                        " weather. The Holy Head Priestess preached about forgiveness and told me something that I would never forget:" +
                        " “Darkness cannot defeat darkness for only the light can penetrate through”. I always loved that line, especially during ";
                    spellTotal.text += "";
                    spells.text = "these frightening times. Pateran and I discussed about what we would do if" +
                        " the witches came.... And what if one of us died. I wish we don’t need to have this kind of conversation" +
                        " but… it seems like the witches will come eventually. As I thought of these things, I came upon these" +
                        " beautiful vines that I never really noticed before.They were beautiful underneath the moonlight. I always loved" +
                        " plantlife so I wonder why I haven’t seen these before..? Ah… oh well. I can’t wait for the next coming festival" +
                        " for Her Goddess.";
                }
                else
                {
                    mainMenu.text = "                             Entry #2 Missing";
                    description.text = "";
                    spellTotal.text = "";
                    spells.text = "";
                }
            }

            if (counter == 11)
            {
                getBook(counter);
                resetFlags();
                spells.fontSize = 14;
                description.fontSize = 14;
                if (pages % 2 != 0)
                {
                    mainMenu.text = "                             Entry #1 Missing";
                    description.text = "";
                    spellTotal.text = "";
                    spells.text = "";

                }

                else
                {
                    mainMenu.text = "  Dairy Entry: Page 1        The Apple Tree     ";
                    description.text += "Mother told me about the forest filled with delicious fruits." +
                        " I wanted to get some for Sophie since she just loves apples! When I stepped outside the gates I saw the most" +
                        " beautiful tree, with bright red apples that I knew Sophie would enjoy. But as I tried to get some, an older" +
                        " gentleman shouted at me. It caught me by surprise! I apologized profusely to him, and he laugh when I told him I" +
                        " just wanted to get some fresh fruits for my daughter. He introduced himself as Neil, and he lived in the edge of" +
                        " the forest since the soil was fertile enough to plant his apple trees. He invited me for a cup of tea, told me a bit";
                    spellTotal.text += "";
                    spells.text = "about his family. His grandson planted him that apple tree before he left to become a soldier for our city." +
                        " They would spend the day making apple pies together. He seemed so sad… I suppose I would feel the same if Sophie had" +
                        " to leave me too. Ah...but to think we are already sending our men to war… is the threat of the witches really coming?" +
                        " I couldn’t help but feel a sense of doom as we spoke of it. Neil told me every thing will be okay, everyone will come" +
                        " home and how he will be making pies soon with his grandson again. But I noticed his gaze seemed distant… and filled with dread.";
                }
            }

            if (counter == 10)
            {
                getBook(counter);
                resetFlags();
                castSpirit.enabled = true;
                spells.text += "1. SPEAK\n2. TALK\n3. FOLLOW\n4. COME\n5. PRAY\n6. PRAYER\n";
                spellTotal.text += " 6 / 6 spells discovered!";
                description.text = "\n\n\n\n\n\nInteract with spirits.";
            }

            if (counter == 9)
            {
                if (type.Contains("hint"))
                {
                    getBook(counter);
                    resetFlags();
                    spells.lineSpacing = .7f;
                    castHint.enabled = true;
                    int n = 1;
                    for (int i = 0; i < type.Count; i++)
                    {
                        if (type[i] == "hint")
                        {
                            if (called[i] == 1) { spells.text += "" + n++ + ". " + discovered[i] + " (NEW SPELL) \n"; }
                            else { spells.text += "" + n++ + ". " + discovered[i] + "\n"; }
                        }
                    }
                    if ((n - 1) < numHint) { spellTotal.text += "" + (n - 1) + " / " + numHint + " spells discovered"; }
                    else { spellTotal.text += " 13 / 13 spells discovered!"; }
                    description.text = "\n\n\n\n\n\nReceive some helpful advice when you're stuck or confused.";
                }
                else
                {
                    counter--;
                    counter = counter % numSpells;
                }
            }

            if (counter == 8)
            {
                if (type.Contains("astral"))
                {
                    getBook(counter);
                    resetFlags();
                    castAstral.enabled = true;
                    int n = 1;
                    for (int i = 0; i < type.Count; i++)
                    {
                        if (type[i] == "astral")
                        {
                            if (called[i] == 1) { spells.text += "" + n++ + ". " + discovered[i] + " (NEW SPELL) \n"; }
                            else { spells.text += "" + n++ + ". " + discovered[i] + "\n"; }
                        }
                    }
                    if ((n - 1) < numAstral) { spellTotal.text += "" + (n - 1) + " / " + numAstral + " spells discovered"; }
                    else { spellTotal.text += " all spells discovered!"; }
                    description.text = "\n\n\n\n\n\nSurvey the area around you.";
                }
                else
                {
                    counter--;
                    counter = counter % numSpells;
                }
            }

            if (counter == 7)
            {
                if (type.Contains("grow"))
                {
                    getBook(counter);
                    resetFlags();
                    spells.lineSpacing = .8f;
                    castGrow.enabled = true;
                    int n = 1;
                    for (int i = 0; i < type.Count; i++)
                    {
                        if (type[i] == "grow")
                        {
                            if (called[i] == 1) { spells.text += "" + n++ + ". " + discovered[i] + " (NEW SPELL) \n"; }
                            else { spells.text += "" + n++ + ". " + discovered[i] + "\n"; }
                        }
                    }
                    if ((n - 1) < numGrow) { spellTotal.text += "" + (n - 1) + " / " + numGrow + " spells discovered"; }
                    else { spellTotal.text += " 12 / 12 spells discovered!"; }
                    description.text = "\n\n\n\n\n\nGrow yourself to reach higher places.";
                }
                else
                {
                    counter--;
                    counter = counter % numSpells;
                }
            }

            if (counter == 6)
            {
                if (type.Contains("shrink"))
                {
                    getBook(counter);
                    resetFlags();
                    castShrink.enabled = true;
                    int n = 1;
                    for (int i = 0; i < type.Count; i++)
                    {
                        if (type[i] == "shrink")
                        {
                            if (called[i] == 1) { spells.text += "" + n++ + ". " + discovered[i] + " (NEW SPELL) \n"; }
                            else { spells.text += "" + n++ + ". " + discovered[i] + "\n"; }
                        }
                    }
                    if ((n - 1) < numShrink) { spellTotal.text += "" + (n - 1) + " / " + numShrink + " spells discovered"; }
                    else { spellTotal.text += " 9 / 9 spells discovered!"; }
                    description.text = "\n\n\n\n\n\nShrink yourself to fit into smaller spaces.";
                }
                else
                {
                    counter--;
                    counter = counter % numSpells;
                }
            }


            if (counter == 5)
            {
                if (type.Contains("wind"))
                {
                    getBook(counter);
                    resetFlags();
                    spells.lineSpacing = .9f;
                    castWind.enabled = true;
                    int n = 1;
                    for (int i = 0; i < type.Count; i++)
                    {
                        if (type[i] == "wind")
                        {
                            if (called[i] == 1) { spells.text += "" + n++ + ". " + discovered[i] + " (NEW SPELL) \n"; }
                            else { spells.text += "" + n++ + ". " + discovered[i] + "\n"; }
                        }
                    }
                    if ((n - 1) < numWind) { spellTotal.text += "" + (n - 1) + " / " + numWind + " spells discovered"; }
                    else { spellTotal.text += " 11 / 11 spells discovered!"; }
                    description.text = "\n\n\n\n\n\nSummon a gust of wind to move objects.";
                }
                else
                {
                    counter--;
                    counter = counter % numSpells;
                }
            }

            if (counter == 4)
            {
                if (type.Contains("earth"))
                {
                    getBook(counter);
                    resetFlags();
                    castEarth.enabled = true;
                    int n = 1;
                    for (int i = 0; i < type.Count; i++)
                    {
                        if (type[i] == "earth")
                        {
                            if (called[i] == 1) { spells.text += "" + n++ + ". " + discovered[i] + " (NEW SPELL) \n"; }
                            else { spells.text += "" + n++ + ". " + discovered[i] + "\n"; }
                        }
                    }
                    if ((n - 1) < numEarth) { spellTotal.text += "" + (n - 1) + " / " + numEarth + " spells discovered"; }
                    else { spellTotal.text += " 6 / 6 spells discovered!"; }
                    description.text = "\n\n\n\n\n\nSummon a mountain of rocks for extra reach.";
                }
                else
                {
                    counter--;
                    counter = counter % numSpells;
                }
            }

            if (counter == 3)
            {
                if (type.Contains("water"))
                {
                    getBook(counter);
                    resetFlags();
                    spells.lineSpacing = .9f;
                    castWater.enabled = true;
                    int n = 1;
                    for (int i = 0; i < type.Count; i++)
                    {
                        if (type[i] == "water")
                        {
                            if (called[i] == 1) { spells.text += "" + n++ + ". " + discovered[i] + " (NEW SPELL) \n"; }
                            else { spells.text += "" + n++ + ". " + discovered[i] + "\n"; }
                        }
                    }
                    if ((n - 1) < numWater) { spellTotal.text += "" + (n - 1) + " / " + numWater + " spells discovered"; }
                    else { spellTotal.text += " 11 / 11 spells discovered!"; }
                    description.text = "\n\n\n\n\n\nFills small gaps with a body of water.";
                }
                else
                {
                    counter--;
                    counter = counter % numSpells;
                }
            }

            if (counter == 2)
            {
                if (type.Contains("fire"))
                {
                    getBook(counter);
                    resetFlags();
                    spells.lineSpacing = .7f;
                    castFire.enabled = true;
                    int n = 1;
                    for (int i = 0; i < type.Count; i++)
                    {
                        if (type[i] == "fire")
                        {
                            if (called[i] == 1) { spells.text += "" + n++ + ". " + discovered[i] + " (NEW SPELL) \n"; }
                            else { spells.text += "" + n++ + ". " + discovered[i] + "\n"; }
                        }
                    }
                    if ((n - 1) < numFire) { spellTotal.text += "" + (n - 1) + " / " + numFire + " spells discovered"; }
                    else { spellTotal.text += " 13 / 13 spells discovered!"; }
                    description.text = "\n\n\n\n\n\nSets objects on fire and destroys them.";
                }
                else
                {
                    counter--;
                    counter = counter % numSpells;
                }
            }

            if (counter == 1)
            {
                if (type.Contains("ice"))
                {
                    getBook(counter);
                    resetFlags();
                    castIce.enabled = true;
                    int n = 1;
                    for (int i = 0; i < type.Count; i++)
                    {
                        if (type[i] == "ice")
                        {
                            if (called[i] == 1) { spells.text += "" + n++ + ". " + discovered[i] + " (NEW SPELL) \n"; }
                            else { spells.text += "" + n++ + ". " + discovered[i] + "\n"; }
                        }
                    }
                    if ((n - 1) < numIce) { spellTotal.text += "" + (n - 1) + " / " + numIce + " spells discovered"; }
                    else { spellTotal.text += " 9 / 9 spells discovered!"; }
                    description.text = "\n\n\n\n\n\nFreezes nearby bodies of water into solid ice.";
                }
                else
                {
                    counter--;
                    counter = counter % numSpells;
                }
            }

            if (counter == 0)
            {
                if (type.Contains("restore"))
                {
                    getBook(counter);
                    resetFlags();
                    spells.lineSpacing = .9f;
                    castRestore.enabled = true;
                    int n = 1;
                    for (int i = 0; i < type.Count; i++)
                    {
                        if (type[i] == "restore")
                        {
                            if (called[i] == 1) { spells.text += "" + n++ + ". " + discovered[i] + " (NEW SPELL) \n"; }
                            else { spells.text += "" + n++ + ". " + discovered[i] + "\n"; }
                        }
                    }
                    if (n < numRestore) { spellTotal.text += "" + (n - 1) + " / " + numRestore + " spells discovered"; }
                    else { spellTotal.text += " 11 / 11 spells discovered!"; }
                    description.text = "\n\n\n\n\n\nRestores all objects to their inital position & state.";
                }
            }

        }

    }

    //turns off the light effect of a newly discovered spell
    public void TurnOffDiscoverAnimation ()
    {
        discoverAnim.enabled = false;
        discoverImage.enabled = false;
    }

    //resets text, size and other contents
    public void resetFlags()
    {
        spells.text = "";
        spellTotal.text = "";
        description.text = "";
        mainMenu.fontSize = 32;
        spells.fontSize = 16;
        spells.lineSpacing = 1f;
        spellTotal.fontSize = 20;
        description.fontSize = 20;
        mainMenu.text = "                            Spells you can Cast";
        castFire.enabled = false;
        castWater.enabled = false;
        castIce.enabled = false;
        castWind.enabled = false;
        castEarth.enabled = false;
        castRestore.enabled = false;
        castShrink.enabled = false;
        castGrow.enabled = false;
        castAstral.enabled = false;
        castHint.enabled = false;
        castSpirit.enabled = false;
        isQuit = false;
    }

    //resets discover new spell animation stuff 
    public void resetFlags2()
    {
        castFire2.enabled = false;
        castWater2.enabled = false;
        castIce2.enabled = false;
        castWind2.enabled = false;
        castEarth2.enabled = false;
        castRestore2.enabled = false;
        castShrink2.enabled = false;
        castGrow2.enabled = false;
        castHint2.enabled = false;
    }

    //gets the appropriate spellbook image
    private void getBook(int index)
    {
        if (index <= 10) { spellbookMenu.GetComponent<Image>().sprite = Spellbook2; }
        else if (index <= 15) { spellbookMenu.GetComponent<Image>().sprite = Spellbook3; }
        else { spellbookMenu.GetComponent<Image>().sprite = Spellbook1; }
    }
}