using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax_collide : MonoBehaviour {

    GameObject Vivi;
    SpeechRecognition01 speech;
    public GameObject midground, background;

    // Use this for initialization
    void Start () {

        Vivi = GameObject.Find("Witch character");
        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //...If Vivi collides with any of the colliding colliders, the background stops moving
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //foreground.GetComponent<Parallax>().speed = 0.0f;
            midground.GetComponent<Parallax>().speed = 0.0f;
            background.GetComponent<Parallax>().speed = 0.0f;
        }
    }

    //...When Vivi is not colliding with anything anymore, have the background move again
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //foreground.GetComponent<Parallax>().speed = foreground.GetComponent<Parallax>().tempSpeed;
            midground.GetComponent<Parallax>().speed = midground.GetComponent<Parallax>().tempSpeed;
            background.GetComponent<Parallax>().speed = background.GetComponent<Parallax>().tempSpeed;
        }
    }
}
