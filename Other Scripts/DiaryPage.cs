using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaryPage : MonoBehaviour
{

    Rigidbody2D rb;

    //public string entry;
    public int ID;
    public bool tripped;
    bool falling;
    //Inkan: Plays the sparkle animation  (before pickup)
    public GameObject sparkleAni;
    //Inkan: Plays the particle system to indicate you picked it up
    public GameObject pickUpAni;
    //Inkan: For the page pick up sound
    FMOD.Studio.EventInstance pageSound;
    private string pageSoundPath = "event:/UI sounds/SFX_PagePickUp";

    SpeechRecognition01 sr;

    // Use this for initialization
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();

        tripped = false;

        falling = false;
        StartCoroutine("Falling");

        //turns off the pickup ani
        pickUpAni.SetActive(false);

        //Inkan: create the instance for the sou d
        pageSound = FMODUnity.RuntimeManager.CreateInstance(pageSoundPath);

        sr = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
    }

    // Update is called once per frame
    void Update()
    {

        if (rb.velocity.y < -.3f)
        {
            print("falling now");
            falling = true;
            StartCoroutine("Falling");
        }
        else
        {
            StopCoroutine("Falling");
            falling = false;
        }

    }

    public void Acquired()
    {
        //turns off diary page
        print("Got a page");
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        //Inkan: remove sparkle animation
        sparkleAni.GetComponent<SpriteRenderer>().enabled = false;
        sparkleAni.SetActive(false);

        //turns on the pickup ani
        pickUpAni.SetActive(true);
        //Inkan:Play the sound
        FMODUnity.RuntimeManager.PlayOneShot(pageSoundPath);

        sr.PointSystem(0, "page");

        //checks to see which diary page player got
        switch (ID)
        {
            case 1:
                GameObject.Find("SpellBook").GetComponent<SpellBook>().pages *= 2;
                break;
            case 2:
                GameObject.Find("SpellBook").GetComponent<SpellBook>().pages *= 3;
                break;
            case 3:
                GameObject.Find("SpellBook").GetComponent<SpellBook>().pages *= 5;
                break;
            case 4:
                GameObject.Find("SpellBook").GetComponent<SpellBook>().pages *= 7;
                break;
            case 5:
                GameObject.Find("SpellBook").GetComponent<SpellBook>().pages *= 11;
                break;
            default:
                break;
        }

    }

    IEnumerator Falling()
    {

        while (falling)
        {
            rb.AddForce(new Vector3(1, 0, 0), ForceMode2D.Force);
            yield return new WaitForSeconds(.5f);

            rb.AddForce(new Vector3(-1, 0, 0), ForceMode2D.Force);
            yield return new WaitForSeconds(.5f);
        }

    }
}
