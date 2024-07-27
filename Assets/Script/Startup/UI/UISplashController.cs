﻿using System;
using System.Collections;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UISplashController : MonoBehaviour
{
    public static UISplashController Instance;

    private void Awake()
    {
        Instance = this;
    }

    [SerializeField] private CanvasGroup[] _canvasGroupArray;

    [SerializeField] private float _fade = 1f;

    [SerializeField] private float _duration = 1.5f;

    public void Run()
    {
        GameObject.Destroy(gameObject);
        return;
        StartCoroutine(Sequence0());
    }

    private IEnumerator Sequence0()
    {
        //if (UILanguage.IsSimplifiedChinese)
        //{
        yield return DOTween.To(delegate(float a) { SetColor(0, a); }, 0f, 1f, this._fade).WaitForCompletion();

        yield return new WaitForSeconds(5f);

        yield return DOTween.To(delegate(float a) { SetColor(0, a); }, 1f, 0f, this._fade).WaitForCompletion();
        yield return new WaitForSeconds(this._fade);
        //}
        for (var i = 1; i < this._canvasGroupArray.Length; i++)
        {
            var iCopy = i;
            yield return DOTween.To(delegate(float a) { SetColor(iCopy, a); }, 0f, 1f, this._fade).WaitForCompletion();
            yield return new WaitForSeconds((i == 0) ? 5f : this._duration);
            yield return DOTween.To(delegate(float a) { SetColor(iCopy, a); }, 1f, 0f, this._fade).WaitForCompletion();
            if (iCopy != this._canvasGroupArray.Length - 1)
            {
                yield return new WaitForSeconds(this._fade);
            }
        }

        //this._asyncOperation.allowSceneActivation = true;
        GameObject.Destroy(gameObject);
        yield break;

        void SetColor(int i, float a)
        {
            _canvasGroupArray[i].alpha = a;
        }
    }
}