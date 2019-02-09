using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holder : MonoBehaviour {

    public GameObject item;

    public bool is2DRigidbody;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (is2DRigidbody)
            {
                item.GetComponent<Rigidbody2D>().simulated = true;
            }
        }
    }
}
