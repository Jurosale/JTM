using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ********************************************
 *      Animates the fire spell the complete
 *      frames. 
*********************************************** */

public class FireSpell : MonoBehaviour {

    private ParticleSystem sparks, smoke;
    private Animator anim;
    private float clock;
    private bool animFlag;

    SpeechRecognition01 speech;

    void Start()
    {
        anim = GetComponent<Animator>();
        sparks = this.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        smoke = this.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
        anim.speed = 0;
        animFlag = false;
        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
    }

    // Update is called once per frame
    void Update () {

        // When the player casts fire, the fire animation works
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