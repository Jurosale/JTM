using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ********************************************
 *      Checks for spells that directly
 *      affect the objects in scene
 *      Current Spells: Wind
*********************************************** */

class ObjectSpells : MonoBehaviour
{

    //wind animation objects; one blows to the left, the other right
    private GameObject WindParticlesL, WindParticlesR;

    private float windSpeed;

    //determines whether or not spell animations should be currently playing 
    private bool animFlag;

    //grants access to speech recognition software, avatar and UI
    SpeechRecognition01 speech;
    GameObject vivi, wind1, wind2;
    UIHistory uiH;

    //timer
    private float timer;

    void Start()
    {

        //modify these values if you want to change wind speed and
        //timer's initial starting value for delayed effect purposes
        windSpeed = 50;
        timer = 0;

        animFlag = false;

        //assigns game object values for various use
        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
        vivi = GameObject.Find("Witch character");
        wind1 = GameObject.Find("WindAnim");
        wind2 = GameObject.Find("WindAnim2");
        WindParticlesL = GameObject.Find("WindParticlesLeft");
        WindParticlesR = GameObject.Find("WindParticlesRight");
        uiH = GameObject.Find("UIH1").GetComponent<UIHistory>();
    }

    // When Vivi gets close enough to object 
    // And the player casted wind, 
    // It will move object
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && (speech.word == "wind" || uiH.isWind))
        {
            //turns off wind spell and animation 1/2 a second after activation
            if (timer > .5f) { uiH.isWind = false; }

            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z),
                Time.deltaTime * (windSpeed * vivi.GetComponent<Movement>().facingRight) * speech.pitch);
        }
    }

    //after user casts wind, activates the wind animation
    void FixedUpdate()
    {
        if ((speech.word == "wind" || uiH.isWind))
        {
            //turns off wind spell and animation 1/2 a second after activation
            if (timer > .5f) { uiH.isWind = false; }

            //Plays wind animation either left or right depending on what direction player is facing
            if (vivi.GetComponent<Movement>().facingRight >= 0)
            {
                wind1.transform.position = new Vector3(vivi.transform.position.x +
                    (4f * vivi.GetComponent<Movement>().facingRight),
                    vivi.transform.position.y, vivi.transform.position.z);

                WindParticlesR.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                WindParticlesR.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
            }
            else
            {
                wind2.transform.position = new Vector3(vivi.transform.position.x +
                    (4f * vivi.GetComponent<Movement>().facingRight),
                    vivi.transform.position.y, vivi.transform.position.z);

                WindParticlesL.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
                WindParticlesL.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
            }

            animFlag = true;
        }

        //Times how long the game object will appear
        if (animFlag) { timer += Time.deltaTime; }

        //once timer passes specified time, moves the object out of the way
        if (timer > 2.5f)
        {
            //Reset position and timer
            wind1.transform.position = new Vector3(transform.position.x + 500.0f,
                transform.position.y + 500.0f, transform.position.z + 500.0f);

            wind2.transform.position = new Vector3(transform.position.x + 500.0f,
                transform.position.y + 500.0f, transform.position.z + 500.0f);

            timer = 0;
            animFlag = false;
        }
    }
}