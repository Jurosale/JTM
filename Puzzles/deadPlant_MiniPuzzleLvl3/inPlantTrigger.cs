using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inPlantTrigger : MonoBehaviour {

    public bool nearPlant;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //checks if the player is in the trigger and if they are set it to true
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            nearPlant = true;
        }
    }

    //if not then set it to false
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            nearPlant = false;
        }
    }
}
