using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAffector : MonoBehaviour {

    public bool tripped;

    public bool footsteps;
    public bool soundEffect;

    public bool waves;
    public bool storm;
    public bool splash;

    public string footstepPath;
    public string soundEffectType;

    public bool fadeAway;

    SoundManager manager;

	// Use this for initialization
	void Start () {

        manager = transform.parent.gameObject.GetComponent<SoundManager>();
        tripped = false;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {

            tripped = true;

            if (footsteps)
            {
                manager.changeFootsteps(footstepPath);
            }

            if (soundEffect)
            {
                if (fadeAway)
                {
                    if (waves)
                    {
                        manager.FadeOutWaves();
                    }
                    else if (storm)
                    {
                        manager.FadeOutStorm();
                    }

                }
                else
                {
                    if (waves)
                    {
                        manager.FadeInWaves();
                    }
                    else if (storm)
                    {
                        manager.FadeInStorm();
                    }
                }
            }
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            print("bye");
            tripped = false;

            if (!fadeAway)
                fadeAway = true;
            else
                fadeAway = false;
        }
    }
}
