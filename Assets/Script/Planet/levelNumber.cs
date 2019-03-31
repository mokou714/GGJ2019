using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelNumber : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {
        for(int i=0; i<transform.childCount; ++i) {
            transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 0f);
        }

        StartCoroutine(fadeIn());
    }

    private void OnDisable()
    {
        StopCoroutine(fadeIn());
        for (int i = 0; i < transform.childCount; ++i)
        {
            transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 1f);
        }
    }

    IEnumerator fadeIn()
    {
        float currentAlpha = 0f;
        int dir = 1;

        while (true)
        {
            if (currentAlpha <= 0f)
                dir = 1;
            else if(currentAlpha >= 1f)
                dir = -1;
                
            currentAlpha = transform.GetChild(0).GetComponent<SpriteRenderer>().color.a;
            float newAlpha = currentAlpha + 0.05f * dir;

            for (int i = 0; i < transform.childCount; ++i)
            {
                transform.GetChild(i).GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, newAlpha);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}
