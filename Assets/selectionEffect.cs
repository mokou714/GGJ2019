using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectionEffect : MonoBehaviour {


    public float largeScale;
    Vector3 originScale;

  

	// Use this for initialization
	void Start () {
        originScale = transform.GetChild(0).localScale;
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    IEnumerator fadeIn() {
        float currentAlpha = 0f;

        while (currentAlpha < 1f)
        {
            currentAlpha = GetComponent<SpriteRenderer>().color.a;
            float newAlpha = currentAlpha + 0.05f;
            GetComponent<SpriteRenderer>().color = new Color(255f,255f,255f,newAlpha);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator fadeOut()
    {
        float currentAlpha = 1f;

        while (currentAlpha > 0f)
        {
            currentAlpha = GetComponent<SpriteRenderer>().color.a;
            float newAlpha = currentAlpha - 0.05f;
            GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, newAlpha);
            yield return new WaitForSeconds(0.1f);
        }
    }




    private void OnMouseEnter()
    {
        transform.GetChild(0).localScale = new Vector3(largeScale, largeScale, 0f);


    }

    private void OnMouseExit()
    {

        transform.GetChild(0).localScale = originScale;

    }

}
