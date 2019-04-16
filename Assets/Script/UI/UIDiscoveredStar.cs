using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIDiscoveredStar : MonoBehaviour {

    public GameObject unknown;
    public GameObject content;

    public UIFrame frame;

    public float contentFadeInTime = 2f;

    private void OnEnable()
    {
        if(unknown == null)
            unknown = transform.GetChild(0).gameObject;
        if (content == null)
            content = transform.GetChild(1).gameObject;
        print(name + ", " + PlayerPrefs.HasKey(name));
        // using name of the star as the key
        if (PlayerPrefs.HasKey(name))
        {
            int value = PlayerPrefs.GetInt(name);
            // moved to finishing a level
            if (value == 0)
            {
                // To do: sfx and animation
                StartCoroutine(ShowStar());
                AudioManager.instance.PlaySFX(.5f, "Unlocking");
            }
            else if (value == 1)
            {
                frame.gameObject.SetActive(true);
                unknown.SetActive(false);
                content.SetActive(true);
            }            
        }
        else
        {
            // display only the edges of the frame
            frame.gameObject.SetActive(true);
            frame.DisplayOnlyEdges();

            // display the unknown image
            unknown.SetActive(true);
            content.SetActive(false);
        }

        PlayerPrefs.Save();
    }

    IEnumerator ShowStar()
    {
        PlayerPrefs.SetInt(name, 1);

        frame.HideLines();

        unknown.SetActive(false);
        
        StartCoroutine(frame.Show());

        yield return new WaitForSecondsRealtime(frame.dotFadeDuration);

        // fade in content after dot appears
        content.SetActive(true);

        StartCoroutine(UIManager.instance.FadeInWhiteImages
            (contentFadeInTime, content.GetComponentsInChildren<Image>()));

        StartCoroutine(UIManager.instance.FadeInTexts
            (contentFadeInTime, content.GetComponentsInChildren<Text>()));

    }

    
}
