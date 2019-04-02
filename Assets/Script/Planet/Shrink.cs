using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrink : MonoBehaviour
{

    /*
    Shrink Planet: this planet will shrink periodically
    */
    public float scaleFactor;
    bool startPulse = false;
    Vector3 defaultScale;
    public float lowerBound;

    private Vector3 origScale;
    private Vector3 origBottomScale;
    private Vector3 origChildScale;


    private float origHaloSize;
    private float origCatchRadius;
    private float origSlowResTime;

    public float change_rate = 0.005f;
    public float period;
    private float waitTime = 0.01f;

    private bool startShrink;
    private Transform childPlanet;
    private Transform subChild;

    public Transform bottomSprite;
    private float origGlowSizeShrink;


    void Start()
    {
        startShrink = true;
        scaleFactor = 1f;

        defaultScale = transform.localScale;

        //Record the original scale for each component for later resizing
        origScale = transform.GetChild(0).transform.localScale;
        origChildScale = transform.GetChild(1).transform.localScale;
        childPlanet = transform.Find("Planet2");

        //Debug.Log(gameObject.GetComponent<Planet>().lightController.range);

        if(gameObject.GetComponent<Planet>().lightController != null)
            origHaloSize = gameObject.GetComponent<Planet>().lightController.range;

        bottomSprite = gameObject.GetComponent<Planet>().planetBottom;


        bottomSprite = gameObject.GetComponent<Planet>().planetBottom;

        if(bottomSprite != null)
            origBottomScale = bottomSprite.localScale;

        origCatchRadius = transform.gameObject.GetComponent<Planet>().catchRadius;
        origSlowResTime = gameObject.GetComponent<Planet>().slowRespTime;
        origGlowSizeShrink = gameObject.GetComponent<Planet>().origGlowSize;
    }


    // Update is called once per frame
    void Update()
    {
        if (transform.GetComponent<Planet>().thePlayerOnPlanet == null && startShrink){
            startShrink = false;
            StartCoroutine(Scale());
        }
    }


    //rescale every 0.01 second 
    IEnumerator Scale()
    {
        while (transform.GetComponent<Planet>().thePlayerOnPlanet == null)
        {

            if(bottomSprite == null){
                bottomSprite = gameObject.GetComponent<Planet>().planetBottom;
                origBottomScale = bottomSprite.localScale;

                origCatchRadius = transform.gameObject.GetComponent<Planet>().catchRadius;
                origSlowResTime = gameObject.GetComponent<Planet>().slowRespTime;
                origGlowSizeShrink = gameObject.GetComponent<Planet>().origGlowSize;
                childPlanet = transform.Find("Planet2");

                Debug.Log(name + "," + childPlanet);
                //if (childPlanet.childCount > 0)
                    //subChild = childPlanet.GetChild(0);
            }

            //Debug.Log("Shrink");
            transform.GetChild(0).transform.localScale = origScale * scaleFactor;
            //dust exist, not been aborbed
            childPlanet.localScale = origChildScale * scaleFactor;
            if(childPlanet.childCount > 0){
                for (int i = 0; i < childPlanet.childCount; i ++){
                    childPlanet.GetChild(i).localScale = origChildScale * scaleFactor;
                }
            }
                //subChild.localScale = origChildScale * scaleFactor;
            transform.GetComponent<Planet>().catchRadius = origCatchRadius * scaleFactor;
            transform.GetComponent<Planet>().slowRespTime = origSlowResTime * scaleFactor;
            bottomSprite.localScale = origBottomScale * scaleFactor;
            //Debug.Log(bottomSprite);

            gameObject.GetComponent<Planet>().lightController.range = origHaloSize * scaleFactor;
            gameObject.GetComponent<Planet>().origGlowSize = origGlowSizeShrink * scaleFactor;

            //Make sure when it shrinks to some extremely small scale it should not catch player 
            if (childPlanet.localScale.x < 0.2f && transform.GetComponent<Planet>().isActiveAndEnabled)
                transform.GetComponent<Planet>().enabled = false;
            else if(!transform.GetComponent<Planet>().isActiveAndEnabled)
                transform.GetComponent<Planet>().enabled = true;


            //If the size reaches bounds then change the direction
            if ((scaleFactor <= lowerBound || scaleFactor >= 1))
            {
                //When reaching bounds, stay there for a while
                change_rate = -change_rate;
                float num = Random.Range(0.2f, period);
                //Debug.Log(num);
                waitTime = num;
            }
            yield return new WaitForSeconds(waitTime);
            if (waitTime > 0.1f)
                waitTime = 0.01f;
            scaleFactor += change_rate;

        }

        startShrink = true;
    }

}
