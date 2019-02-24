using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pulse : MonoBehaviour {


    public float pulseSpeed;

    public float scaleFactor;
    bool startPulse = false;
    Vector3 defaultScale;
    public bool stop = false;
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
        }



    }

    IEnumerator Scale() {
        while (true){
            if (player == null)
            {
                transform.GetChild(0).transform.localScale *= scaleFactor;
                //dust exist, not been aborbed
                if (transform.childCount > 1)
                    transform.GetChild(1).transform.localScale *= scaleFactor;
                transform.GetComponent<dustPlanet>().catchRadius *= scaleFactor;
                if (player != null)
                {
                    Vector3 newPos = transform.position - (transform.position - player.transform.position) * scaleFactor;
                    player.transform.position = newPos;
                }
            }
            yield return new WaitForSeconds(time);
        }

    }

    //IEnumerator Pulse() {
    //    bool _pulsing = true;
    //    bool _rollBack = false;
    //    float _initScaleFactor = scaleFactor;
    //    while (true)
    //    {
    //        //Every x second, pulse
    //        if (startPulse)
    //        {
    //            //speed up
    //            if (_pulsing)
    //            {
    //                if (scaleFactor < 0)
    //                    scaleFactor = _initScaleFactor;

    //                scaleFactor += 0.1f;
    //                _pulsing = false;
    //            }

    //            yield return new WaitForSeconds(0.1f);




    //            //speed down/rollback
    //            if (!_pulsing)
    //            {
    //                scaleFactor -= 0.1f;
    //                Debug.Log("half");
    //                yield return new WaitForSeconds(0.05f);


    //                scaleFactor -= 0.05f;
    //                yield return new WaitForSeconds(0.1f);
    //                scaleFactor += 0.05f;
    //                _pulsing = true;
    //            }



    //            startPulse = false;

    //        }
    //        else {
    //            yield return new WaitForSeconds(3f);
    //            startPulse = true;
                    
    //        }
    //    }
    //}

    //IEnumerator pauseforSec(float time){
    //    yield return new WaitForSeconds(time);

    //}


}
