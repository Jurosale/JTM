//tell pat to comment this

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour {

    //This script can only apply to one puzzle and a new one must be made for every puzzle
    //Hopefully we can find a way to make one universal script to apply to all puzzles

    public GameObject mainCameraObj;
    public GameObject blockingCollider;
    public GameObject puzzleCenter;

    public GameObject thoughtBubble;
    public GameObject[] angryStuffOne;
    public GameObject[] angryStuffTwo;

    public int patienceMeter;

    GameObject speechRecObj;
    SpeechRecognition01 speechRecognition;

    public GameObject[] puzzleComponentObjects;
    List<PuzzleComponent> puzzleComponents;

    MainCamera mainCamera;

    public int cameraSpeed;

    bool puzzleSolved;
    bool tripped;
    bool patienceTicked;

	// Use this for initialization
	void Start () {

        speechRecObj = GameObject.Find("SpeechRecognition");
        speechRecognition = speechRecObj.GetComponent<SpeechRecognition01>();

        puzzleSolved = false;
        tripped = false;

        mainCamera = mainCameraObj.GetComponent<MainCamera>();

        puzzleComponents = new List<PuzzleComponent>();

        foreach (GameObject thing in puzzleComponentObjects)
        {
            puzzleComponents.Add(thing.GetComponent<PuzzleComponent>());
        }

        patienceMeter = 0;
        patienceTicked = false;

    }
	
	// Update is called once per frame
	void Update () {

        if (puzzleSolved)
        {
            Destroy(blockingCollider);
            puzzleSolved = false;  //Otherwise the script will keep trying to destroy a destroyed object
        }

        bool puzzleSolvedCheck = true;

        foreach (PuzzleComponent thing in puzzleComponents)
        {
            if (thing.conditionMet == false)
                puzzleSolvedCheck = false;
        }

        if (puzzleSolvedCheck)
        {
            puzzleSolved = true;
        }
        
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Vivi in puzzle zone");

            //Camera "zoom" out
            mainCamera.StopCoroutine("ZoomIn");
            mainCamera.StartCoroutine("ZoomOut");

            if (!tripped)
            {
                other.GetComponent<Movement>().inPuzzleZone = true;
                tripped = true;
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            if (!puzzleSolved)
            {
                if (speechRecognition.word == "right")
                {
                    if (patienceTicked == false)
                    {
                        patienceMeter++;
                        patienceTicked = true;

                        other.gameObject.GetComponent<Movement>().moveright = false;
                        StartCoroutine("NoCanDo");
                    }
                }
            }
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Vivi out of puzzle zone");

            //Camera zoom in
            mainCamera.StopCoroutine("ZoomOut");
            mainCamera.StartCoroutine("ZoomIn");

            if (tripped)
            {
                other.GetComponent<Movement>().inPuzzleZone = true;
                tripped = false;
            }
        }
    }

    IEnumerator NoCanDo()
    {

        //thoughtBubble.gameObject.SetActive(true);

        if (patienceMeter > 2)
        {
            StartCoroutine("MadNow");
        }

        yield return new WaitForSeconds(2f);

        thoughtBubble.gameObject.SetActive(false);
        patienceTicked = false;

    }

    IEnumerator MadNow()
    {
        foreach (GameObject thingy in angryStuffOne)
        {
            thingy.SetActive(true);
        }

        yield return new WaitForSeconds(1f);

        foreach (GameObject thingy in angryStuffOne)
        {
            thingy.SetActive(false);
        }

        foreach (GameObject thingy in angryStuffTwo)
        {
            thingy.SetActive(true);
        }

        yield return new WaitForSeconds(1f);

        foreach (GameObject thingy in angryStuffTwo)
        {
            thingy.SetActive(false);
        }

        patienceTicked = false;
    }
}
