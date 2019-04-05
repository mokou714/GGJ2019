using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScript : MonoBehaviour {

    private SpriteRenderer title;
	// Use this for initialization
	void Start () {
        //title = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void showTitle(){
        StartCoroutine(fadeIn(0));
    }


    IEnumerator fadeIn(float newAlpha)
    {
        if(title == null)
            title = GetComponent<SpriteRenderer>();
        title.color = new Color(title.color.r, title.color.g, title.color.b, newAlpha);
        yield return new WaitForSeconds(0.05f);
        if(newAlpha < 1)
            StartCoroutine(fadeIn(newAlpha + 0.05f));
        else{
            yield return new WaitForSeconds(1.5f);

            StartCoroutine(fadeOut(1));
        }
            
    }


    IEnumerator fadeOut(float newAlpha)
    {
        title.color = new Color(title.color.r, title.color.g, title.color.b, newAlpha);

        yield return new WaitForSeconds(0.05f);
        if (newAlpha > 0){
            
            StartCoroutine(fadeOut(newAlpha - 0.05f));
        }
     }
            
}
