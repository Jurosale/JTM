using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopGrow : MonoBehaviour {

    public GameObject avatar;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //prevents player from growing in specific situations
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            avatar.GetComponent<AvatarSpells>().canGrow = false;
        }
    }
}
