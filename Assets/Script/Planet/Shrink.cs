using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrink : MonoBehaviour {

    /*
    Shrink Planet: this planet will shrink periodically
    */
    public float scaleFactor;
    bool startPulse = false;
    Vector3 defaultScale;
    public float lowerBound;

    private Vector3 origScale;
    private Vector3 origChildScale;
    private float origCatchRadius;

    public float change_rate = 0.005f;
    public float period;
    private float waitTime = 0.01f;


    GameObject player;


    void Start(){
        scaleFactor = 1f;

        defaultScale = transform.localScale;

        //Record the original scale for each component for later resizing
        origScale = transform.GetChild(0).transform.localScale;
        origChildScale = transform.GetChild(1).transform.localScale;
        origCatchRadius = transform.GetComponent<dustPlanet>().catchRadius;

        StartCoroutine(Scale());
    }


    // Update is called once per frame
    void Update () {

    }

    //rescale every 0.01 second 
    IEnumerator Scale() {
        while (true){
            if (player == null)
            {
                transform.GetChild(0).transform.localScale = origScale * scaleFactor;
                //dust exist, not been aborbed
                if (transform.childCount > 1)
                    transform.GetChild(1).transform.localScale = origChildScale * scaleFactor;
                transform.GetComponent<dustPlanet>().catchRadius = origCatchRadius * scaleFactor;

                if (player != null)
                {
                    Vector3 newPos = transform.position - (transform.position - player.transform.position) * scaleFactor;
                    player.transform.position = newPos;
                }
            }

            //If the size reaches bounds then change the direction
            if ((scaleFactor <= lowerBound || scaleFactor >= 1))
            {
                //When reaching bounds, stay there for a while
                change_rate = -change_rate;
                waitTime = period;
            }
            yield return new WaitForSeconds(waitTime);
            if (waitTime > 0.1f)
                waitTime = 0.01f;
            scaleFactor += change_rate; 
        }

    }

}
