using UnityEngine;
using System.Collections;

public class Meteor : MonoBehaviour
{
	// Use this for initialization
	void Start()
	{

	}

	private void OnEnable()
	{
        Invoke( "DestroySelf", 10f);
	}

	// Update is called once per frame
	void Update()
	{
			
	}


    void DestroySelf()
    {
        Destroy(gameObject);
    }

	private void OnDestroy()
	{
		
	}
}
