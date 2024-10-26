using Animancer;
using R3;
using System.Collections.Generic;
using UnityEngine;

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
        [SerializeField] private float m_CfgMaxMoveSpeed = 2.4f; // 这个速度好像再初始化的时候，对动画不生效
        [SerializeField] private float m_CfgMaxRunSpeed = 3.5f;
        [SerializeField] private float m_CfgAcceleration = 10f;
        [SerializeField] private float m_CfgRotateDeg = 60f;

        [SerializeField] private CharacterController m_CCT;
        [SerializeField] private AnimancerComponent m_Animancer;
        [SerializeField] public PlayerAnimData m_AnimData;

        [SerializeField] public TriggerHelper m_PunchAttackTrigger;
        [SerializeField] public TriggerHelper m_HeadAttackTrigger;

        private List<PlayerAbility> m_Abilities = new();

        public readonly ReactiveProperty<Vector3> PlayerInputMoveDir = new();
        public readonly ReactiveProperty<bool> PlayerInputRun = new();
        public readonly ReactiveProperty<bool> PlayerInputAttack = new();

        public readonly ReactiveProperty<PlayerLocomotionState> LocomotionState = new();
        public readonly ReactiveProperty<Vector3> Velocity = new();
        public float VelocityMagnitude { get; private set; }
        public bool CanMove;
        public bool IsDead => LocomotionState.Value == PlayerLocomotionState.Dead;

        private void Awake()
        {
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
            CanMove = true;
            LocomotionState.Value = PlayerLocomotionState.Locomotion;
        }

        private void Update()
        {
            SimpleMove(PlayerInputMoveDir.Value);
            foreach (var ability in m_Abilities)
            {
                ability.Update(Time.deltaTime);
            }
        }

        private void OnDestroy()
        {
        }

        public void PlayAnim(ClipTransition anim)
        {
            m_Animancer.Play(anim);
        }

        public void SimpleMove(Vector3 moveDir)
        {
            if (!CanMove) return;

            var curSpeed = VelocityMagnitude;
            var targetSpeed = PlayerInputRun.Value ? m_CfgMaxRunSpeed : m_CfgMaxMoveSpeed;
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

            // TODO: play sfx and vfx
            var dir = damager.Position - this.Position;
            CachedTransform.rotation = Quaternion.LookRotation(dir);

            LocomotionState.Value = PlayerLocomotionState.Dead;

            // calculate player's score
            ScoreboardManager.OnPlayerDead?.Invoke(1, 2);
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
                    CanMove = false;
                    m_Animancer.Play(m_AnimData.Dead);
                    break;
                case PlayerLocomotionState.Ability:
                    Debug.Log("Ability");
                    break;
            }
        }

        private void OnVelocityChange(Vector3 velocity)
        {
            VelocityMagnitude = velocity.magnitude;
            velocity.y = 0f;
            m_AnimData.LocomotionMixer.State.Parameter = velocity.magnitude;
        }
    }
}