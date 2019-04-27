using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelController : MonoBehaviour {


    public GameObject textTutorial;
	// Use this for initialization
	void Start () {         
        int progress = GameStates.instance.getUnlockedLevels();
        Debug.Log("Progress:" + progress);
        if (progress > 0){

            //textTutorial.SetActive(false);

            int bigLevel = (progress - 1) / 10 + 1;
            for (int i = 1; i < bigLevel + 1; i++){
                Transform child = transform.Find(i.ToString());
                if(child != null){
                    child.gameObject.SetActive(true);
                    if ((int)GameStates.instance.getData("showed" + i.ToString(), typeof(int)) == 0){
                        child.GetComponent<levelPlanet>().showUp();
                        GameStates.instance.saveData("showed" + i.ToString(), 1);
                    }
                    child.gameObject.GetComponent<levelPlanet>().progress = progress;
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
