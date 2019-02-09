//we dont use this anymore

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDown : MonoBehaviour {

    float gravity;

	// Use this for initialization
	void Start () {
        gravity = 1.0f;	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x, (transform.position.y - Time.deltaTime * gravity), transform.position.z);
        if (transform.position.y <= -9000f) { gravity = 1f; }
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gravity = 0f;
    }

}
