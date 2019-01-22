//Ignore this; we don't use this in the final version

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SpiritSpells : MonoBehaviour {

    public GameObject angrySpirit;
    public GameObject happySpirt;
    public GameObject shrineParticle;

    public GameObject vines;
    public GameObject fruit;

    public bool canPray;
    public bool Pray, Cast; //animator

    //Music and sound
    FMOD.Studio.EventInstance spiritInteraction;
    public string musicPrayer = "event:/BGM/MUS_SpiritPrayer";
    public string surprisedSound = "event:/Voice Overs/VO_Vivi_Surprise02";
    //Reverb snapshot (stops the human music song)
    FMOD.Studio.EventInstance reverbTurnOn;
    public string snapshotPrayer = "snapshot:/PrayerMusic";

    private Vector4 colorVines, colorFruit;
    private bool shrineAnim;
    private float shrineTimer;

    //Reverb timer (how long the city bgm stops playing)
    private float reverbTimer;

    SpeechRecognition01 speech;
    GameObject vivi;

    // Use this for initialization
    void Start () {
        colorVines = new Vector4(vines.GetComponent<SpriteRenderer>().color.r, vines.GetComponent<SpriteRenderer>().color.g,
            vines.GetComponent<SpriteRenderer>().color.b, vines.GetComponent<SpriteRenderer>().color.a);
        colorFruit = new Vector4(fruit.GetComponent<SpriteRenderer>().color.r, fruit.GetComponent<SpriteRenderer>().color.g,
            fruit.GetComponent<SpriteRenderer>().color.b, fruit.GetComponent<SpriteRenderer>().color.a);
        vines.GetComponent<SpriteRenderer>().color = new Vector4(vines.GetComponent<SpriteRenderer>().color.r, 
            vines.GetComponent<SpriteRenderer>().color.g, vines.GetComponent<SpriteRenderer>().color.b, 0f);
        fruit.GetComponent<SpriteRenderer>().color = new Vector4(fruit.GetComponent<SpriteRenderer>().color.r,
            fruit.GetComponent<SpriteRenderer>().color.g, fruit.GetComponent<SpriteRenderer>().color.b, 0f);
        vines.SetActive(false);
        fruit.SetActive(false);
        happySpirt.SetActive(false);
        canPray = false;
        shrineAnim = false;
        shrineTimer = 0f;
        reverbTimer = 0f;
        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
        vivi = GameObject.Find("Witch character");

    }
	
	// Update is called once per frame
	void Update () {

        vivi.GetComponent<Animator>().SetBool("Pray", Pray);
        Pray = false;

        if (speech.word == "prayer" && canPray == true)
        {
            //Replace images
           vines.SetActive(true);
           fruit.SetActive(true);
           happySpirt.SetActive(true);
           angrySpirit.SetActive(false);
           shrineParticle.SetActive(true);
            //Play music
           spiritInteraction = FMODUnity.RuntimeManager.CreateInstance(musicPrayer);
           spiritInteraction.start();
            //Turn on Reverb
            reverbTurnOn = FMODUnity.RuntimeManager.CreateInstance(snapshotPrayer);
            reverbTurnOn.start();

            //play anim
            shrineAnim = true;
            shrineTimer = 0f;
            vines.GetComponent<SpriteRenderer>().color = new Vector4(vines.GetComponent<SpriteRenderer>().color.r,
                vines.GetComponent<SpriteRenderer>().color.g, vines.GetComponent<SpriteRenderer>().color.b, 0f);
            fruit.GetComponent<SpriteRenderer>().color = new Vector4(fruit.GetComponent<SpriteRenderer>().color.r,
                fruit.GetComponent<SpriteRenderer>().color.g, fruit.GetComponent<SpriteRenderer>().color.b, 0f);

            Pray = true;

        }
        if(shrineAnim == true)
        {
            reverbTimer += Time.deltaTime;
            if(reverbTimer > 2.0f)
            {
                reverbTurnOn.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

            }
        }

        if(shrineAnim)
        {
            float timePass = Time.deltaTime;
            shrineTimer += timePass;
            reverbTimer += Time.deltaTime;

            vines.GetComponent<SpriteRenderer>().color = new Vector4(vines.GetComponent<SpriteRenderer>().color.r,
                vines.GetComponent<SpriteRenderer>().color.g, vines.GetComponent<SpriteRenderer>().color.b, 
                vines.GetComponent<SpriteRenderer>().color.a + timePass/2.5f);
            Debug.Log("reverbTimer: " + reverbTimer);
            Debug.Log("shrineTimer: " + shrineTimer);
            Debug.Log("timePass: " + timePass);
            if (shrineTimer >= 2.5f)
            {
                reverbTimer = 0f;
                //reverbTurnOn.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                shrineAnim = false;
                fruit.GetComponent<SpriteRenderer>().color = colorFruit;
            }
        }
    }

    //set flag
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            canPray = true;
        }
    }

    //play sound
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            //FMODUnity.RuntimeManager.PlayOneShot(surprisedSound);
        }
    }

    //undo flag
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            canPray = false;
        }
    }
}
