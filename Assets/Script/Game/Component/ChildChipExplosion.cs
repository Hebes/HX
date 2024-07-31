using UnityEngine;

/// <summary>
/// 
/// </summary>
public class ChildChipExplosion : MonoBehaviour
{
    private float deltaTime
    {
        get
        {
            return Time.time - this.startTime;
        }
    }

    private void OnEnable()
    {
        this.startTime = Time.time;
        this.backToPlayer = false;
        this.player = R.Player.Transform;
        base.transform.GetChild(0).gameObject.SetActive(true);
    }

    private void Update()
    {
        if (this.backToPlayer)
        {
            base.transform.position = Vector3.MoveTowards(base.transform.position, this.player.position + new Vector3(0f, 1.2f, 0f), 0.5f * this.playerChargingSpeed * this.deltaTime);
            if (Vector3.Distance(base.transform.position, this.player.position + new Vector3(0f, 1.2f, 0f)) < 0.1f)
            {
                R.Player.Action.AbsorbEnergyBall();
                this.backToPlayer = false;
                base.StartCoroutine(this.player.GetComponent<ChangeSpineColor>().EnergyBallColorChange());
                base.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    public bool backToPlayer;

    public Transform player;

    private float startTime;

    private float playerChargingSpeed = 20f;
}