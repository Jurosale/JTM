using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWindPush : MonoBehaviour
{

    SpeechRecognition01 speech;
    UIHistory uiH;
    GameObject avatar;
    GameObject earth;

    public GameObject rightCollider;
    public GameObject leftCollider;
    Rigidbody2D rb;

    public bool pushRight;
    public bool pushLeft;
    public bool dontMove;
    bool moving;

    //Still sail for boat
    public GameObject sail;
    //Animated sail for boat
    public GameObject sailAni;

    public string splashPath = "event:/Other SFX/Splash";
    FMOD.Studio.EventInstance splash;

    Vector3 originalPosition;
    public bool willReset;

    // Use this for initialization
    void Start()
    {
        originalPosition = transform.position;

        leftCollider.SetActive(true);
        rightCollider.SetActive(true);

        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
        uiH = GameObject.Find("UIH1").GetComponent<UIHistory>();
        avatar = GameObject.Find("Witch character");
        earth = GameObject.Find("earth");
        pushRight = false;
        pushLeft = false;
        dontMove = false;

        //turns off animation for sail in particular
        sailAni.SetActive(false);

        splash = FMODUnity.RuntimeManager.CreateInstance(splashPath);
        moving = false;

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Resets position of some boats
        if (speech.word == "home" && willReset)
        {
            transform.position = originalPosition;
        }

        //makes boat move right
            if (pushRight)
        {
            if (!moving)
            {
                StartCoroutine("PushRight");
                moving = true;

            }
            //turns on flag ani
            sailAni.SetActive(true);
            //turns off still flag
            sail.SetActive(false);
        }

        //makes boat move left
        else if (pushLeft)
        {
            if (!moving)
            {
                StartCoroutine("PushLeft");
                moving = true;
            }
            //turns on flag ani
            sailAni.SetActive(true);
            //turns off still flag
            sail.SetActive(false);
        }

        //turns off sail animation and turns on the still sail
        if (pushLeft == false && pushRight == false)
        {
            StopAllCoroutines();
            moving = false;
            //turns off flag ani
            sailAni.SetActive(false);
            //turns on stil flag
            sail.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //if boat touches right end of dock, turn off left collider to exit
        //same appiles to the other LCpoint instances
        if (collision.tag == "LCpoint")
        {

            //rightCollider.SetActive(true);
            leftCollider.SetActive(false);
            pushLeft = false;

            //Debug.Log("enteredLC");
        }

        //if boat touches left end of dock, turn off right collider to exit
        //same appiles to the other RCpoint instances
        if (collision.tag == "RCpoint")
        {

            rightCollider.SetActive(false);
            pushRight = false;

            //leftCollider.SetActive(true);

        }

        //if player casts "wind" on boat, boat will move left/right
        if (collision.tag == "Player")
        {
            print("Player here");
            rb.constraints = RigidbodyConstraints2D.None;

            if (speech.word == "wind" || uiH.isWind)
            {
                // && collision.transform.position.x <= transform.position.x
                if (avatar.GetComponent<Movement>().facingRight > 0f)
                {
                    
                    pushRight = true;
                    pushLeft = false;
                    dontMove = false;
                }
                // && collision.transform.position.x > transform.position.x
                else if (avatar.GetComponent<Movement>().facingRight < 0f)
                {
                    pushLeft = true;
                    pushRight = false;
                    dontMove = false;
                }
                uiH.isWind = false;
            }
            //if (speech.word == "jump") { avatar.GetComponent<Movement>().jumpForce = .064f; }
            //else { avatar.GetComponent<Movement>().jumpForce = 0f; }
            //avatar.GetComponent<Rigidbody2D>().mass = .01f;

            //print(gameObject.GetComponent<Rigidbody2D>().velocity.y);

            //turns off earth while on boat
            earth.GetComponent<SummonSpells>().canEarth = false;
            uiH.isEarth = false;
        }

        if (collision.tag == "LCpoint")
        {

            //rightCollider.SetActive(true);
            leftCollider.SetActive(false);
            //Debug.Log("enteredLC");
        }

        if (collision.tag == "RCpoint")
        {

            rightCollider.SetActive(false);
            //leftCollider.SetActive(true);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        //turns on boat colliders to keep player inside
        if (collision.tag == "Player")
        {
            rightCollider.SetActive(true);
            leftCollider.SetActive(true);
            //Debug.Log("entered");

        }

        //makes boat stop when it reaches a boatpoint
        if (collision.tag == "boatpoint")
        {
            if (!collision.gameObject.GetComponent<BoatNode>().entered)
            {
                pushRight = false;
                pushLeft = false;
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, gameObject.GetComponent<Rigidbody2D>().velocity.y);
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //prevents player from casting "earth" while on boat
        if (collision.tag == "Player")
        {
            print("Player gone");
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;

            //rightCollider.SetActive(false);
            //leftCollider.SetActive(false);

            //turns on earth when exiting boat
            earth.GetComponent<SummonSpells>().canEarth = true;
        }

        if (collision.tag == "LCpoint")
        {
            leftCollider.SetActive(true);
            dontMove = false;
            //Debug.Log("leftLC");
        }

        if (collision.tag == "RCpoint")
        {
            rightCollider.SetActive(true);
            dontMove = false;
        }
        //avatar.GetComponent<Movement>().jumpForce = 8f;
        //if (avatar.GetComponent<Rigidbody2D>().mass <= .01f) { avatar.GetComponent<Rigidbody2D>().mass = 1.25f; }

    }

    IEnumerator PushRight()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(10, 0), ForceMode2D.Impulse);
        yield return new WaitForSeconds(2);
        StartCoroutine("PushRight");
    }

    IEnumerator PushLeft()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(-10, 0), ForceMode2D.Impulse);
        yield return new WaitForSeconds(2);
        StartCoroutine("PushLeft");
    }
}

