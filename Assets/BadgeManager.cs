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

    Dictionary<string, int> nameIndexMap = 
        new Dictionary<string, int>(){{"aqua", 0}, {"libra", 1}, 
        {"pisces", 2}, {"sagi", 3}, 
        {"cancer", 4}, {"aries", 5}};

	void Start () {
        if (instance == null)
            instance = this;
        if(randomPick){
            int num = Random.Range(0, 10);
            if (num < 5)
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
            int index = Random.Range(1, maxNum);
            transform.GetChild(index).gameObject.SetActive(true);
            transform.GetChild(index).gameObject.GetComponent<GoldenPlanet>().initBadge(random: false, sprite_index: nameIndexMap[name]);
            given = true;
        }
    }
}
