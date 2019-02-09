using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MayorLv3 : MonoBehaviour
{

    private int currRoots;
    private float complete, minReq;
    SpeechRecognition01 speech;
    GameObject vivi, spiritual_tree;
    public bool move, moved, revived, first;
    public GameObject ori_travel, moved_travel;

    public GameObject musicManager;

    // Use this for initialization
    void Start()
    {
        complete = 5;
        minReq = Mathf.Ceil(complete * .7f);
        vivi = GameObject.Find("Witch character");
        vivi.GetComponent<AvatarSpells>().roots = 0;
        spiritual_tree = GameObject.Find("spiritualtree");
        speech = GameObject.Find("SpeechRecognition").GetComponent<SpeechRecognition01>();
        currRoots = -1;
        move = false;
        moved = false;
        ori_travel = GameObject.Find("Goddess_Ori");
        moved_travel = GameObject.Find("Goddess_Moved");
        ori_travel.SetActive(true);
        moved_travel.SetActive(false);
        first = true;
    }

    // Update is called once per frame
    void Update()
    {
        //will update what goddess says depending on situation
        if (currRoots != vivi.GetComponent<AvatarSpells>().roots || GetComponent<NPCSpells>().spokeTo)
        {
            currRoots = vivi.GetComponent<AvatarSpells>().roots;
            Debug.Log(currRoots);

            //if you've already spoken to goddess and havent restore all roots, she lets you know how many are still missing
            if (!first)
            {
                if (GetComponent<NPCSpells>().spiritMessage.Count > 1)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        GetComponent<NPCSpells>().spiritMessage.Remove(GetComponent<NPCSpells>().spiritMessage[1]);
                    }                
                }
                if (GetComponent<NPCSpells>().commandMessage.Count > 2)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        GetComponent<NPCSpells>().commandMessage.Remove(GetComponent<NPCSpells>().commandMessage[2]);
                    }
                }
                if (complete - vivi.GetComponent<AvatarSpells>().roots != 1)
                {
                    GetComponent<NPCSpells>().spiritMessage[0] = "You still need to restore " + (complete - vivi.GetComponent<AvatarSpells>().roots)
                        + "  more roots to save the spiritual tree.";
                }
                else
                {
                    GetComponent<NPCSpells>().spiritMessage[0] = "You still need to restore 1 more root to save the spiritual tree.";
                }
            }

            //if first time speaking to goddess, she greets you
            if (first && GetComponent<NPCSpells>().spokeTo)
            {
                first = false;
            }

            //if player restores all roots, goddess let's player know that
            else if (vivi.GetComponent<AvatarSpells>().roots / complete == 1 && !first)
            {
                GetComponent<NPCSpells>().spiritMessage[0] = "You've restored all the roots! Please come with me and cast \"Revitalize\" to revive the spiritual tree. Thank you Vivi!";
                GetComponent<NPCSpells>().spokeTo = false;
            }
            else if (GetComponent<NPCSpells>().spokeTo) { GetComponent<NPCSpells>().spokeTo = false; }
        }

        //once all roots are restored and player talks to goddess, she moves to spiritual tree
        else
        {
            if (speech.word == "speak" && !move && currRoots == complete && !first && Mathf.Abs(vivi.transform.position.x - gameObject.transform.position.x) < 4.0f)
            {
                move = true;
                ori_travel.SetActive(false);
            }
        }
        if (move && !moved)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, new Vector3(spiritual_tree.transform.position.x - 10.0f, gameObject.transform.position.y, gameObject.transform.position.z), Time.deltaTime * 0.2f);
            if (gameObject.transform.position.x <= spiritual_tree.transform.position.x - 1.0f)
            {
                moved = true;
                moved_travel.SetActive(true);
            }
        }
        if (moved)
        {
            Debug.Log(Mathf.Abs(vivi.transform.position.x - spiritual_tree.transform.position.x));
            if (speech.word == "revitalize" && Mathf.Abs(vivi.transform.position.x - spiritual_tree.transform.position.x) < 10.0f)
            {
                revived = true;
            }
        }

        //if player casts revitalized after goddess moves to spiritual tree, ending credits play
        if (revived)
        {
            speech.word = "";
            Time.timeScale = 1f;
            GameObject.Find("Variables").GetComponent<UniversialVariables>().GetVariables();
            GameObject.Find("Variables").GetComponent<Highscores>().UpdateScores();
            GameObject.Find("Variables").GetComponent<Scores>().keepScores();
            musicManager.GetComponent<MusicManager>().StartCoroutine("KillMusic");
            speech.enabled = false;
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
        }
    }
}
