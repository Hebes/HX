using System.Collections;
using Framework.Core;
using LitJson;
using UnityEngine;

public class BeelzebubAnimListener : MonoBehaviour
{
    private Transform player => R.Player.Transform;

    private void Start()
    {
        _eAction = GetComponent<BeelzebubAction>();
        _eAttr = GetComponent<EnemyAttribute>();
        _atk = GetComponentInChildren<EnemyAtk>();
        _atkData = EnemyDataPreload.Instance.attack[EnemyType.暴食];
        _atk2HitLoopTime = 1;
        Atk2MoveLoopTime = 2;
    }

    public void ChangeState(BeelzebubAction.StateEnum sta)
    {
        _eAction.AnimChangeState(sta);
    }

    public void EatAtkEnd()
    {
        BeelzebubAction.StateEnum stateEnum = (!_eAction.EatSuccess) ? BeelzebubAction.StateEnum.Atk1Fail : BeelzebubAction.StateEnum.Atk1Success;
        _eAction.AnimChangeState(stateEnum);
    }

    public void EatAtkSuccess()
    {
        _eAction.EatSuccess = false;
        player.GetComponent<Collider2D>().enabled = false;
        player.localRotation = Quaternion.identity;
        PlayerHurtAtkEventArgs args = new PlayerHurtAtkEventArgs(player.gameObject, gameObject, gameObject, _eAttr.atk, Incrementor.GetNextId(),
            _atkData["Atk1Release"], true);
        EGameEvent.PlayerHurtAtk.Trigger((transform, args));
    }

    public void SawAttackLoopEnd()
    {
        _atk2HitLoopTime--;
        if (_atk2HitLoopTime >= 0)
        {
            return;
        }

        _atk2HitLoopTime = 1;
        SawAttackFinish();
    }

    public void SawAttackMoveLoopEnd()
    {
        Atk2MoveLoopTime--;
        if (Atk2MoveLoopTime < 0)
        {
            Atk2MoveLoopTime = 2;
            ChangeState(BeelzebubAction.StateEnum.Atk2End);
        }
        else
        {
            GetComponent<AnimMoveController>().AnimMoveLoopReset();
        }
    }

    public void SawAttackFinish()
    {
        _eAction.SawSuccess = false;
        player.GetComponent<Collider2D>().enabled = false;
        player.localRotation = Quaternion.identity;
        PlayerHurtAtkEventArgs args = new PlayerHurtAtkEventArgs(player.gameObject, gameObject, gameObject, _eAttr.atk, Incrementor.GetNextId(),
            _atkData["Atk2Release"], true);
        EGameEvent.PlayerHurtAtk.Trigger((transform, args));
        _eAction.AnimChangeState(BeelzebubAction.StateEnum.Atk2End);
    }

    public void StartPabody()
    {
        _eAttr.paBody = true;
    }

    public void EndPabody()
    {
        _eAttr.paBody = false;
    }

    public void SetAtkData()
    {
        _atk.atkData = _atkData[_eAction.stateMachine.currentState];
    }

    public void SetAtkId()
    {
        _atk.atkId = Incrementor.GetNextId();
    }

    public void PlaySound(int index)
    {
        R.Audio.PlayEffect(index, transform.position);
    }

    public void PlayMoveSound()
    {
        R.Audio.PlayEffect(moveSound[Random.Range(0, moveSound.Length)], transform.position);
    }

    public void PlayEatAudio()
    {
        if (_eAction.EatSuccess)
        {
            R.Audio.PlayEffect(84, transform.position);
        }
    }

    public void Atk3Effect()
    {
        if (!R.Camera.IsInView(this.gameObject))
        {
            return;
        }

        GameObject prefab = CameraEffectProxyPrefabData.GetPrefab(6);
        GameObject gameObject = Instantiate(prefab, transform.position, Quaternion.identity);
        gameObject.transform.parent = transform;
    }

    public void AngeyEffect()
    {
        if (!R.Camera.IsInView(gameObject))
        {
            return;
        }

        R.Camera.Controller.OpenMotionBlur(1.16666663f, 0.1f, transform.position + Vector3.up * 2f);
        R.Camera.Controller.CameraShake(1.16666663f, 0.1f);
        ShoutEffect();
    }

    private void ShoutEffect()
    {
        R.Effect.Generate(17, null, transform.Find("Center").position);
    }

    public void PlayAirDistort()
    {
        R.Effect.Generate(49, transform, new Vector3(0f, 1.2f, LayerManager.ZNum.Fx), Vector3.zero);
    }

    public void DestroySelf()
    {
        Invoke("RealDestroy", 2f);
        gameObject.SetActive(false);
    }

    private void RealDestroy()
    {
        Destroy(gameObject);
    }

    public void DieBlock()
    {
        R.Effect.Generate(163, null, new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.1f), Vector3.zero);
    }

    public void BackToIdle()
    {
        if (_eAction.IsInWeakSta())
        {
            _eAttr.enterWeakMod = false;
            _eAction.AnimChangeState(BeelzebubAction.StateEnum.WeakMod);
        }
        else
        {
            _eAction.AnimChangeState(BeelzebubAction.StateEnum.Idle);
        }
    }

    public IEnumerator WeakOver()
    {
        for (int i = 0; i < 96; i++)
        {
            if (_eAction.stateMachine.currentState == "WeakMod" && !_eAction.IsInWeakSta())
            {
                _eAction.AnimChangeState(BeelzebubAction.StateEnum.Idle);
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public void PlayATK3Effect()
    {
        R.Effect.Generate(133, transform);
        R.Effect.Generate(145, transform);
    }

    public void PlayATK1Effect()
    {
        R.Effect.Generate(134, transform, new Vector3(-0.4f, 1.65f, 0f));
    }

    public void Atk2Effect()
    {
        LSawUpper2.gameObject.SetActive(true);
    }

    public void StopAtk2Effect()
    {
        LSawUpper2.gameObject.SetActive(false);
    }

    public void PlayATK12Effect()
    {
        R.Effect.Generate(139, transform, new Vector3(-1f, 2f, 0f));
    }

    public void PlayATK2Effect()
    {
        BeelzebubATK2.gameObject.SetActive(true);
        BeelzebubATK2.rotation = Quaternion.Euler(new Vector3(0f, (transform.localScale.x <= 0f) ? 180 : 0, 0f));
    }

    public void QTECameraZoomIn()
    {
        SingletonMono<CameraController>.Instance.CameraZoom(transform.position + Vector3.up * 3f, 0.166666672f);
        SingletonMono<CameraController>.Instance.CameraShake(0.2f);
        SingletonMono<CameraController>.Instance.OpenMotionBlur(0.06666667f, 0.1f, transform.position + Vector3.up * 3f);
    }

    public void QTEPlayerAppear()
    {
        player.position = transform.position + Vector3.up * 8f;
        R.Player.Action.ChangeState(PlayerAction.StateEnum.DahalRoll);
    }

    public void QTEShadeAtk()
    {
        QTENormalHurt();
        SingletonMono<CameraController>.Instance.CameraShake(0.166666672f, 0.3f);
    }

    public void QTENormalHurt()
    {
        EnemyBaseHurt component = GetComponent<EnemyBaseHurt>();
        component.HitEffect(component.center.localPosition);
    }

    public void QTEFinalHurt()
    {
        EnemyHurtAtkEventArgs args = new EnemyHurtAtkEventArgs(gameObject, EnemyHurtAtkEventArgs.HurtTypeEnum.QTEHurt);
        EGameEvent.EnemyHurtAtk.Trigger((gameObject, args));
    }

    public void ExecutePlayerPush()
    {
        R.Player.Action.ChangeState(PlayerAction.StateEnum.QTEPush);
    }

    public void ExecuteFinalHurt()
    {
        EnemyHurtAtkEventArgs args = new EnemyHurtAtkEventArgs(gameObject, EnemyHurtAtkEventArgs.HurtTypeEnum.Execute, string.Empty);
        EGameEvent.EnemyHurtAtk.Trigger((player.gameObject, args));
    }

    private BeelzebubAction _eAction;

    private EnemyAttribute _eAttr;

    private EnemyAtk _atk;

    private JsonData1 _atkData;

    private int _atk2HitLoopTime;

    public int Atk2MoveLoopTime;

    [SerializeField] public Transform LSawUpper2;

    [SerializeField] public Transform BeelzebubATK2;

    [SerializeField] private int[] moveSound;
}