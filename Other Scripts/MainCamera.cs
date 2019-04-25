using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MainCamera : MonoBehaviour
{

    Camera mainCamera;

    //helper variables to create water reflection in level 3
    private GameObject reflectionObj;
    WaterFX reflection;
    Vector3 reflectionScale;

    private bool levelThree;

    // Use this for initialization
    void Start()
    {
        mainCamera = gameObject.GetComponent<Camera>();
    }

    //gets called in main script to zoom out camera during large puzzles
    public IEnumerator ZoomOut()
    {
        if (levelThree)
            reflectionObj.GetComponent<WaterFX>().m_distorsionAmount = 0;


        while (mainCamera.orthographicSize < 10)
        {
            yield return new WaitForEndOfFrame();
            mainCamera.orthographicSize += .1f;
        }
    }

    //gets called in main script to zoom in camera after completing large puzzles
    public IEnumerator ZoomIn()
    {
        while (mainCamera.orthographicSize > 5)
        {
            yield return new WaitForEndOfFrame();
            mainCamera.orthographicSize -= .1f;
        }

        if (levelThree)
            reflectionObj.GetComponent<WaterFX>().m_distorsionAmount = .127f;
    }

    //these 2 functions get called in main script once player reaches lvl 3
    public void SetReflection(GameObject reflect)
    {
        reflectionObj = reflect;
    }

    public void SetLevelThree(bool isLevelThree)
    {
        levelThree = isLevelThree;
    }
}