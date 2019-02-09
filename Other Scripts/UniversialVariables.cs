using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversialVariables : MonoBehaviour {

    public int currWordPnts, currPages, pages;
    public List<string> currDiscovered;
    public List<string> currType;
    public List<int> currCalled;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

    //stores the current scores into the next scene
    public void GetVariables()
    {
        currWordPnts = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().wordPnts;
        currPages = GameObject.Find("SpellBook").GetComponent<SpellBook>().numPages;
        pages = GameObject.Find("SpellBook").GetComponent<SpellBook>().pages;
        currDiscovered = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().discovered;
        currType = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().type;
        currCalled = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>().called;
        DontDestroyOnLoad(gameObject);
    }
}
