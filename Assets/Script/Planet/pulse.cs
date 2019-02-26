using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pulse : MonoBehaviour {


    public float pulseSpeed;

    public float scaleFactor;
    bool startPulse = false;
    Vector3 defaultScale;
    private bool stop = false;
    public float time = 0.01f;

    float T = 0;

    GameObject player;



    void Start(){
        scaleFactor = 1f;
        defaultScale = transform.lossyScale;

        StartCoroutine(Scale());
        //StartCoroutine(Pulse());
    }


    // Update is called once per frame
    void Update () {
        player = GetComponent<dustPlanet>().thePlayerOnPlanet;
        if (player == null)
        {
            T += Time.deltaTime * pulseSpeed;
            scaleFactor = Mathf.Cos(T) * 1 / 100 + 1;
            if (Mathf.Abs(Mathf.Cos(T)-1f) < 0.0001f)
                Debug.Log("scaleFactor " + scaleFactor);
        }

    }

    IEnumerator Scale() {
        while (true){

            if (player == null)
            {

                //They all change at the same time
                if (transform.childCount > 1){
                    transform.GetChild(0).transform.localScale *= scaleFactor;
                    transform.GetChild(1).transform.localScale *= scaleFactor;
                    transform.GetComponent<dustPlanet>().catchRadius = transform.GetComponent<dustPlanet>().catchRadius * scaleFactor;
                }
                if (player != null)
                {
                    Vector3 newPos = transform.position - (transform.position - player.transform.position) * scaleFactor;
                    player.transform.position = newPos;
                }
            }
            yield return new WaitForSeconds(time);
        }

    }



}
