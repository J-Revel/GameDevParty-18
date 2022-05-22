using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    public int allyVotes;
    public int enemyVotes;
    public PNJDestroyer pnjVoteHitbox;

    public System.Action cantVoteDelegate;
    public System.Action allyVoteDelegate;
    public System.Action enemyVoteDelegate;

    public Transform fxPrefab;
    public Transform fxPosition;
    public Color rightColor = Color.green;
    public Color wrongColor = Color.red;
    public Color cantVoteColor = Color.yellow;

    public TMPro.TextMeshProUGUI timerText;
    public float remainingTime = 60 * 3;
    public EndingScreen endingScreenPrefab;

    void Start()
    {
        pnjVoteHitbox.pnjDestroyDelegate += OnPNJVote;
    }

    void Update()
    {
        remainingTime -= Time.deltaTime;
        if(remainingTime < 0)
        {
            remainingTime = 0;
            EndingScreen endingScreen = Instantiate(endingScreenPrefab);
            endingScreen.leftVotes = allyVotes;
            endingScreen.rightVotes = enemyVotes;
            enabled = false;
        }
        timerText.text = (Mathf.FloorToInt(remainingTime / 60)).ToString("00") + ":" + (Mathf.FloorToInt(remainingTime % 60)).ToString("00");
    }

    private void OnPNJVote(PNJProfile pnj)
    {
        Transform fx = Instantiate(fxPrefab, fxPosition.position, fxPosition.rotation);
        TMPro.TextMeshProUGUI fxText = fx.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        if(!pnj.canVote)
        {
            cantVoteDelegate?.Invoke();
            fxText.text = "0 : pas le droit de voter !";
            fxText.color = cantVoteColor;
        }
        else if(pnj.leftWing)
        {
            allyVoteDelegate?.Invoke();
            allyVotes += pnj.procurationCount + 1;
            fxText.text = "+" + (1 + pnj.procurationCount) + " pour la GÔCHE !";
            fxText.color = rightColor;
        }
        else
        {
            enemyVoteDelegate?.Invoke();
            enemyVotes += pnj.procurationCount + 1;
            fxText.text = "-" + (pnj.procurationCount + 1) + " : Vote à DROUATE !";
            fxText.color = wrongColor;
        }
        StartCoroutine(FXCoroutine(fx));
    }

    IEnumerator FXCoroutine(Transform fx)
    {
        float duration = 3;
        float speed = 0.5f;
        float movementSpeed = 1;
        TMPro.TextMeshProUGUI fxText = fx.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        Color color = fxText.color;
        Vector3 targetScale = fx.transform.localScale;
        float angle = Random.Range(0, 180);
        Vector3 velocity = Vector3.right * Mathf.Cos(angle) + Vector3.up * Mathf.Sin(angle);
        for(float time=0; time < duration; time+=Time.deltaTime)
        {
            float ratio = time / duration;
            float alphaRatio = Mathf.Clamp01((time - duration / 2) / (duration / 2));
            fx.transform.position += (Vector3.up * speed + velocity * movementSpeed) * Time.deltaTime;
            fx.transform.localScale = targetScale * (1 - (1-ratio) * (1-ratio) * (1-ratio));
            color.a = 1 - alphaRatio * alphaRatio;
            fxText.color = color;
            yield return null;
        }
    }
}
