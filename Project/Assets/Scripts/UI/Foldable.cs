using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foldable : MonoBehaviour
{
    private RectTransform rectTransform;
    public float animDuration = 0.5f;
    private float animTime = 0;
    [SerializeField]
    private bool _isOn = false;

    public bool isOn {
        get { return _isOn; } 
        set { 
            _isOn = value;
            foldStateChangedDelegate?.Invoke(value);
            if(value)
                unfoldDelegate?.Invoke();
            else
                foldDelegate?.Invoke();
        }
    }
    public float animRatio { get => animTime / animDuration; }

    public System.Action<bool> foldStateChangedDelegate;
    public System.Action foldDelegate;
    public System.Action unfoldDelegate;


    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetAnimDuration(float newDuration)
    {
        float ratio = animRatio;
        animDuration = newDuration;
        animTime = animDuration * ratio;
    }

    public IEnumerator CloseCoroutine()
    {
        isOn = false;
        while(!isOn && animTime > 0)
        {
            yield return null;
        }
    }

    void Update()
    {
        if(isOn)
        {
            animTime += Time.deltaTime;
            if(animTime > animDuration)
                animTime = animDuration;
        }
        else
        {
            animTime -= Time.deltaTime;
            if(animTime < 0)
                animTime = 0;
        }
    }
}
