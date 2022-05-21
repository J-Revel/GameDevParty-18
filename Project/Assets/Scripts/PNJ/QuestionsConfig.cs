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

[System.Serializable]
public struct QuestionConfig
{
    public string question;
    public string answerYes;
    public string answerNo;
    public string answerIndecisive;
}

[CreateAssetMenu]
public class QuestionsConfig : ScriptableObject
{
    public QuestionConfig[] questions;
}
