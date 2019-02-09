using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ********************************************
 *      Moves the background art
*********************************************** */


public class Parallax : MonoBehaviour {

    public GameObject canMoveCollider;

    GameObject Vivi;
    SpeechRecognition01 speech;
    
    public float speed;
    public float tempSpeed;
    

    // Use this for initialization
    void Start () {
        
        Vivi = GameObject.Find("Witch character");
        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
        tempSpeed = speed;
		
	}
	
	// Update is called once per frame
	void Update () {
        if (canMoveCollider.GetComponent<Parallax_canMove>().canMove == true)
        {
            //...If Vivi moves right or left, the background moves at speed
            if (Vivi.gameObject.GetComponent<Movement>().moveright == true)
            {
                gameObject.transform.Translate(Vector3.left * speed, Camera.main.transform);

            }
            if (Vivi.gameObject.GetComponent<Movement>().moveleft == true)
            {
                gameObject.transform.Translate(Vector3.right * speed, Camera.main.transform);
            }
        }
    }

}

