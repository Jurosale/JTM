using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* ******************************************************
 *      Moves background decoration with the player
********************************************************* */
public class Background : MonoBehaviour {

    public float speedOfCamera3, speedOfCamera2;
    public GameObject Layer3; //Back-background
    public GameObject Layer2; //Trees & house closer to the camera

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
                    Layer3.transform.Translate(Vector3.right * speedOfCamera3, Camera.main.transform);
                    Layer2.transform.Translate(Vector3.right * speedOfCamera2, Camera.main.transform);
                }
                if (vivi.GetComponent<Movement>().moveright)
                {
                    Layer3.transform.Translate(Vector3.left * speedOfCamera3, Camera.main.transform);
                    Layer2.transform.Translate(Vector3.left * speedOfCamera2, Camera.main.transform);
                }
            }
        }
    }
}
