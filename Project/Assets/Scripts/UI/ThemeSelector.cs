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

    public float bubbleAnimFPS = 20;
    public float answerAppearDelay = 1;
    public float[] bubbleAnimAppearScales = new float[]{0.3f, 0.8f, 1.1f, 0.9f, 1};
    public float[] bubbleAnimDisappearScales = new float[]{0.9f, 1.1f, 0.8f, 0.3f, 0};
    public RectTransform leftPartTransform;
    public RectTransform rightPartTransform;
    public RectTransform idCardRectTransform;

    public QuestionsConfig[] configs;

    private TMPro.TextMeshProUGUI questionText;
    private TMPro.TextMeshProUGUI answerText;
    public TMPro.TextMeshProUGUI idCardText;
    private PNJProfile pnj;

    public Image pnjImage;

    private bool bubblesVisible = false;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        visibleBarY = rectTransform.anchoredPosition.y;
        elementColors = new Color[elements.Length];
        for(int i=0; i<elements.Length; i++)
            elementColors[i] = elements[i].color;
        
        questionTransform.localScale = Vector3.zero;
        answerTransform.localScale = Vector3.zero;
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, hiddenBarY);
        rightPartTransform.pivot = new Vector2(0, 0.5f);
        leftPartTransform.pivot = new Vector2(1, 0.5f);
        questionText = questionTransform.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        answerText = answerTransform.GetComponentInChildren<TMPro.TextMeshProUGUI>();
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
        }
    }

    public IEnumerator ShowSelectorCoroutine()
    {
        playing = true;
        for(float time=0; time < barSlideDuration; time += Time.deltaTime)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(new Vector2(rectTransform.anchoredPosition.x, hiddenBarY), new Vector2(rectTransform.anchoredPosition.x, visibleBarY), time / barSlideDuration);
            yield return null;
        }
    }
    
    public IEnumerator HideSelectorCoroutine()
    {
        playing = true;
        for(float time=0; time < barSlideDuration; time += Time.deltaTime)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(new Vector2(rectTransform.anchoredPosition.x, visibleBarY), new Vector2(rectTransform.anchoredPosition.x, hiddenBarY), time / barSlideDuration);
            yield return null;
        }
    }

    private IEnumerator DisappearBubblesCoroutine()
    {
        for(int i=0; i<bubbleAnimDisappearScales.Length; i++)
        {
            questionTransform.localScale = Vector3.one * bubbleAnimDisappearScales[i];
            answerTransform.localScale = Vector3.one * bubbleAnimDisappearScales[i];
            yield return new WaitForSeconds(1 / bubbleAnimFPS);
        }
    }

    private IEnumerator AppearCoroutine()
    {
        float duration = 0.5f;
        for(float time = 0; time < duration; time += Time.deltaTime)
        {
            rightPartTransform.pivot = new Vector2(time / duration, 0.5f);
            leftPartTransform.pivot = new Vector2(1 - time / duration, 0.5f);
            yield return null;
        }
        for(float time = 0; time < duration; time += Time.deltaTime)
        {
            idCardRectTransform.pivot = new Vector2(1, 0.5f + (1 - time/duration));
            yield return null;
        }
        yield return ShowSelectorCoroutine();
    }

    public void CloseMenu()
    {
        StartCoroutine(DisappearCoroutine());
    }

    private IEnumerator DisappearCoroutine()
    {
        StartCoroutine(HideSelectorCoroutine());
        if(bubblesVisible)
        {
            StartCoroutine(DisappearBubblesCoroutine());
        }
        float duration = 0.5f;
        for(float time = 0; time < duration; time += Time.deltaTime)
        {
            rightPartTransform.pivot = new Vector2(1 - time / duration, 0.5f);
            leftPartTransform.pivot = new Vector2(time / duration, 0.5f);
            yield return null;
        }
    }

    public IEnumerator SelectThemeCoroutine()
    {
        if(bubblesVisible)
        {
            StartCoroutine(DisappearBubblesCoroutine());
        }
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
        QuestionTheme[] themes = new QuestionTheme[]{QuestionTheme.Economy, QuestionTheme.Ecology, QuestionTheme.Social, QuestionTheme.Politics, QuestionTheme.Culture};
        QuestionConfig question = GetRandomQuestion(pnj.genre, themes[selectedIndex]);
        questionText.text = question.question;
        if(!pnj.IsThemeFavourite(question.theme))
        {
            answerText.text = question.answersIndecisive[Random.Range(0, question.answersIndecisive.Length)];
        }
        else if(pnj.leftWing)
        {
            answerText.text = question.answersAgree[Random.Range(0, question.answersAgree.Length)];
        }
        else
        {
            answerText.text = question.answersDisagree[Random.Range(0, question.answersDisagree.Length)];
        }
        for(int i=0; i<bubbleAnimAppearScales.Length; i++)
        {
            questionTransform.localScale = Vector3.one * bubbleAnimAppearScales[i];
            yield return new WaitForSeconds(1 / bubbleAnimFPS);
        }
        yield return new WaitForSeconds(answerAppearDelay);
        for(int i=0; i<bubbleAnimAppearScales.Length; i++)
        { 
            answerTransform.localScale = Vector3.one * bubbleAnimAppearScales[i];
            yield return new WaitForSeconds(1 / bubbleAnimFPS);
        }
        bubblesVisible = true;
        yield return ShowSelectorCoroutine();
    }

    public QuestionConfig GetRandomQuestion(Genre genre, QuestionTheme theme)
    {
        List<QuestionConfig> usableQuestions = new List<QuestionConfig>();
        for(int i=0; i<configs.Length; i++)
        {
            for(int j=0; j<configs[i].questions.Length; j++)
            {
                QuestionConfig question = configs[i].questions[j];
                if((question.genre == genre || question.genre == Genre.Both) && question.theme == theme)
                    usableQuestions.Add(question);
            }
        }
        return usableQuestions[Random.Range(0, usableQuestions.Count)];
    }

    public void StartDialogue(PNJProfile pnj)
    {
        this.pnj = pnj;
        pnjImage.sprite = pnj.dialogueSprite;
        idCardText.text = pnj.idCard;
        StartCoroutine(AppearCoroutine());
    }

    public void OnTalkButtonPressed()
    {
        playing = false;
        StartCoroutine(SelectThemeCoroutine());
    }
}
