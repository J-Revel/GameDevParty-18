using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public Foldable leftCharacterFoldable;
    public Foldable rightCharacterFoldable;
    public Foldable leftBubbleFoldable;
    public Foldable rightBubbleFoldable;
    public Foldable themeSelectorFoldable;
    public Foldable idCardFoldable;
    public ThemeSelector themeSelector;

    private PNJProfile pnj;

    public TMPro.TextMeshProUGUI questionText;
    public TMPro.TextMeshProUGUI answerText;
    public TMPro.TextMeshProUGUI idCardText;
    public Image talkerImage;
    private bool canAskQuestion = false;

    private AudioSource audioSource;
    public System.Action themeSelectedDelegate;
    public System.Action questionAskedDelegate;
    public System.Action answerGivenDelegate;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StartDialogue(PNJProfile pnj)
    {
        this.pnj = pnj;
        StartCoroutine(DialogueCoroutine());
    }

    private IEnumerator DialogueCoroutine()
    {
        talkerImage.sprite = pnj.dialogueSprite;
        leftCharacterFoldable.isOn = true;
        rightCharacterFoldable.isOn = true;
        yield return new WaitForSeconds(0.2f);
        idCardText.text = pnj.idCard;
        idCardFoldable.isOn = true;
        yield return new WaitForSeconds(0.3f);
        themeSelectorFoldable.isOn = true;
        canAskQuestion = true;
        audioSource.Play();
    }

    public void CloseDialogue()
    {
        StopAllCoroutines();
        canAskQuestion = false;
        audioSource.Stop();
        leftCharacterFoldable.isOn = false;
        rightCharacterFoldable.isOn = false;
        themeSelectorFoldable.isOn = false;
        leftBubbleFoldable.isOn = false;
        rightBubbleFoldable.isOn = false;
        idCardFoldable.isOn = false;
    }

    public void OnTalkButtonPressed()
    {
        if(canAskQuestion)
        {
            canAskQuestion = false;
            audioSource.Stop();
            themeSelectedDelegate?.Invoke();
            StartCoroutine(themeSelector.SelectThemeCoroutine(pnj, OnBubbleTextSelected));
        }
    }

    public void OnBubbleTextSelected(string question, string answer)
    {
        StartCoroutine(BubbleDisplayCoroutine(question, answer));
    }

    private IEnumerator BubbleDisplayCoroutine(string question, string answer)
    {
        canAskQuestion = false;
        audioSource.Stop();
        themeSelectorFoldable.isOn = false;

        Coroutine leftBubbleCoroutine = null;
        Coroutine rightBubbleCoroutine = null;
        if(leftBubbleFoldable.isOn)
            leftBubbleCoroutine = StartCoroutine(leftBubbleFoldable.CloseCoroutine());
        if(rightBubbleFoldable.isOn)
            rightBubbleCoroutine = StartCoroutine(rightBubbleFoldable.CloseCoroutine());

        if(leftBubbleCoroutine != null)
            yield return leftBubbleCoroutine;
        if(rightBubbleCoroutine != null)
            yield return rightBubbleCoroutine;

        questionText.text = question;
        answerText.text = answer;

        leftBubbleFoldable.isOn = true;
        questionAskedDelegate?.Invoke();
        yield return new WaitForSeconds(0.7f);
        rightBubbleFoldable.isOn = true;
        answerGivenDelegate?.Invoke();
        yield return new WaitForSeconds(0.3f);
        themeSelector.playing = true;
        audioSource.Play();
        themeSelectorFoldable.isOn = true;
        canAskQuestion = true;
        
    }
}
