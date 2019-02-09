using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaryPageTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 9)
        {

            transform.parent.gameObject.GetComponent<DiaryPage>().Acquired();
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
