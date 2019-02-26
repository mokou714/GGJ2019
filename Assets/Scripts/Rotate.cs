using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Rotate : MonoBehaviour {

	[SerializeField]
	private float degreesASecond = 10f;
	
	// Update is called once per frame
	void Update () {
		Vector3 newRot = transform.rotation.eulerAngles;
		newRot.z += degreesASecond * Time.deltaTime;
		transform.rotation = Quaternion.Euler(newRot);
	}
}
