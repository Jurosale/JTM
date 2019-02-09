//tell pat to comment this

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boiler : MonoBehaviour {

    public GameObject body;
    public GameObject chimney;
    public GameObject whistle;
    //public GameObject water;

    public GameObject waterAnim;
    Vector2 waterAnimOriginalLocation;

    Animator b_waterAnim;
    Animator bu_waterAnim;

    public GameObject fire;
    public GameObject steamOne;
    public GameObject steamTwo;
    public GameObject steamThree;

    public GameObject goodie;
    public GameObject sparkle;

    bool lit;
    bool watered;

    GameObject speechRecObj;
    SpeechRecognition01 speechRecognition;
    Level2Hints lv2H;

    Vector3 bodyOriginalLocation;
    Vector3 chimneyOriginalLocation;
    Vector3 whistleOriginalLocation;

    bool bodyShakes;
    bool chimneyShakes;
    bool whistleShakes;
    bool SFXplayed;

    FMOD.Studio.EventInstance drumrollEvent;
    public string drumrollPath = "event:/Other SFX/Drumroll";

    FMOD.Studio.EventInstance cymbalEvent;
    public string cymbalPath = "event:/Other SFX/Cymbal";

    FMOD.Studio.EventInstance whistleEvent;
    public string whistlePath = "event:/Other SFX/Whistle";

    // Use this for initialization
    void Start () {

        speechRecObj = GameObject.Find("SpeechRecognition");
        speechRecognition = speechRecObj.GetComponent<SpeechRecognition01>();
        lv2H = GameObject.Find("Puzzle and Textures").GetComponent<Level2Hints>();

        lit = false;
        watered = false;

        bodyOriginalLocation = body.transform.position;
        chimneyOriginalLocation = chimney.transform.position;
        whistleOriginalLocation = whistle.transform.position;

        bodyShakes = false;
        chimneyShakes = false;
        whistleShakes = false;

        steamOne.GetComponent<ParticleSystem>().Stop();
        steamTwo.GetComponent<ParticleSystem>().Stop();
        steamThree.GetComponent<ParticleSystem>().Stop();

        fire.GetComponent<ParticleSystem>().Stop();

        //water.SetActive(false);

        drumrollEvent = FMODUnity.RuntimeManager.CreateInstance(drumrollPath);
        cymbalEvent = FMODUnity.RuntimeManager.CreateInstance(cymbalPath);
        whistleEvent = FMODUnity.RuntimeManager.CreateInstance(whistlePath);

        SFXplayed = false;

        //water animation
        b_waterAnim = GameObject.Find("Boiler_water").GetComponent<Animator>();
        bu_waterAnim = GameObject.Find("Bucket_water").GetComponent<Animator>();

        bu_waterAnim.enabled = false;
        b_waterAnim.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (bodyShakes)
        {
            body.transform.position =
                new Vector3(
                    bodyOriginalLocation.x + Random.Range(-.05f, .05f),
                    bodyOriginalLocation.y + Random.Range(-.05f, .05f),
                    bodyOriginalLocation.z);
        }

        if (chimneyShakes)
        {
            chimney.transform.position =
            new Vector3(
                chimneyOriginalLocation.x + Random.Range(-.05f, .05f),
                chimneyOriginalLocation.y + Random.Range(-.05f, .05f),
                chimneyOriginalLocation.z);
        }

        if (whistleShakes)
        {
            whistle.transform.position =
            new Vector3(
                whistleOriginalLocation.x + Random.Range(-.05f, .05f),
                whistleOriginalLocation.y + Random.Range(-.05f, .05f),
                whistleOriginalLocation.z);
        }

    }

    void OnTriggerStay2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            if (speechRecognition.word == "fire")
            {

                lit = true;

                fire.GetComponent<ParticleSystem>().Play();

                if (watered)
                {
                    drumrollEvent.start();
                    StartCoroutine("Boiling");
                }

            }

            if (speechRecognition.word == "water")
            {
                lv2H.ShitsOnWater = true;
                watered = true;
                //water.SetActive(true);
                bu_waterAnim.enabled = true;
                b_waterAnim.enabled = true;
                b_waterAnim.Play("Boiler_water");
                bu_waterAnim.Play("bucket_water");

                if (lit)
                {
                    lit = false;
                    fire.GetComponent<ParticleSystem>().Stop();
                }

            }
        }

    }

    IEnumerator Boiling()
    {


        bodyShakes = true;
        steamOne.GetComponent<ParticleSystem>().Play();

        yield return new WaitForSeconds(1f);

        chimneyShakes = true;
        steamTwo.GetComponent<ParticleSystem>().Play();

        yield return new WaitForSeconds(1f);

        whistleShakes = true;
        steamOne.GetComponent<ParticleSystem>().Stop();
        steamTwo.GetComponent<ParticleSystem>().Stop();
        steamThree.GetComponent<ParticleSystem>().Play();

        if (SFXplayed == false)
        {
            drumrollEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            cymbalEvent.start();
            whistleEvent.start();
            SFXplayed = true;
        }

        goodie.GetComponent<Rigidbody2D>().gravityScale = .3f;
        goodie.GetComponent<Rigidbody2D>().AddForce(new Vector3(0.05f, 0, 0), ForceMode2D.Impulse);
        goodie.GetComponent<SpriteRenderer>().sortingOrder = 5;
        sparkle.GetComponent<SpriteRenderer>().sortingOrder = 6;

        yield return new WaitForSeconds(1f);

        steamThree.GetComponent<ParticleSystem>().Stop();
        bodyShakes = false;
        chimneyShakes = false;
        whistleShakes = false;
        watered = false;
        //water.SetActive(false);
        lv2H.ShitsOnWater = false;

        body.transform.position = bodyOriginalLocation;
        chimney.transform.position = chimneyOriginalLocation;
        body.transform.position = bodyOriginalLocation;

        SFXplayed = false;


    }
}
