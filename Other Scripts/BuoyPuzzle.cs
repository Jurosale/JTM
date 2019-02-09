using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoyPuzzle : MonoBehaviour {

    GameObject speechObj;
    SpeechRecognition01 speech;
    Rigidbody2D rb;

    public GameObject goodie;
    public GameObject sparkle;
    Rigidbody2D goodieRb;

    bool bell;

    FMOD.Studio.EventInstance bellSound;
    string bellPath = "event:/Other SFX/Buoy";

    // Use this for initialization
    void Start () {

        speechObj = GameObject.Find("SpeechRecognition");
        speech = speechObj.GetComponent<SpeechRecognition01>();
        rb = GetComponent<Rigidbody2D>();
        goodieRb = goodie.GetComponent<Rigidbody2D>();

        StartCoroutine("Stormy");
        StartCoroutine("DingDing");
        bell = false;

        bellSound = FMODUnity.RuntimeManager.CreateInstance(bellPath);
		
	}
	
	// Update is called once per frame
	void Update () {
		
    }

    void OnTriggerStay2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            print("Vivi is here");
            bell = true;

            if (speech.word == "wind")
            {
                print("Winded");
                if (other.transform.position.x > transform.position.x)
                    Knocked(-1);
                else if (other.transform.position.x <= transform.position.x)
                    Knocked(1);
            }
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            bell = false;
        }

    }

    void Knocked(int dir)
    {
        if (dir > 0)
            rb.AddTorque(-400f);
        else if (dir < 0)
            rb.AddTorque(400f);
        else
            print("Knocked with false paramter");

        ReleaseGoodie();
    }

    void ReleaseGoodie()
    {
        goodie.transform.parent = null;
        goodieRb.simulated = true;
        goodieRb.AddForce(Vector3.right * 12f, ForceMode2D.Impulse);
        goodieRb.AddForce(Vector3.up * 15f, ForceMode2D.Impulse);
        goodieRb.AddTorque(-100f);
        StartCoroutine("TeleportPage");
    }

    IEnumerator TeleportPage()
    {

        yield return new WaitForSeconds(2f);
        goodie.transform.position = new Vector3(180, 4, 0);
        goodie.GetComponent<SpriteRenderer>().sortingOrder = 6;
        sparkle.GetComponent<SpriteRenderer>().sortingOrder = 6;
    }

    IEnumerator Stormy()
    {

        yield return new WaitForSeconds(2);
            rb.AddTorque(Random.Range(-300, 0));
        yield return new WaitForSeconds(2);
            rb.AddTorque(Random.Range(0, 300));

        StartCoroutine("Stormy");

    }

    IEnumerator DingDing()
    {
        if (bell)
        {
            bellSound.start();
        }

        yield return new WaitForSeconds(8);
        StartCoroutine("DingDing");
    }
}
