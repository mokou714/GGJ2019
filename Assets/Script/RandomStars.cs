using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomStars : MonoBehaviour {

	[SerializeField]
	private int numberOfStars = 55;

	[SerializeField] [Range(0, 5)] 
	private float minScaleChange = .1f;

	[SerializeField] [Range(0, 5)]
	private float maxScaleChange = 1.15f;

	[SerializeField] [Range(0, 360)] 
	private float minRotationChange = 0;

	[SerializeField] [Range(0, 360)]
	private float maxRotationChange = 360f;

	[SerializeField] [Range(-5, 5)]
	private float minPositionChange = -1f;

	[SerializeField] [Range(-5, 5)]
	private float maxPositionChange = 1f;

    public bool blinkStars;
    private List<GameObject> starList = new List<GameObject>();

	// Use this for initialization
	void Start () {
		maxScaleChange = Mathf.Max(minScaleChange, maxScaleChange);
		maxRotationChange = Mathf.Max(minRotationChange, maxRotationChange);
		maxPositionChange = Mathf.Max(minPositionChange, maxPositionChange);

		GameObject star = transform.GetChild(0).gameObject;
		formStars(numberOfStars, star);
		offsetTransform();

        if (blinkStars)
            StartCoroutine(blink());
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
            float heightEx = Camera.main.orthographicSize;
            float widthEx = heightEx * Camera.main.aspect;

            Vector2 starPos = new Vector2(
                Random.Range(-widthEx, widthEx),
                Random.Range(-heightEx, heightEx)
			);
			newStar.transform.position = starPos;
            starList.Add(newStar);
		} while (numberOfStars-- > 0);
	}

    IEnumerator blink()
    {
        List<int> allStarsIdx = new List<int>();
        while (true)
        {
            int numStars = numberOfStars;

            foreach (int i in allStarsIdx)
            {
                starList[i].GetComponent<SpriteRenderer>().enabled = true;
            }
            allStarsIdx = new List<int>();
            while (numStars > numberOfStars / 2) {
                int starIdx = Random.Range(0, numberOfStars);
                allStarsIdx.Add(starIdx);
                starList[starIdx].GetComponent<SpriteRenderer>().enabled = false;
                numStars--;
            }
            yield return new WaitForSeconds(1f);
        }
    }
}