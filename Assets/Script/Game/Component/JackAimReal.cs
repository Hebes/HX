using UnityEngine;

public class JackAimReal : MonoBehaviour
{
    public Transform Jack
    {
        private get => _jack;
        set
        {
            _jack = value;
            _jackAction = value.GetComponent<JackAction>();
        }
    }

    private void Awake()
    {
        _skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
        _animation = GetComponent<Animation>();
    }

    private void Update()
    {
        if (Jack == null)
        {
            return;
        }
        if (_jackAction.IsInWeakSta())
        {
            _animation.Play("ShowDisappear");
        }
    }

    private void PlaySpine(string name)
    {
        _skeletonAnimation.skeleton.SetToSetupPose();
        _skeletonAnimation.state.SetAnimation(0, name, false);
        _skeletonAnimation.Update(0f);
    }

    public void PlayShoot()
    {
        PlaySpine("ShowShoot");
        _animation.Play("ShowShoot");
    }

    public void PlayShootEnd()
    {
        PlaySpine("ShowShootEnd");
        _animation.Play("ShowShootEnd");
    }

    public void PlayDisappear()
    {
        PlaySpine("ShowDisappear");
        _animation.Play("ShowDisappear");
    }

    public void PlayDestroy()
    {
        Destroy(gameObject);
    }

    public void PlayShootEffect()
    {
        Transform transform = R.Effect.Generate(202, null, this.transform.position);
        R.Audio.PlayEffect(321, transform.position);
        EnemyBullet component = transform.GetComponent<EnemyBullet>();
        component.SetAtkData(Jack.GetComponent<JackAnimEvent>().jsonData["Atk5Ready"]);
        component.origin = Jack.gameObject;
    }

    private Transform _jack;

    private SkeletonAnimation _skeletonAnimation;

    private Animation _animation;

    private JackAction _jackAction;
}