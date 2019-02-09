using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* ********************************************
 *      Animates the magic circle
*********************************************** */
public class UseSpell : MonoBehaviour {

    //For Keyword: Cast
    public GameObject castDarkness;
    public GameObject castMoon;
    
    //For Keyword: Vivi
    public GameObject viviKeyword;
    public GameObject gradient;
    public GameObject viviKeywordUi;

    private Animator anim;
    private float clock;
    private float moveClock;

    public bool Cast; //Animator
    public bool Move;
    public bool Active;
    private bool animFlag;

    bool rolling;
    bool rolledOut;
    bool rollingIn;

    SpeechRecognition01 speech;
    GameObject vivi;
    Vector3 uiSize;

    public List<GameObject> spells;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = 0;
        animFlag = false;
        vivi = GameObject.Find("Witch character");
        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
        viviKeyword.SetActive(false);
        gradient.SetActive(false);
        uiSize = viviKeywordUi.transform.localScale;
        viviKeywordUi.transform.localScale = new Vector3(0f,0f,0f);
        Active = true;
        rolling = false;
        rolledOut = false;
    }

    // Update is called once per frame
    void Update()
    {
        vivi.GetComponent<Animator>().SetBool("Cast", Cast);
        Cast = false;
        //Controlling movement state feedback
        Move = speech.moveOn;

        if (speech.castOn)
        {
            //print("castOn");
            if (!rolledOut)
            {
                rolledOut = true;
                StartCoroutine("RollOutAllSpells");
            }
 
        }
        else if (!speech.castOn)
        {
            //print("!castOn");
            if (rolledOut)
            {
                rolledOut = false;
                StartCoroutine("RollInAllSpells");
            }
        }

        if (Move)
        {
            moveClock = 0;
            animFlag = true;
        }
        else
        {
            moveClock = 0f;
            animFlag = false;
            viviKeyword.SetActive(false);
            gradient.SetActive(false);
            viviKeywordUi.transform.localScale = new Vector3(0f, 0f, 0f);
        }

        // When the player casts any spell, make magic circle appear below Vivi

        if (speech.castOn && !Cast && Active)
        {
            Cast = true;
            //Has magic circle appear below Vivi's feet
            gameObject.transform.position = new Vector3(vivi.transform.position.x,
                        vivi.GetComponent<BoxCollider2D>().bounds.min.y + 1.2f, vivi.transform.position.z);
            // Make magical circle appear below Vivi no matter what size she is

            
            anim.speed = 1.4f;
            anim.Play("UseSpellAnim", 0, 0);
            clock = 0f;
            animFlag = true;
        }

        // Makes the UseSpell spell "disappear" once the animation is done
        if (animFlag) {

            if (!Move)
            {
                clock += Time.deltaTime;
                castDarkness.SetActive(true);
                castMoon.SetActive(true);
            }
            else
            {
                moveClock += Time.deltaTime;
                viviKeyword.SetActive(true);
                gradient.SetActive(true);
                viviKeywordUi.transform.localScale = uiSize;
            }

        }

        //Debug.Log(moveClock);
        if (clock > .5f || !Cast)
        {
            gameObject.transform.position = new Vector3(0, -10000f, 0);
            anim.speed = 0;
            clock = 0f;
            animFlag = false;
            castDarkness.SetActive(false);
            castMoon.SetActive(false);
        }

        if (moveClock > 5f)
        {
            moveClock = 0f;
            animFlag = false;
            viviKeyword.SetActive(false);
            gradient.SetActive(false);
            viviKeywordUi.transform.localScale = new Vector3(0f, 0f, 0f);
        }


 
   }

    public IEnumerator RollOutAllSpells()
    {
        print("Rolling Out");
        for (int i = 0; i < spells.Count; i++)
        {
            StartCoroutine(RollOutSpell(spells[i]));
            yield return new WaitForSecondsRealtime(.25f);
        }
    }

    IEnumerator RollOutSpell(GameObject spell)
    {

        for (int j = 0; j < 30; j++)
        {
            spell.GetComponent<RectTransform>().Rotate(new Vector3(0, 0, -2));
            yield return null;
        }
    }

    public IEnumerator RollInAllSpells()
    {
        print("Rolling In");
        for (int i = 0; i < spells.Count; i++)
        {
            StartCoroutine(RollInSpell(spells[i]));
            yield return new WaitForSecondsRealtime(.25f);
        }
    }

    IEnumerator RollInSpell(GameObject spell)
    {

        for (int j = 0; j < 30; j++)
        {
            spell.GetComponent<RectTransform>().Rotate(new Vector3(0, 0, 2));
            yield return null;
        }
    }
}
