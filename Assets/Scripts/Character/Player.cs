using Animancer;
using ConfigAuto;
using Cysharp.Threading.Tasks;
using R3;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace PunchPeng
{
    public enum PlayerLocomotionState
    {
        Locomotion,
        HeadAttack,
        PunchAttac,
        Dead,
        Ability,
    }

    public class Player : MonoEntity
    {
        /// <summary>
        /// 玩家ID, 正常玩家ID为 1 或者 2，0表示AI
        /// </summary>
        public int PlayerId;

        [SerializeField] private float m_CfgMaxMoveSpeed = 2.4f;
        [SerializeField] private float m_CfgMaxRunSpeed = 3.5f;
        [SerializeField] private float m_CfgAcceleration = 10f;
        [SerializeField] private float m_CfgRotateDeg = 30f;

        [SerializeField] private CharacterController m_CCT;
        [SerializeField] private AnimancerComponent m_Animancer;
        [SerializeField] public PlayerAnimData m_AnimData;

        [SerializeField] public TriggerHelper m_PunchAttackTrigger;
        [SerializeField] public TriggerHelper m_HeadAttackTrigger;
        [SerializeField] private AudioSource m_AudioSource;

        private List<PlayerAbility> m_Abilities = new();
        private BehaviorTree m_BehaviorTree;
        private CopyPlayerInputAI m_CopyAI;

        public Vector3 InputMoveDir;
        public bool InputRun;
        public bool InputAttack;
        public bool InputUseSkill;

        public readonly ReactiveProperty<PlayerLocomotionState> LocomotionState = new();
        public readonly ReactiveProperty<Vector3> Velocity = new();
        public float VelocityMagnitude { get; private set; }
        [ReadOnly] public ReferenceBool CanMove;
        [ReadOnly] public ReferenceBool CanAttack;
        public bool IsDead => LocomotionState.Value == PlayerLocomotionState.Dead;
        public bool IsAI => m_BehaviorTree != null;

        public override Vector3 Position
        {
            get => base.Position;
            set
            {
                var cctOldEnable = m_CCT.enabled;
                m_CCT.enabled = false;
                base.Position = value;
                m_CCT.enabled = cctOldEnable;
            }
        }

        private void Awake()
        {
            m_AudioSource = GetComponent<AudioSource>();
            Assert.IsNotNull(m_AudioSource);

            //m_Abilities.Add(new PlayerHeadAttackAbility());
            m_Abilities.Add(new PlayerPunchAttackAbility());

            foreach (var ability in m_Abilities)
            {
                ability.Init(this);
            }

            LocomotionState.Subscribe(OnLocomotionChange);
            Velocity.Subscribe(OnVelocityChange);

            m_PunchAttackTrigger.SetActiveEx(false);
            m_HeadAttackTrigger.SetActiveEx(false);
        }

        private void Start()
        {
            CanMove.RefCnt++;
            LocomotionState.Value = PlayerLocomotionState.Locomotion;
            StartAsync().Forget();
        }

        private async UniTask StartAsync()
        {
            await UniTask.Delay(3000); // 前三秒不准攻击
            CanAttack.RefCnt++;
        }

        private void Update()
        {
            if (!IsDead)
            {
                if (m_CopyAI != null)
                {
                    m_CopyAI.OnUpdate();
                }
                else
                {
                    m_BehaviorTree?.OnUpdate(Time.deltaTime);
                }

                foreach (var ability in m_Abilities)
                {
                    ability.Update(Time.deltaTime);
                }

                UpdateMoveCommand(InputMoveDir);
            }
        }

        private void LateUpdate()
        {
            InputAttack = false;
        }

        private void OnDestroy()
        {
        }

        public void OnGameEnd()
        {
            CanMove.RefCnt--;
            Velocity.Value = Vector3.zero;
        }

        public void SetIsAI(bool copyAI)
        {
            if (copyAI)
            {
                m_CopyAI = new CopyPlayerInputAI();
                m_CopyAI.Init(this);
            }
            else
            {
                m_BehaviorTree = new BehaviorTree();
                m_BehaviorTree.Init(this);
            }
        }

        public void PlayAnim(ClipTransition anim)
        {
            m_Animancer.Play(anim, fadeDuration: 0.15f, mode: FadeMode.FromStart);
        }

        private void UpdateMoveCommand(Vector3 inputMove)
        {
            if (!CanMove) return;

            var targetVelocity = inputMove * m_CfgMaxMoveSpeed;
            var targetSpeed = targetVelocity.magnitude;
            var playerCanRun = InputRun && targetSpeed.Approximately(m_CfgMaxMoveSpeed);

            if (playerCanRun)
            {
                targetSpeed = Mathf.MoveTowards(VelocityMagnitude, m_CfgMaxRunSpeed, m_CfgAcceleration * Time.deltaTime);
                targetSpeed = targetSpeed > m_CfgMaxRunSpeed ? m_CfgMaxRunSpeed : targetSpeed;

                targetVelocity = inputMove.normalized * targetSpeed;
            }
            if (!playerCanRun && VelocityMagnitude > m_CfgMaxMoveSpeed)
            {
                targetSpeed = Mathf.MoveTowards(VelocityMagnitude, targetSpeed, m_CfgAcceleration * Time.deltaTime);
                targetVelocity = inputMove.normalized * targetSpeed;
            }

            Velocity.Value = targetVelocity;
            m_CCT.SimpleMove(targetVelocity);
            if (!targetVelocity.Approximately(Vector3.zero))
            {
                CachedTransform.rotation = Quaternion.RotateTowards(CachedTransform.rotation, Quaternion.LookRotation(targetVelocity), m_CfgRotateDeg);
            }
        }

        public void RecieveDamage(Player damager, int damageValue)
        {
            if (IsDead) return;

            PlaySfx(Config_Global.Inst.data.PlayerBeHitSfxs.RandomOne()).Forget();
            VfxManager.Inst.PlayVfx(Config_Global.Inst.data.BeHitVfx, CachedTransform, 2f).Forget();

            var dir = damager.Position - Position;
            CachedTransform.rotation = Quaternion.LookRotation(dir);

            LocomotionState.Value = PlayerLocomotionState.Dead;

            // calculate player's score
            GameEvent.Inst.OnPlayerDead?.Invoke(damager.PlayerId, PlayerId);
        }

        private void OnLocomotionChange(PlayerLocomotionState state)
        {
            switch (state)
            {
                case PlayerLocomotionState.Locomotion:
                    m_Animancer.Play(m_AnimData.LocomotionMixer);
                    break;
                case PlayerLocomotionState.Dead:
                    // TODO: rag doll
                    CanMove.RefCnt--;
                    m_Animancer.Play(m_AnimData.Dead);
                    break;
                case PlayerLocomotionState.Ability:
                    break;
            }
        }

        private void OnVelocityChange(Vector3 velocity)
        {
            VelocityMagnitude = velocity.magnitude;
            velocity.y = 0f;
            m_AnimData.LocomotionMixer.State.Parameter = VelocityMagnitude;
        }

        public async UniTask PlaySfx(string res)
        {
            var audioClip = await ResourceMgr.Inst.LoadAsync<AudioClip>(res);
            m_AudioSource.clip = audioClip;
            m_AudioSource.loop = false;
            m_AudioSource.Play();
        }
    }
}