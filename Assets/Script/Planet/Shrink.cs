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

    private bool stop = false;
    private Transform child;

    void Start(){
        scaleFactor = 1f;

        defaultScale = transform.localScale;

        //Record the original scale for each component for later resizing
        origScale = transform.GetChild(0).transform.localScale;

        if(tag == "dustPlanet"){
            origChildScale = transform.GetChild(2).transform.localScale;
            child = transform.GetChild(2);
        }else{
            origChildScale = transform.GetChild(1).transform.localScale;
            child = transform.GetChild(1);
        }
            

        origCatchRadius = transform.gameObject.GetComponent<Planet>().catchRadius;
    }


    // Update is called once per frame
    void Update () {
        if(transform.GetComponent<Planet>().thePlayerOnPlanet == null && !stop){
            stop = true;
            StartCoroutine(Scale());
        }
    }


    //rescale every 0.01 second 
    IEnumerator Scale() {
        while(transform.GetComponent<Planet>().thePlayerOnPlanet == null){

                //Debug.Log("Shrink");
                transform.GetChild(0).transform.localScale = origScale * scaleFactor;
                //dust exist, not been aborbed
                //if (transform.childCount > 1)
                child.localScale = origChildScale * scaleFactor;
                transform.GetComponent<Planet>().catchRadius = origCatchRadius * scaleFactor;

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

        stop = false;
    }


}
