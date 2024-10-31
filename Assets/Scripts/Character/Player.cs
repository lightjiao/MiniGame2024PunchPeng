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

        [SerializeField] private float m_CfgMaxMoveSpeed = 2.4f; // 这个速度好像再初始化的时候，对动画不生效
        [SerializeField] private float m_CfgMaxRunSpeed = 3.5f;
        [SerializeField] private float m_CfgAcceleration = 10f;
        [SerializeField] private float m_CfgRotateDeg = 60f;

        [SerializeField] private CharacterController m_CCT;
        [SerializeField] private AnimancerComponent m_Animancer;
        [SerializeField] public PlayerAnimData m_AnimData;

        [SerializeField] public TriggerHelper m_PunchAttackTrigger;
        [SerializeField] public TriggerHelper m_HeadAttackTrigger;
        [SerializeField] private AudioSource m_AudioSource;

        private List<PlayerAbility> m_Abilities = new();
        private BehaviorTree m_BehaviorTree;

        public readonly ReactiveProperty<Vector3> InputMoveDir = new();
        public readonly ReactiveProperty<bool> InputRun = new();
        public readonly ReactiveProperty<bool> InputAttack = new();

        public readonly ReactiveProperty<PlayerLocomotionState> LocomotionState = new();
        public readonly ReactiveProperty<Vector3> Velocity = new();
        public float VelocityMagnitude { get; private set; }
        [ReadOnly] public ReferenceBool CanMove;
        public bool IsDead => LocomotionState.Value == PlayerLocomotionState.Dead;
        public bool IsAI => m_BehaviorTree != null;

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
        }

        private void Update()
        {
            if (!IsDead)
            {
                m_BehaviorTree?.OnUpdate(Time.deltaTime);
                foreach (var ability in m_Abilities)
                {
                    ability.Update(Time.deltaTime);
                }

                UpdateMoveCommand(InputMoveDir.Value);

                InputAttack.Value = false;
                InputRun.Value = false;
                InputMoveDir.Value = Vector3.zero;
            }
        }

        private void LateUpdate()
        {

        }

        private void OnDestroy()
        {
        }

        public void EndGameStop()
        {
            CanMove.RefCnt--;
            Velocity.Value = Vector3.zero;
        }

        public void SetIsAI()
        {
            m_BehaviorTree = new BehaviorTree();
            m_BehaviorTree.Init(this);
        }

        public void PlayAnim(ClipTransition anim)
        {
            m_Animancer.Play(anim, fadeDuration: 0.15f, mode: FadeMode.FromStart);
        }

        private void UpdateMoveCommand(Vector3 moveDir)
        {
            if (!CanMove) return;

            var curSpeed = VelocityMagnitude;
            var targetSpeed = InputRun.Value ? m_CfgMaxRunSpeed : m_CfgMaxMoveSpeed;
            var realSpeed = Mathf.Lerp(curSpeed, targetSpeed, m_CfgAcceleration * Time.deltaTime);
            if (realSpeed < m_CfgMaxMoveSpeed) realSpeed = m_CfgMaxMoveSpeed;

            var realVelocity = realSpeed * moveDir;
            Velocity.Value = realVelocity;
            m_CCT.SimpleMove(realVelocity);
            if (!realVelocity.Approximately(Vector3.zero))
            {
                CachedTransform.rotation = Quaternion.RotateTowards(CachedTransform.rotation, Quaternion.LookRotation(realVelocity), m_CfgRotateDeg);
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
            m_AnimData.LocomotionMixer.State.Parameter = velocity.magnitude;
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