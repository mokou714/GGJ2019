// Animations of the frame

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFrame : MonoBehaviour
{

    public RectTransform[] cornorRTs;

    public RectTransform topLineRT;
    public RectTransform rightLineRT;
    public RectTransform bottomLineRT;
    public RectTransform leftLineRT;

    public RectTransform topMaskRT;
    public RectTransform rightMaskRT;
    public RectTransform bottomMaskRT;
    public RectTransform leftMaskRT;

    public Image[] cornorImages;
    public Image[] lineImages;
    public Image[] lineMaskImages;

    Color32 transparent = new Color32(255, 255, 255, 0);
    Color32 white = new Color32(255, 255, 255, 255);
    Color32 black = new Color32(0, 0, 0, 255);

    public float lineFadeDuration = 0.5f;
    public float fadeOutDuration = 0.5f;
    public float lineShowTime = 2f;
    public float dotSize = 10;

    public float dotFadeDuration = 1f;

    public float breathTime = 0.5f;

    public void DisplayOnlyEdges()
    {
        foreach (Image i in cornorImages)
            i.color = transparent;

        foreach (Image i in lineImages)
            i.color = white;

        foreach (Image i in lineMaskImages)
            i.color = transparent;
    }

    public void ShowFrame()
    {
        StartCoroutine(Show());
    }

    public void HideLines()
    {
        foreach (Image i in lineImages)
            i.color = transparent;
    }


    public IEnumerator FadeOut()
    {
        // fade out line images and corner images
        // alpha
        float alpha = 255;
        foreach (Image i in lineImages)
            i.color = white;

        while (alpha > 0.1f)
        {
            alpha -= Time.unscaledDeltaTime / dotFadeDuration * 255f;
            foreach (Image i in cornorImages)
                i.color = new Color(255, 255, 255, (byte)alpha);

            yield return null;
        }

        foreach (Image i in lineImages)
            i.color = transparent;

        StartCoroutine(Show());
    }

    public IEnumerator Show()
    {

        //print("show");

        // initialize alpha value for cornorImages
        float alpha = 0;
        foreach (Image i in cornorImages)
            i.color = transparent;

        // size
        float size = 0;
        foreach (RectTransform rt in cornorRTs)
            rt.sizeDelta = Vector2.zero;


        //print("show1");

        // fade-in the cornors
        while (alpha < 255f)
        {            
            alpha += Time.unscaledDeltaTime / dotFadeDuration * 255f;

            foreach (Image i in cornorImages)
                i.color = new Color32(255,255,255,(byte)alpha);
            
            size += Time.unscaledDeltaTime / dotFadeDuration * dotSize;
            foreach (RectTransform rt in cornorRTs)
                rt.sizeDelta = Vector2.one * size;


            yield return null;
        }

        //print("show2");


        // end of fade-in the cornors
        foreach (Image i in cornorImages)
            i.color = white;

        foreach (RectTransform rt in cornorRTs)
            rt.sizeDelta = Vector2.one * dotSize;


        // default line colors, sizes and pivots

        foreach (Image i in lineImages)
            i.color = white;
        foreach (Image i in lineMaskImages)
            i.color = black;

        topMaskRT.sizeDelta = topLineRT.sizeDelta;
        rightMaskRT.sizeDelta = rightLineRT.sizeDelta;
        bottomMaskRT.sizeDelta = bottomLineRT.sizeDelta;
        leftMaskRT.sizeDelta = leftLineRT.sizeDelta;

        // pivots affect the directions of animation
        topMaskRT.pivot = new Vector2(1, 0.5f);
        rightMaskRT.pivot = new Vector2(0.5f, 0);  // bottom
        bottomMaskRT.pivot = new Vector2(0, 0.5f); // right
        leftMaskRT.pivot = new Vector2(0.5f, 1); // up

        //print("topMaskRT.sizeDelta: " + topMaskRT.sizeDelta);
        //print("rightMaskRT.sizeDelta: " + rightMaskRT.sizeDelta);
        //print("bottomMaskRT.sizeDelta: " + bottomMaskRT.sizeDelta);
        //print("leftMaskRT.sizeDelta: " + leftMaskRT.sizeDelta);


        // calculate duration for each line
        float x = topMaskRT.sizeDelta.x;
        float y = rightMaskRT.sizeDelta.y;
        float t1 = lineShowTime / 2 * x / (x + y);
        float t2 = lineShowTime / 2 * y / (x + y);

        //print("t1: " + t1 + ", t2: " + t2);
        // remove line masks:
        // top
        while (topMaskRT.sizeDelta.x > 0.1f)
        {
            topMaskRT.sizeDelta -= Vector2.right * Time.unscaledDeltaTime / t1 * x;
            yield return null;
        }
        topMaskRT.sizeDelta = Vector2.zero;

        //print("show right");

        while (rightMaskRT.sizeDelta.y > 0.1f)
        {
            rightMaskRT.sizeDelta -= Vector2.up * Time.unscaledDeltaTime / t2 * y;
            yield return null;
        }
        rightMaskRT.sizeDelta = Vector2.zero;

        //print("show bottom");

        while (bottomMaskRT.sizeDelta.x > 0.1f)
        {
            bottomMaskRT.sizeDelta -= Vector2.right * Time.unscaledDeltaTime / t1 * x;
            yield return null;
        }
        bottomMaskRT.sizeDelta = Vector2.zero;

        //print("show left");
 
        while (leftMaskRT.sizeDelta.y > 0.1f)
        {
            leftMaskRT.sizeDelta -= Vector2.up * Time.unscaledDeltaTime / t2 * y;
            yield return null;
        }
        leftMaskRT.sizeDelta = Vector2.zero;


    }
}