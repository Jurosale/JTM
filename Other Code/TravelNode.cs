//tell pat to comment this

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelNode : MonoBehaviour {

    GameObject gameMasterObj;
    GameMaster gameMaster;

    public List<GameObject> speakObjects;
    public List<GameObject> castObjects;

    // Use this for initialization
    void Start () {

        gameMasterObj = GameObject.Find("Game Master");
        gameMaster = gameMasterObj.GetComponent<GameMaster>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {

            gameMaster.LoadSpeakInfo(speakObjects);
            gameMaster.LoadCastInfo(castObjects);

            gameMaster.StateOn();

        }

    }

    void OnTriggerExit2D(Collider2D other)
    {

        if (other.tag == "Player")
        {

            gameMaster.StateOff();

        }

    }
}
