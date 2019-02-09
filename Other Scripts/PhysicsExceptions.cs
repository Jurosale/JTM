using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsExceptions : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //player will sink in water
        Physics2D.IgnoreCollision(GameObject.Find("Witch character").GetComponent<Collider2D>(), GameObject.Find("water").GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(GameObject.Find("Witch character").GetComponent<Collider2D>(), GameObject.Find("water2").GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(GameObject.Find("Witch character").GetComponent<Collider2D>(), GameObject.Find("water3").GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(GameObject.Find("Witch character").GetComponent<Collider2D>(), GameObject.Find("water4").GetComponent<Collider2D>());
    }
	
	// Update is called once per frame
	void Update () {

    }
}
