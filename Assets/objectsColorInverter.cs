using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectsColorInverter : MonoBehaviour {



    public bool invertingEnabled;

	// Use this for initialization
	void Start () {
        if (invertingEnabled)
        {
            StartCoroutine(invert());
        }
    }
	
	// Update is called once per frame
	void Update () {

    }

    IEnumerator invert()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 4.5f));

            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(0.2f);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;


        }
    }

}
