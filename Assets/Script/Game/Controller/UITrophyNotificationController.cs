using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 通知控制器
/// </summary>
public class UITrophyNotificationController : MonoBehaviour
{
    private void Awake()
    {
    }

    private readonly Queue<Trophy> _trophyQueue = new Queue<Trophy>(30);

    private bool _isPlaying;

    // private Coroutine AwardTrophy()
    // {
    //     Trophy trophy = this._trophyQueue.Dequeue();
    //     this._trophyName.text = trophy.TrophyName;
    //     this._trophyIcon.spriteName = trophy.SpriteName;
    //     this._widget.alpha = 1f;
    //     this._widget.transform.localPosition = new Vector3(0f, (float)this._widget.height, 0f);
    //     return base.StartCoroutine(AwardTrophyCoroutine());
    // }
    //
    // private IEnumerator AwardTrophyCoroutine()
    // {
    //     this._isPlaying = true;
    //     this._panel.gameObject.SetActive(true);
    //     yield return this._widget.transform.DOLocalMoveY(0f, 0.5f, false).WaitForCompletion();
    //     yield return new WaitForSeconds(5f);
    //     yield return this._widget.DOFade(0f, 0.5f).WaitForCompletion();
    //     this._panel.gameObject.SetActive(false);
    //     this._isPlaying = false;
    //     if (this._trophyQueue.Count != 0)
    //         this.AwardTrophy();
    //     yield break;
    // }

    /// <summary>
    /// 奖杯
    /// </summary>
    private class Trophy
    {
        public Trophy(string trophyName, string spriteName)
        {
            this.TrophyName = trophyName;
            this.SpriteName = spriteName;
        }

        public string TrophyName { get; set; }

        public string SpriteName { get; set; }
    }
}