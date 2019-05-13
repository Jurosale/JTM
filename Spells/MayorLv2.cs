using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MayorLv2 : MonoBehaviour
{

    int currTasks;
    float complete, minReq;
    GameObject vivi, wall;

    void Start()
    {
        complete = 4;
        minReq = Mathf.Ceil(complete * .7f);
        vivi = GameObject.Find("Witch character");
        wall = GameObject.Find("MayorWall");
        vivi.GetComponent<AvatarSpells>().tasks = 0;
        currTasks = -1;
    }

    void Update()
    {
        //mayor will say one of the following things...
        if (currTasks != vivi.GetComponent<AvatarSpells>().tasks)
        {
            currTasks = vivi.GetComponent<AvatarSpells>().tasks;

            //if you've helped everyone...
            if (vivi.GetComponent<AvatarSpells>().tasks / complete == 1)
            {
                GetComponent<NPCSpells>().spiritMessage[0] = "You've helped everyone here. Thank you kind witch. You may pass.";
                wall.SetActive(false);
            }

            //if you've helped enough people (70%)...
            else if (vivi.GetComponent<AvatarSpells>().tasks / complete >= .7f)
            {
                GetComponent<NPCSpells>().spiritMessage[0] = "You've helped a lot but there are still lost souls who need you. " +
                    "You may pass though. I can't force you to help those souls.";
                wall.SetActive(false);
            }

            //if you haven't helped enough people...
            else
            {
                GetComponent<NPCSpells>().spiritMessage[0] = "There are plenty of lost souls that need you right now. Try helping at least " + 
                    (minReq - vivi.GetComponent<AvatarSpells>().tasks) + " red spirits to proceed.";
                wall.SetActive(true);
            }
        }
    }
}