using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingScreen : MonoBehaviour
{
    public int leftVotes;
    public int rightVotes;
    public TMPro.TextMeshProUGUI text;
    public Image victoryImage;
    public Image defeatImage;

    void Start()
    {
        text.text = "Résultat final :\n Drouate " + rightVotes + "\nGôche " + leftVotes;
        victoryImage.gameObject.SetActive(leftVotes > rightVotes);
        defeatImage.gameObject.SetActive(leftVotes <= rightVotes);
    }
}
