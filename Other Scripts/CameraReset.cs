using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraReset : MonoBehaviour {

    public GameObject gameCamObject;
    MainCamera gameCam;

	// Use this for initialization
	void Start () {

        gameCam = gameCamObject.GetComponent<MainCamera>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        print("Checking camera state");

        if (other.gameObject.tag == "Player")
        {
            if (gameCamObject.GetComponent<Camera>().orthographicSize > 5)
            {
                print("Camera zoomed out, zooming back in");
                gameCam.StartCoroutine("ZoomIn");

            }
        }
    }
}
