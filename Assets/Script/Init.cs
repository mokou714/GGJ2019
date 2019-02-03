using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Awake(){
        /*
        Todo: initialization for background music
        */
        GameObject[] audio_objs = GameObject.FindGameObjectsWithTag("background");

        if (audio_objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);


        GameObject[] playBGM = GameObject.FindGameObjectsWithTag("bgm");
        DontDestroyOnLoad(this.gameObject);
    }

}
