using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestionTheme
{
    Economy,
    Ecology,
    Social,
    Politics,
    Culture,
}

public enum Genre
{
    Male,
    Female,
    Both,
}

[System.Serializable]
public struct QuestionConfig
{
    public QuestionTheme theme;
    public Genre genre;
    public AnswerType agreeAnswerSide;
    [TextArea(3,10)]
    public string question;
    public string[] answersAgree;
    public string[] answersDisagree;
    public string[] answersIndecisive;
}

[CreateAssetMenu]
public class QuestionsConfig : ScriptableObject
{
    public QuestionConfig[] questions;
}
