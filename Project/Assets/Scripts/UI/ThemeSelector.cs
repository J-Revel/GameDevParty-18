using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemeSelector : MonoBehaviour
{
    public RectTransform cursor;
    private RectTransform rectTransform;
    public float oscillationDuration = 1;
    private float animTime = 0;
    public bool playing = true;
    public Image[] elements;
    private Color[] elementColors;
    public Color highlightColor;
    public Color blinkColor;

    public float blinkAnimFreq = 5;
    public float blinkAnimDuration = 3;
    public float barSlideDuration = 0.5f;
    public float hiddenBarY = -150;
    private float visibleBarY;

    public Transform questionTransform;
    public Transform answerTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        visibleBarY = rectTransform.anchoredPosition.y;
        elementColors = new Color[elements.Length];
        for(int i=0; i<elements.Length; i++)
            elementColors[i] = elements[i].color;
    }

    private int selectedIndex { get {
        float animRatio = 1 - Mathf.Abs(((animTime % oscillationDuration) / oscillationDuration) * 2 - 1);
        return Mathf.FloorToInt(animRatio * elements.Length);
    }}

    void Update()
    {
        if(playing)
        {
            animTime += Time.deltaTime;
            float animRatio = 1 - Mathf.Abs(((animTime % oscillationDuration) / oscillationDuration) * 2 - 1);
            for(int i=0; i<elements.Length; i++)
            {
                if(Mathf.FloorToInt(animRatio * elements.Length) == i)
                {
                    elements[i].transform.localScale = Vector3.one * 1.05f;
                    elements[i].color = highlightColor;
                    elements[i].transform.SetAsLastSibling();
                }
                else
                {
                    elements[i].transform.localScale = Vector3.one;
                    elements[i].color = elementColors[i];
                }
            }

            cursor.anchorMax = new Vector2(animRatio, 1);
            cursor.anchorMin = new Vector2(animRatio, 0);
            if(Input.GetButtonDown("Talk"))
            {
                playing = false;
                StartCoroutine(SelectThemeCoroutine());
            }
        }
    }

    public IEnumerator AppearCoroutine()
    {
        playing = true;
        for(float time=0; time < barSlideDuration; time += Time.deltaTime)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(new Vector2(rectTransform.anchoredPosition.x, hiddenBarY), new Vector2(rectTransform.anchoredPosition.x, visibleBarY), time / barSlideDuration);
            yield return null;
        }
    }

    public IEnumerator SelectThemeCoroutine()
    {
        for(float time=0; time < blinkAnimDuration; time += Time.deltaTime)
        {
            elements[selectedIndex].color = Mathf.Floor(time * blinkAnimFreq) % 2 == 0 ? highlightColor : blinkColor;
            yield return null;
        }
        for(float time=0; time < barSlideDuration; time += Time.deltaTime)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(new Vector2(rectTransform.anchoredPosition.x, visibleBarY), new Vector2(rectTransform.anchoredPosition.x, hiddenBarY), time / barSlideDuration);
            yield return null;
        }
        // for(float time=0; )
        yield return AppearCoroutine();
    }
}
