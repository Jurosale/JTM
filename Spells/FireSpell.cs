using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ********************************************
 *      Animates the fire spell
*********************************************** */

class FireSpell : MonoBehaviour
{

    //used to access the fire animation and particle systems
    private ParticleSystem sparks, smoke;
    private Animator anim;

    //helps control how long the animation last
    private float clock;
    private bool animFlag;

    //grants access to the speech recognition software
    SpeechRecognition01 speech;

    void Start()
    {
        //assigns the animations and particles
        anim = GetComponent<Animator>();
        sparks = this.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        smoke = this.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();

        anim.speed = 0;
        animFlag = false;

        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
    }

    // Update is called once per frame
    void Update()
    {

        // When the player casts fire, activates the fire animation
        if (speech.word == "fire")
        {
            anim.speed = 1.2f;
            anim.Play("FireAnim", 0, 0);
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            sparks.Clear();
            smoke.Clear();
            sparks.Play();
            smoke.Play();
            clock = 0f;
            animFlag = true;
        }

        // Makes the fire spell "disappear" once the animation is done
        if (animFlag)
        {
            clock += Time.deltaTime;
        }

        //fire spell disappears after 1.4 seconds
        if (clock > 1.4f && animFlag)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            anim.speed = 0;
            if (clock > 3.0f)
            {
                gameObject.transform.position = new Vector3(0, -10000f, 0);
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                animFlag = false;
            }
        }
    }
}