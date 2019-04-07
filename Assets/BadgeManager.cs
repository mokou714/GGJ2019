using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadgeManager : MonoBehaviour
{
    public static BadgeManager instance;
    // Use this for initialization
    private List<GameObject> goldenPlanets = new List<GameObject>();
    public spacecraft sc_player;
    public bool randomPick = true;

    public GameObject hitKey;
    private bool given = false;

    public int maxNum = 2;

    public int prob = 4;

    Dictionary<string, int> nameIndexMap = 
        new Dictionary<string, int>(){{"aquarius", 0}, 
        {"pisces", 2}, {"sagittarius", 3}, 
        {"cancer", 4}};

	void Start () {
        if (instance == null)
            instance = this;
        if(randomPick){
            int num = Random.Range(0, 10);
            if (num < prob)
            {
                int index = Random.Range(1, maxNum + 1);
                transform.GetChild(index).gameObject.SetActive(true);
                transform.GetChild(index).gameObject.GetComponent<GoldenPlanet>().initBadge();
                given = true;
            }
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void showHiddenPlanet(string name){
        if(nameIndexMap.ContainsKey(name) && ! given){
            given = true;
            int index = Random.Range(1, maxNum);
            transform.GetChild(index).gameObject.SetActive(true);
            transform.GetChild(index).gameObject.GetComponent<GoldenPlanet>().initBadge(random: false, sprite_index: nameIndexMap[name]);
        }
    }
}
