using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelReset : MonoBehaviour {

    GameObject avatar;
    Vector3 pos;

	// Use this for initialization
	void Start () {
        avatar = GameObject.Find("Witch character");
        pos = avatar.transform.position;    
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //if vivi falls out of bounds, reset her position to the starting point
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            avatar.transform.position = pos;
        }
    }
}
