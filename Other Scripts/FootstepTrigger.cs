using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepTrigger : MonoBehaviour {

    bool tripped;

    public string forwardFootfalls;
    public string backwardsFootfalls;

	// Use this for initialization
	void Start () {

        tripped = false;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            if (!tripped)
            {
                other.GetComponent<Movement>().ResetFootstepSound(forwardFootfalls);
                tripped = true;
            }
            else if (tripped)
            {
                other.GetComponent<Movement>().ResetFootstepSound(backwardsFootfalls);
                tripped = false;
            }
        }

    }
}
