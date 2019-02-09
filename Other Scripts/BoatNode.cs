using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatNode : MonoBehaviour {

    public bool entered;

	// Use this for initialization
	void Start () {

        entered = false;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay2D(Collider2D other)
    {
        if ((other.tag == "boat" || other.tag == "Player") && other.name != "LeftSideCollider" && other.name != "LeftSideCollider")
        {
            //Debug.Log(gameObject.name + " E: " + other.name);
            entered = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if ((other.tag == "boat" || other.tag == "Player") && other.name != "LeftSideCollider" && other.name != "LeftSideCollider")
        {
            //Debug.Log(gameObject.name + " L: " + other.name);
            entered = false;
        }
    }
}
