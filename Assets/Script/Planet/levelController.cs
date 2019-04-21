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
                    child.gameObject.GetComponent<levelPlanet>().progress = progress;
                }
            }

            // play sfx when a new big level is unlocked
            if (!PlayerPrefs.HasKey("bigLevel") && bigLevel==1)
            {
                PlayerPrefs.SetInt("bigLevel", 1);
                AudioManager.instance.PlaySFX(2f, "Star");
            }
            else if (PlayerPrefs.HasKey("bigLevel"))
            {
                if(PlayerPrefs.GetInt("bigLevel") == 1 && bigLevel == 2)
                {
                    PlayerPrefs.SetInt("bigLevel", 2);
                    AudioManager.instance.PlaySFX(2f, "Star");
                }
            }

        }
	}
   
}
