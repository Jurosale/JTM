using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax_canMove : MonoBehaviour {

    //public GameObject foreground;
    public GameObject midground;
    public GameObject background;

    public bool canMove;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //If inside trigger, bg can move
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canMove = true;
        }
    }
    //If outside the trigger, bg cannot move
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //foreground.GetComponent<Parallax>().speed = 0;
            midground.GetComponent<Parallax>().speed = 0;
            background.GetComponent<Parallax>().speed = 0;
            canMove = false;
        }
    }
}
