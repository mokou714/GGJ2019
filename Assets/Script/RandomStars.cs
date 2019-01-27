using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomStars : MonoBehaviour {

	[SerializeField]
	private int numberOfStars;

	[SerializeField] [Range(0, 5)] 
	private float minScaleChange = 1;

	[SerializeField] [Range(0, 5)]
	private float maxScaleChange = 1;

	[SerializeField] [Range(0, 360)] 
	private float minRotationChange;

	[SerializeField] [Range(0, 360)]
	private float maxRotationChange;

	[SerializeField] [Range(-5, 5)]
	private float minPositionChange;

	[SerializeField] [Range(-5, 5)]
	private float maxPositionChange;

	// Use this for initialization
	void Start () {
		maxScaleChange = Mathf.Max(minScaleChange, maxScaleChange);
		maxRotationChange = Mathf.Max(minRotationChange, maxRotationChange);
		maxPositionChange = Mathf.Max(minPositionChange, maxPositionChange);

		GameObject star = transform.GetChild(0).gameObject;
		formStars(numberOfStars, star);
		offsetTransform();
	}

	private void offsetTransform() {
		foreach (Transform star in transform) {
			star.localScale *= Random.Range(minScaleChange, maxScaleChange);
			star.localRotation = offsetRotation(star);
			star.localPosition = offsetPosition(star);
		}
	}

	private Quaternion offsetRotation(Transform star) {
		Vector3 rotation = star.localRotation.eulerAngles;
		rotation.z += Random.Range(minRotationChange, maxRotationChange);
		return Quaternion.Euler(rotation);
	}

	private Vector2 offsetPosition(Transform star) {
		Vector2 currPos = star.localPosition;
		Vector2 offset = new Vector2(
			currPos.x += Random.Range(minPositionChange, maxPositionChange),
			currPos.y += Random.Range(minPositionChange, maxPositionChange)
		);
		return offset;
	}

	private void formStars(int numberOfStars, GameObject star) {
		do {
			GameObject newStar = Instantiate(star, transform);
			int width = Camera.main.pixelWidth;
			int height = Camera.main.pixelHeight;
			Vector2 starPos = new Vector2(
				Random.Range(0, width), 
				height
			);
			newStar.transform.position = starPos;
		} while (numberOfStars-- > 0);
	}
}