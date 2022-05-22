using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PNJState
{
    GoingToVote,
    Walking,
    Fleeing,
    Aggressive,
}

public enum AnswerType
{
    LeftWingAnswer,
    RightWingAnswer,
    DontKnowAnswer,
}

public class PNJProfile : MonoBehaviour
{

    public PNJState state;
    public bool leftWing;
    public Genre genre;

    public QuestionTheme[] favouriteThemes;

    public bool IsThemeFavourite(QuestionTheme theme)
    {
        for(int i=0; i<favouriteThemes.Length; i++)
        {
            if(favouriteThemes[i] == theme)
                return true;
        }
        return false;
    }

}
