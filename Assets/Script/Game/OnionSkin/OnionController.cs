using UnityEngine;

/// <summary>
/// 控制器
/// </summary>
public class OnionController : MonoBehaviour
{
    private void OnDisable()
    {
        this._disappearTime = 1f;
        this._disappearTimer = 0f;
        this._meshFilter = null;
        this.tint = this._startColor;
    }

    private void OnEnable()
    {
        if (this._meshFilter != null)
        {
            this._meshFilter.sharedMesh = null;
        }

        if (this._meshFilter == null)
        {
            this._meshFilter = base.GetComponent<MeshFilter>();
        }

        this._startColor = this.tint;
    }

    private void Update()
    {
        if (this._materials == null)
        {
            return;
        }

        if (this._curve == null)
        {
            this._curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
            this._disappearTime = 1f;
        }

        if (this._scaleCurve == null)
        {
            this._scaleCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);
        }

        this._disappearTimer += Time.deltaTime;
        float num = Mathf.Clamp01(this._curve.Evaluate(this._disappearTimer / this._disappearTime));
        this.tint.a = this.tint.a * num;
        float d = Mathf.Clamp01(this._scaleCurve.Evaluate(this._disappearTimer / this._disappearTime));
        Vector3 localScale = this._startScale * d;
        base.transform.localScale = localScale;
        this.Refresh();
        if (this._disappearTimer > this._disappearTime)
        {
            EffectController.TerminateEffect(base.gameObject);
        }
    }

    public void Clone(GameObject game)
    {
        this._startScale = base.transform.localScale;
        Mesh mesh = game.GetComponent<MeshFilter>().mesh;
        this._materials = game.GetComponent<Renderer>().materials;
        base.GetComponent<Renderer>().materials = this._materials;
        if (this._meshFilter == null)
        {
            this._meshFilter = base.GetComponent<MeshFilter>();
        }

        this._meshFilter.mesh = UnityEngine.Object.Instantiate<Mesh>(mesh);
        for (int i = 0; i < this._materials.Length; i++)
        {
            this._materials[i].shader = this.shader;
            this._materials[i].SetFloat("_EmissionStrength", this.EmissionStrength);
            this._materials[i].SetColor("_Color", this.tint);
        }
    }

    public void Refresh()
    {
        for (int i = 0; i < this._materials.Length; i++)
        {
            this._materials[i].SetColor("_Color", this.tint);
        }
    }

    public void SetDisappear(AnimationCurve disappearCurve, float disappearTime)
    {
        this._curve = ((this._curve == null) ? AnimationCurve.Linear(0f, 1f, 1f, 0f) : disappearCurve);
        this._disappearTime = disappearTime;
    }

    public Color tint = new Color(0f, 0f, 0.5f, 0.5f);

    public float EmissionStrength;

    private MeshFilter _meshFilter;

    public Shader shader;

    private Material[] _materials;

    private AnimationCurve _curve;

    private AnimationCurve _scaleCurve;

    private Vector3 _startScale;

    private float _disappearTime;

    private float _disappearTimer;

    private Color _startColor;
}