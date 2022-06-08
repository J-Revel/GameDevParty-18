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
    public bool playing = false;
    public Image[] elements;
    private Color[] elementColors;
    public Color highlightColor;
    public Color blinkColor;

    public float blinkAnimFreq = 5;
    public float blinkAnimDuration = 3;
    public float barSlideDuration = 0.5f;
    private float visibleBarY;

    


    public QuestionsConfig[] configs;

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
        }
    }

    public IEnumerator SelectThemeCoroutine(PNJProfile pnj, System.Action<string, string> bubbleContentDelegate)
    {
        playing = false;
        Debug.Log("playing = false");
        for(float time=0; time < blinkAnimDuration; time += Time.deltaTime)
        {
            elements[selectedIndex].color = Mathf.Floor(time * blinkAnimFreq) % 2 == 0 ? highlightColor : blinkColor;
            yield return null;
        }
        QuestionTheme[] themes = new QuestionTheme[]{QuestionTheme.Economy, QuestionTheme.Ecology, QuestionTheme.Social, QuestionTheme.Politics, QuestionTheme.Culture};
        QuestionConfig question = GetRandomQuestion(pnj.genre, themes[selectedIndex]);
        string answer;
        if(!pnj.IsThemeFavourite(question.theme))
        {
            answer = question.answersIndecisive[Random.Range(0, question.answersIndecisive.Length)];
        }
        else if(pnj.leftWing)
        {
            answer = question.answersAgree[Random.Range(0, question.answersAgree.Length)];
        }
        else
        {
            answer = question.answersDisagree[Random.Range(0, question.answersDisagree.Length)];
        }
        bubbleContentDelegate(question.question, answer);
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
}
