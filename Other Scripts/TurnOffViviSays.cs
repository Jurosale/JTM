using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TurnOffViviSays : MonoBehaviour {

    public GameObject viviInput;

	// Use this for initialization
	void Start () {

	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            viviInput.transform.GetChild(0).GetComponent<Text>().enabled = false;
            viviInput.GetComponent<Image>().enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            viviInput.transform.GetChild(0).GetComponent<Text>().enabled = true;
            viviInput.GetComponent<Image>().enabled = true;
        }
    }
}
