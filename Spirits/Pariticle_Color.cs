//Ignore this

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pariticle_Color : MonoBehaviour {

    GameObject vivi;

	// Use this for initialization
	void Start () {
        vivi = GameObject.Find("Witch character");

    }
	
	// Update is called once per frame
	void Update () {
        if (Vector2.Distance(transform.position, vivi.transform.position) < 20f)
        {
            if (transform.parent.GetComponent<SpriteRenderer>())
            {
                //gameObject.GetComponent<ParticleSystem>().startColor = transform.parent.GetComponent<SpriteRenderer>().color;
            }
            else
            {
                //gameObject.GetComponent<ParticleSystem>().startColor = transform.parent.GetChild(1).GetComponent<SpriteRenderer>().color;
            }
        }
	}
}
