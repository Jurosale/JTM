using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowPush : MonoBehaviour {

    GameObject avatar;
    Rigidbody2D rb;

    bool moving;
    string grindPath = "event:/Other SFX/Grind";
    FMOD.Studio.EventInstance grinding;

    // Use this for initialization
    void Start () {

        avatar = GameObject.Find("Witch character");
        rb = GetComponent<Rigidbody2D>();
        moving = false;

        grinding = FMODUnity.RuntimeManager.CreateInstance(grindPath);

    }
	
	// Update is called once per frame
	void Update () {

        if (rb.velocity.x != 0)
        {
            //print("Moving bucket");
            if (!moving)
                grinding.start();

            moving = true;
        }

        if (moving && (rb.velocity.x > 0 || rb.velocity.x < 0))
        {
            grinding.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            moving = false;
        }
		
	}

    //adjusts the weight of bucket according to player's size
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && avatar.transform.localScale.y > 1f)
        {
            rb.mass = 1f;
        }

        else if (collision.tag == "Player" && avatar.transform.localScale.y <= 1f)
        {
            rb.mass = 1000f;
        }
    }

    //bucket gets heavy once player leaves it
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            rb.mass = 1000f;
        }
    }
}
