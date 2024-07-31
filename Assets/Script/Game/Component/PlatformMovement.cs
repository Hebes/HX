using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 平台运动
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
[AddComponentMenu("PlatformMovement 2D/CharacterMotor")]
public class PlatformMovement : MonoBehaviour
{
    public LayerMask m_rayLayer
    {
        get
        {
            string currentState = R.Player.StateMachine.currentState;
            if (currentState.IsInArray(PlayerAction.FlashAttackSta) && R.Player.Attribute.flashLevel == 3)
            {
                return LayerManager.GroundMask | LayerManager.WallMask;
            }

            return LayerManager.GroundMask | LayerManager.WallMask | LayerManager.ObstacleMask;
        }
    }

    public PlatformMovement.PlatformControllerState State { get; private set; }

    public Vector2 ComputeVelocityOnGround()
    {
        return this.ProjectOnGround(this.velocity);
    }

    public Vector2 GetGroundNormal()
    {
        return this.m_groundNormal;
    }

    public Vector2 GetGroundNormalLs()
    {
        return this.m_groundNormalLs;
    }

    public float GetDistanceToGround()
    {
        return this.m_distToGround;
    }

    public GameObject GetGroundGameObject()
    {
        return this.m_groundGameObject;
    }

    public Vector2 ScaleOffset
    {
        get { return this.m_scaleOffset; }
        set { this.m_scaleOffset = value; }
    }

    public Vector2 Scale
    {
        get { return this.m_scale; }
        set { this.m_scale = value; }
    }

    public Vector2 GetRayPositionWs(PlatformMovement.CharacterRay2D ray)
    {
        return base.transform.TransformPoint(Vector2.Scale(ray.m_position - this.m_scaleOffset, this.m_scale) + this.m_scaleOffset);
    }

    private void Awake()
    {
        this.State = new PlatformMovement.PlatformControllerState();
        this.m_boxCollider = base.GetComponent<BoxCollider2D>();
        this.rigid = base.GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        this.ClearRuntimeValues();
    }

    public void ClearRuntimeValues()
    {
        this.velocity = Vector2.zero;
        this.m_curPos = Vector2.zero;
        this.m_groundNormal = Vector2.up;
        this.m_groundNormalLs = Vector2.up;
        this.m_distToGround = 0f;
        this.m_scaleOffset = Vector2.zero;
        this.m_scale = Vector2.one;
        this.m_groundGameObject = null;
        this.m_ignoreColliders.Clear();
        this.position = this.Position2D;
        this.lastFramePosition = this.position;
        this.isKinematic = false;
    }

    public void AddIgnoreCollider(Collider2D collider, float duration)
    {
        PlatformMovement.IgnoreCollider ignoreCollider = new PlatformMovement.IgnoreCollider();
        ignoreCollider.m_collider = collider;
        ignoreCollider.m_duration = duration;
        ignoreCollider.m_curTime = 0f;
        this.m_ignoreColliders.Add(ignoreCollider);
    }

    public bool IgnoreOnOneWayGround()
    {
        bool result = false;
        foreach (PlatformMovement.CharacterRay2D characterRay2D in this.m_RaysGround)
        {
            if (characterRay2D.m_hitInfo.m_collider != null && (double)characterRay2D.m_hitInfo.m_penetration < 0.1 &&
                this.IsOneWayGround(characterRay2D.m_hitInfo.m_collider))
            {
                result = true;
                this.AddIgnoreCollider(characterRay2D.m_hitInfo.m_collider, 0.5f);
            }
        }

        return result;
    }

    private void RefreshIgnoreColliders()
    {
        for (int i = 0; i < this.m_ignoreColliders.Count; i++)
        {
            this.m_ignoreColliders[i].m_curTime += this.deltaTime;
        }

        for (int j = 0; j < this.m_ignoreColliders.Count; j++)
        {
            if (this.m_ignoreColliders[j].m_duration > 0f && this.m_ignoreColliders[j].m_curTime > this.m_ignoreColliders[j].m_duration)
            {
                this.m_ignoreColliders.RemoveAt(j);
            }
        }
    }

    private bool IsIgnoredCollider(Collider2D collider)
    {
        for (int i = 0; i < this.m_ignoreColliders.Count; i++)
        {
            if (this.m_ignoreColliders[i].m_collider == collider)
            {
                return true;
            }
        }

        return false;
    }

    private Vector2 ProjectOnGround(Vector2 proj)
    {
        Vector2 vector = Vector2.right;
        float f = Vector2.Dot(vector, this.m_groundNormal);
        if (Mathf.Abs(f) > 1.401298E-45f)
        {
            Vector3 rhs = Vector3.Cross(vector, this.m_groundNormal);
            vector = Vector3.Cross(this.m_groundNormal, rhs);
            vector.Normalize();
        }

        return vector * Vector2.Dot(proj, vector);
    }

    private LayerMask BuildRayLayer()
    {
        LayerMask rayLayer = this.m_rayLayer;
        if (this.m_oneWayLayer != 0)
        {
            rayLayer.value |= this.m_oneWayLayer;
        }

        if (this.m_Ceiling != 0)
        {
            rayLayer.value |= this.m_Ceiling;
        }

        return rayLayer;
    }

    private Vector2 Position2D
    {
        get { return this.rigid.position; }
        set
        {
            Vector3 vector = value;
            vector.z = base.transform.position.z;
            base.transform.position = vector;
        }
    }

    private float deltaTime
    {
        get { return Time.fixedDeltaTime; }
    }

    private void FixedUpdate()
    {
        this.RefreshIgnoreColliders();
        this.Move();
        this.position = this.Position2D;
        this.lastFramePosition = this.position;
    }

    private void Move()
    {
        Vector2 positionOffset = this.position - this.lastFramePosition;
        Vector2 moveOffset;
        if (this.isKinematic)
        {
            moveOffset = Vector2.zero;
        }
        else
        {
            if (this.isGravityActive)
            {
                this.UpdateMoveGround();
            }

            moveOffset = this.velocity * this.deltaTime;
        }

        if (this.detecteCollided)
        {
            this.UpdateMovePosition(positionOffset);
            this.UpdateMoveVelocity(moveOffset);
        }

        this.UpdateState();
        this.UpdateMoveGroundFriction();
        if (this.m_DrawRays)
        {
            Vector3 b = this.rigid.velocity;
            UnityEngine.Debug.DrawLine(base.transform.position, base.transform.position + b, Color.green);
        }
    }

    private void UpdateMovePosition(Vector2 positionOffset)
    {
        this.m_curPos = this.Position2D;
        Vector2 zero = Vector2.zero;
        this.ClearRaysCollisionInfo(this.m_RaysGround);
        this.ClearRaysCollisionInfo(this.m_RaysHead);
        this.ClearRaysCollisionInfo(this.m_RaysFront);
        this.ClearRaysCollisionInfo(this.m_RaysBack);
        bool flag = this.MoveVertical(ref positionOffset, false, true, ref zero);
        bool flag2 = this.MoveHorizontal(ref positionOffset, true, true, ref zero);
        this.MoveHorizontal(ref positionOffset, false, !flag2, ref zero);
        this.MoveVertical(ref positionOffset, true, !flag, ref zero);
        this.Position2D = this.m_curPos;
    }

    private void UpdateMoveVelocity(Vector2 moveOffset)
    {
        this.m_curPos = this.Position2D;
        Vector2 position2D = this.Position2D;
        Vector2 zero = Vector2.zero;
        this.ClearRaysCollisionInfo(this.m_RaysGround);
        this.ClearRaysCollisionInfo(this.m_RaysHead);
        this.ClearRaysCollisionInfo(this.m_RaysFront);
        this.ClearRaysCollisionInfo(this.m_RaysBack);
        bool flag = this.MoveVertical(ref moveOffset, false, true, ref zero);
        bool flag2 = this.MoveHorizontal(ref moveOffset, true, true, ref zero);
        this.MoveHorizontal(ref moveOffset, false, !flag2, ref zero);
        this.MoveVertical(ref moveOffset, true, !flag, ref zero);
        if (this.deltaTime > Mathf.Epsilon)
        {
            this.rigid.velocity = (this.m_curPos - position2D) / this.deltaTime;
            this.velocity = (this.m_curPos - position2D - zero) / this.deltaTime;
        }
        else
        {
            this.rigid.velocity = Vector2.zero;
            this.velocity = Vector2.zero;
        }
    }

    private void UpdateMoveGround()
    {
        float num = 100f;
        float num2 = (!this.State.IsDetectedGround) ? float.MaxValue : this.GetDistanceToGround();
        float num3 = 0.01f;
        if (num2 > num3)
        {
            float num4 = (Physics2D.gravity * (this.deltaTime * this.rigid.gravityScale)).y * 1.2f;
            if (this.velocity.y > -num)
            {
                this.velocity.y = Mathf.Max(-num, this.velocity.y + num4);
            }
        }
    }

    private void UpdateMoveGroundFriction()
    {
        PlatformMovement.CharacterRay2D collidedRay = this.GetCollidedRay(this.m_RaysGround);
        if (this.State.IsGrounded && collidedRay != null)
        {
            float num;
            if (collidedRay.m_hitInfo.m_material == null)
            {
                num = 0.8f;
            }
            else
            {
                num = collidedRay.m_hitInfo.m_material.m_dynFriction;
            }

            float b = Mathf.Abs(this.velocity.x) - num * this.deltaTime * 10f;
            this.velocity.x = Mathf.Max(0f, b) * Mathf.Sign(this.velocity.x);
        }
    }

    private void UpdateState()
    {
        bool front = this.GetCollidedRay(this.m_RaysFront) != null;
        bool back = this.GetCollidedRay(this.m_RaysBack) != null;
        bool head = this.GetCollidedRay(this.m_RaysHead) != null;
        bool ground = this.GetCollidedRay(this.m_RaysGround) != null;
        this.GetCollidedGroundRay();
        this.State.Update(front, back, head, ground);
    }

    private PlatformMovement.CharacterRay2D GetCollidedRay(PlatformMovement.CharacterRay2D[] rays)
    {
        foreach (PlatformMovement.CharacterRay2D characterRay2D in rays)
        {
            if (characterRay2D.m_hitInfo.m_collider != null && characterRay2D.m_hitInfo.m_penetration <= characterRay2D.m_extraDistance)
            {
                return characterRay2D;
            }
        }

        return null;
    }

    private PlatformMovement.CharacterRay2D GetCollidedGroundRay()
    {
        PlatformMovement.CharacterRay2D[] raysGround = this.m_RaysGround;
        foreach (PlatformMovement.CharacterRay2D characterRay2D in raysGround)
        {
            if (characterRay2D.m_hitInfo.m_collider != null &&
                characterRay2D.m_hitInfo.m_penetration <= Mathf.Max(characterRay2D.m_extraDistance, 1f))
            {
                return characterRay2D;
            }
        }

        return null;
    }

    private void ClearRaysCollisionInfo(PlatformMovement.CharacterRay2D[] rays)
    {
        foreach (PlatformMovement.CharacterRay2D characterRay2D in rays)
        {
            characterRay2D.m_hitInfo.m_collider = null;
            characterRay2D.m_hitInfo.m_material = null;
        }
    }

    private bool IsOneWayGround(Collider2D collider)
    {
        int num = 1 << collider.gameObject.layer;
        return (this.m_oneWayLayer.value & num) != 0;
    }

    private bool IsCeiling(Collider2D collider)
    {
        int num = 1 << collider.gameObject.layer;
        return (this.m_Ceiling.value & num) != 0;
    }

    private bool MoveVertical(ref Vector2 moveOffset, bool bDown, bool bCorrectPos, ref Vector2 m_posError)
    {
        bool flag = false;
        float num = 0f;
        Vector2 vector = (!bDown) ? base.transform.up : (-base.transform.up);
        float num2 = Vector2.Dot(moveOffset, vector);
        float num3 = Mathf.Abs(num2);
        bool flag2 = num2 > 0f;
        float num4 = float.MaxValue;
        Vector2 position2D = this.Position2D;
        this.Position2D = this.m_curPos;
        if (bDown)
        {
            this.State.IsDetectedGround = false;
            this.m_groundGameObject = null;
            this.m_distToGround = 0f;
        }

        foreach (PlatformMovement.CharacterRay2D characterRay2D in (!bDown) ? this.m_RaysHead : this.m_RaysGround)
        {
            float num5 = characterRay2D.m_penetration * Mathf.Abs(base.transform.lossyScale.y) * this.m_scale.y;
            float num6 = num5 + Mathf.Max(characterRay2D.m_extraDistance, 1f);
            if (flag2)
            {
                num6 += Mathf.Abs(num2);
            }

            Vector2 rayPositionWs = this.GetRayPositionWs(characterRay2D);
            if (this.m_DrawRays)
            {
                UnityEngine.Debug.DrawLine(rayPositionWs, rayPositionWs + vector * num6);
            }

            int num7 = Physics2D.RaycastNonAlloc(rayPositionWs, vector, this.m_rayResults, num6, this.BuildRayLayer());
            if (num7 > 0)
            {
                float num8 = float.MaxValue;
                for (int j = 0; j < num7; j++)
                {
                    RaycastHit2D raycastHit2D = this.m_rayResults[j];
                    if (!(raycastHit2D.rigidbody == this.rigid))
                    {
                        if (raycastHit2D.collider != null)
                        {
                            if (raycastHit2D.collider.isTrigger || this.IsIgnoredCollider(raycastHit2D.collider))
                            {
                                goto IL_3D8;
                            }

                            if ((!bDown || (bDown && raycastHit2D.normal.y < 0f)) && this.IsOneWayGround(raycastHit2D.collider))
                            {
                                goto IL_3D8;
                            }

                            if ((bDown || (!bDown && raycastHit2D.normal.y > 0f)) && this.IsCeiling(raycastHit2D.collider))
                            {
                                goto IL_3D8;
                            }
                        }

                        if (raycastHit2D.fraction != 0f)
                        {
                            Vector2 vector2 = base.transform.InverseTransformDirection(raycastHit2D.normal);
                            if ((!bDown || vector2.y > 0f) && (bDown || vector2.y < 0f))
                            {
                                if (this.m_DrawRays)
                                {
                                    UnityEngine.Debug.DrawLine(raycastHit2D.point, raycastHit2D.point + raycastHit2D.normal * 0.1f, Color.red);
                                }

                                float num9 = raycastHit2D.fraction * num6;
                                float num10 = num9 - num5;
                                if (num10 + this.m_allowedPenetration < num)
                                {
                                    flag = true;
                                    num = num10;
                                }

                                if (flag2)
                                {
                                    num3 = Mathf.Max(0f, Mathf.Min(num3, num9 - num5));
                                }

                                if (bDown)
                                {
                                    if (num10 < num4)
                                    {
                                        this.m_groundNormal = raycastHit2D.normal;
                                        num4 = num10;
                                        this.m_groundGameObject = raycastHit2D.collider.gameObject;
                                        this.m_distToGround = num10;
                                    }
                                }

                                if (raycastHit2D.fraction < num8)
                                {
                                    num8 = raycastHit2D.fraction;
                                    characterRay2D.m_hitInfo.m_collider = raycastHit2D.collider;
                                    characterRay2D.m_hitInfo.m_normal = raycastHit2D.normal;
                                    characterRay2D.m_hitInfo.m_penetration = num10;
                                }
                            }
                        }
                    }

                    IL_3D8: ;
                }
            }
        }

        Vector2 curPos = this.m_curPos;
        if (flag2)
        {
            moveOffset += vector * (num3 - num2);
            this.m_curPos += vector * num3;
        }

        if (flag && bCorrectPos)
        {
            Vector2 b = Mathf.Max(num, -this.m_PosErrorMaxVel * this.deltaTime) * vector;
            this.m_curPos += b;
            m_posError += b;
        }

        if (bDown && this.State.IsDetectedGround)
        {
            this.m_groundNormalLs = base.transform.InverseTransformDirection(this.m_groundNormal);
            this.m_distToGround += Vector2.Dot(curPos - this.m_curPos, vector);
        }

        if (bDown)
        {
            for (int k = 0; k < this.m_RaysGround.Length; k++)
            {
                PlatformMovement.CharacterRay2D characterRay2D2 = this.m_RaysGround[k];
                if (characterRay2D2.m_hitInfo.m_collider)
                {
                    float num11 = Vector2.Dot(curPos - this.m_curPos, vector);
                    PlatformMovement.CharacterRay2D characterRay2D3 = characterRay2D2;
                    characterRay2D3.m_hitInfo.m_penetration = characterRay2D3.m_hitInfo.m_penetration + num11;
                }
            }
        }
        else
        {
            for (int l = 0; l < this.m_RaysHead.Length; l++)
            {
                PlatformMovement.CharacterRay2D characterRay2D4 = this.m_RaysHead[l];
                if (characterRay2D4.m_hitInfo.m_collider)
                {
                    PlatformMovement.CharacterRay2D characterRay2D5 = characterRay2D4;
                    characterRay2D5.m_hitInfo.m_penetration = characterRay2D5.m_hitInfo.m_penetration + Vector2.Dot(curPos - this.m_curPos, vector);
                }
            }
        }

        this.Position2D = position2D;
        return flag;
    }

    private bool MoveHorizontal(ref Vector2 moveOffset, bool bFront, bool bCorrectPos, ref Vector2 m_posError)
    {
        bool flag = false;
        float num = 0f;
        bool flag2 = (this.m_faceRight && bFront) || (!this.m_faceRight && !bFront);
        Vector2 vector = (!flag2) ? (-base.transform.right) : base.transform.right;
        float num2 = Vector2.Dot(moveOffset, vector);
        bool flag3 = num2 > 0f;
        float num3 = Mathf.Abs(num2);
        Vector2 position2D = this.Position2D;
        this.Position2D = this.m_curPos;
        foreach (PlatformMovement.CharacterRay2D characterRay2D in (!bFront) ? this.m_RaysBack : this.m_RaysFront)
        {
            float num4 = characterRay2D.m_penetration * Mathf.Abs(base.transform.lossyScale.x) * this.m_scale.x;
            float num5 = num4 + characterRay2D.m_extraDistance;
            if (flag3)
            {
                num5 += Mathf.Abs(num2);
            }

            Vector2 rayPositionWs = this.GetRayPositionWs(characterRay2D);
            if (this.m_DrawRays)
            {
                UnityEngine.Debug.DrawLine(rayPositionWs, rayPositionWs + vector * num5);
            }

            int num6 = Physics2D.RaycastNonAlloc(rayPositionWs, vector, this.m_rayResults, num5, this.BuildRayLayer());
            if (num6 > 0)
            {
                float num7 = float.MaxValue;
                for (int j = 0; j < num6; j++)
                {
                    RaycastHit2D raycastHit2D = this.m_rayResults[j];
                    if (!(raycastHit2D.rigidbody == this.rigid))
                    {
                        if (!(raycastHit2D.collider != null) || (!raycastHit2D.collider.isTrigger && !this.IsIgnoredCollider(raycastHit2D.collider)))
                        {
                            float num8 = raycastHit2D.fraction * num5;
                            float num9 = num8 - num4;
                            APMaterial material = null;
                            if (raycastHit2D.collider != null)
                            {
                                material = raycastHit2D.collider.GetComponent<APMaterial>();
                                if (this.IsOneWayGround(raycastHit2D.collider))
                                {
                                    goto IL_384;
                                }
                            }

                            if (raycastHit2D.fraction != 0f)
                            {
                                Vector2 vector2 = base.transform.InverseTransformDirection(raycastHit2D.normal);
                                if ((!flag2 || vector2.x <= 0f) && (flag2 || vector2.x >= 0f))
                                {
                                    if (this.m_DrawRays)
                                    {
                                        UnityEngine.Debug.DrawLine(raycastHit2D.point, raycastHit2D.point + raycastHit2D.normal * 0.1f, Color.red);
                                    }

                                    if (bFront)
                                    {
                                        this.State.IsDetectedFront = true;
                                    }
                                    else
                                    {
                                        this.State.IsDetectedBack = true;
                                    }

                                    if (num9 + this.m_allowedPenetration < num)
                                    {
                                        flag = true;
                                        num = num9;
                                    }

                                    if (raycastHit2D.fraction < num7)
                                    {
                                        num7 = raycastHit2D.fraction;
                                        characterRay2D.m_hitInfo.m_collider = raycastHit2D.collider;
                                        characterRay2D.m_hitInfo.m_normal = raycastHit2D.normal;
                                        characterRay2D.m_hitInfo.m_penetration = num9;
                                        characterRay2D.m_hitInfo.m_material = material;
                                    }

                                    if (flag3)
                                    {
                                        num3 = Mathf.Min(num3, num8 - num4);
                                        num3 = Mathf.Max(0f, num3);
                                    }
                                }
                            }
                        }
                    }

                    IL_384: ;
                }
            }
        }

        Vector2 curPos = this.m_curPos;
        if (flag3)
        {
            moveOffset += vector * (num3 - num2);
            this.m_curPos += vector * num3;
        }

        if (flag && bCorrectPos)
        {
            Vector2 b = Mathf.Max(num, -this.m_PosErrorMaxVel * this.deltaTime) * vector;
            this.m_curPos += b;
            m_posError += b;
        }

        if (bFront)
        {
            for (int k = 0; k < this.m_RaysFront.Length; k++)
            {
                PlatformMovement.CharacterRay2D characterRay2D2 = this.m_RaysFront[k];
                if (characterRay2D2.m_hitInfo.m_collider)
                {
                    PlatformMovement.CharacterRay2D characterRay2D3 = characterRay2D2;
                    characterRay2D3.m_hitInfo.m_penetration = characterRay2D3.m_hitInfo.m_penetration + Vector2.Dot(curPos - this.m_curPos, vector);
                }
            }
        }
        else
        {
            for (int l = 0; l < this.m_RaysBack.Length; l++)
            {
                PlatformMovement.CharacterRay2D characterRay2D4 = this.m_RaysBack[l];
                if (characterRay2D4.m_hitInfo.m_collider)
                {
                    PlatformMovement.CharacterRay2D characterRay2D5 = characterRay2D4;
                    characterRay2D5.m_hitInfo.m_penetration = characterRay2D5.m_hitInfo.m_penetration + Vector2.Dot(curPos - this.m_curPos, vector);
                }
            }
        }

        this.Position2D = position2D;
        return flag;
    }

    public PlatformMovement.CharacterRay2D[] m_RaysGround;

    public PlatformMovement.CharacterRay2D[] m_RaysHead;

    public PlatformMovement.CharacterRay2D[] m_RaysFront;

    public PlatformMovement.CharacterRay2D[] m_RaysBack;

    public Vector2 velocity = Vector2.zero;

    public bool m_faceRight = true;

    public LayerMask m_oneWayLayer = 0;

    public LayerMask m_Ceiling = 0;

    public bool m_DrawRays;

    public PlatformMovement.AutoBuilder m_autoBuilder = new PlatformMovement.AutoBuilder();

    public Vector2 position;

    public bool isKinematic;

    public bool isGravityActive = true;

    public bool detecteCollided = true;

    private RaycastHit2D[] m_rayResults = new RaycastHit2D[8];

    private float m_PosErrorMaxVel = 100f;

    private float m_allowedPenetration = 0.01f;

    private BoxCollider2D m_boxCollider;

    private Rigidbody2D rigid;

    private Vector2 m_curPos;

    private Vector2 lastFramePosition;

    private Vector2 m_scaleOffset = Vector2.zero;

    private Vector2 m_scale = Vector2.one;

    private Vector2 m_groundNormal;

    private Vector2 m_groundNormalLs;

    private float m_distToGround;

    private GameObject m_groundGameObject;

    private List<PlatformMovement.IgnoreCollider> m_ignoreColliders = new List<PlatformMovement.IgnoreCollider>(8);

    [Serializable]
    public class CharacterRay2D
    {
        public Vector2 m_position = Vector2.zero;

        public float m_penetration;

        public float m_extraDistance = 0.1f;

        public PlatformMovement.RayHitInfo m_hitInfo;
    }

    [Serializable]
    public struct RayHitInfo
    {
        public Collider2D m_collider;

        public APMaterial m_material;

        public Vector2 m_normal;

        public float m_penetration;
    }

    [Serializable]
    public class AutoBuilder
    {
        public int m_rayCountX = 2;

        public int m_rayCountY = 3;

        public float m_extraDistanceFront = 0.1f;

        public float m_extraDistanceBack = 0.1f;

        public float m_extraDistanceUp = 0.1f;

        public float m_extraDistanceDown = 0.1f;

        public Vector2 m_rayXBoxScale = new Vector2(0.9f, 0.6f);

        public Vector2 m_rayYBoxScale = new Vector2(0f, 0.8f);
    }

    private class IgnoreCollider
    {
        public Collider2D m_collider;

        public float m_duration;

        public float m_curTime;
    }

    public class PlatformControllerState
    {
        public bool HasCollisions
        {
            get { return this.IsDetectedFront || this.IsDetectedBack || this.IsDetectedHead || this.IsDetectedGround; }
        }

        public bool JustGotGrounded
        {
            get { return this.IsGrounded && !this.WasGroundedLastFrame; }
        }

        public void Update(bool front, bool back, bool head, bool ground)
        {
            this.WasGroundedLastFrame = this.IsGrounded;
            this.IsDetectedFront = front;
            this.IsDetectedBack = back;
            this.IsDetectedHead = head;
            this.IsDetectedGround = ground;
            this.IsGrounded = ground;
        }

        public override string ToString()
        {
            return string.Format("(controller: r:{0} l:{1} a:{2} b:{3} down-slope:{4} up-slope:{5} angle: {6}", new object[]
            {
                this.IsDetectedFront,
                this.IsDetectedBack,
                this.IsDetectedHead,
                this.IsDetectedGround,
                "无",
                "无",
                "无"
            });
        }

        public bool IsDetectedFront;

        public bool IsDetectedBack;

        public bool IsDetectedHead;

        public bool IsDetectedGround;

        public bool IsGrounded;

        public bool WasGroundedLastFrame;
    }
}