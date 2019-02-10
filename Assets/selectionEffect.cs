using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectionEffect : MonoBehaviour {


    public float largeScale;
    Vector3 originScale;

	// Use this for initialization
	void Start () {
        originScale = transform.localScale;
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void OnMouseEnter()
    {
        transform.localScale = new Vector3(largeScale, largeScale, 0);
    }

    private void OnMouseExit()
    {
        transform.localScale = originScale;
    }



}
