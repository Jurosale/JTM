using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSpell : MonoBehaviour {

    private Animator anim;
    private float clock;
    private bool animFlag;

    public bool treeFlag;
    SpeechRecognition01 speech;
    GameObject vivi, wind1, wind2;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = 0;
        animFlag = false;
        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
        vivi = GameObject.Find("Witch character");
        wind1 = GameObject.Find("WindAnim");
        wind2 = GameObject.Find("WindAnim2");
    }
    // Update is called once per frame
    void Update()
    {

        // Turns on wind spell
        if (speech.word == "wind")
        {
            anim.speed = 1.2f;
            treeFlag = true;
            if (vivi.GetComponent<Movement>().facingRight >= 0) {
                anim.Play("WindAnim", 0, 0);
                wind2.transform.position = new Vector3(0, 100f, 0);
            }
            else {
                anim.Play("WindAnim2", 0, 0);
                wind1.transform.position = new Vector3(0, 100f, 0);
            }
            clock = 0f;
            animFlag = true;
        }

        // Makes the wind spell "disappear" once the animation is done
        if (animFlag) { clock += Time.deltaTime; }

        //Debug.Log(clock);
        if (clock > 1.5f)
        {
            gameObject.transform.position = new Vector3(0, 100f, 0);
            anim.speed = 0;
            clock = 0f;
            animFlag = false;
            treeFlag = false;
        }
    }
}
