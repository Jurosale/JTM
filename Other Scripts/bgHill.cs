using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgHill : MonoBehaviour {

    public float speedOfCamera4;
    public GameObject Layer4;

    GameObject vivi;
    SpeechRecognition01 speech;

    void Start()
    {
        vivi = GameObject.Find("Witch character");
        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
    }

    //...If player moves, the background moves at a small rate
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (vivi.GetComponent<Movement>().bgMove == true)
            {
                if (vivi.GetComponent<Movement>().moveleft)
                {
                    Layer4.transform.Translate(Vector3.left * speedOfCamera4, Camera.main.transform);
                }
                if (vivi.GetComponent<Movement>().moveright)
                {
                    Layer4.transform.Translate(Vector3.right * speedOfCamera4, Camera.main.transform);
                }
            }
        }
    }
}
