using Animancer;
using Cysharp.Threading.Tasks.Triggers;
using R3;
using System;
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
        private readonly float MoveSpeed = 1.6f;
        private readonly float RunSpeed = 3.2f;
        private readonly float MaxRotateDeg = 0.3f;

        [SerializeField] private CharacterController m_CCT;
        [SerializeField] private AnimancerComponent m_Animancer;
        [SerializeField] public PlayerAnimData m_AnimData;

        [SerializeField] public TriggerHelper m_PunchAttackTrigger;
        [SerializeField] public TriggerHelper m_HeadAttackTrigger;

        public bool CanMove;
        public bool IsDead => LocomotionState.Value == PlayerLocomotionState.Dead;
        private List<PlayerAbility> m_Abilities = new();

        public readonly ReactiveProperty<Vector3> PlayerInputMoveDir = new();
        public readonly ReactiveProperty<bool> PlayerInputRun = new();
        public readonly ReactiveProperty<bool> PlayerInputAttack = new();

        public readonly ReactiveProperty<PlayerLocomotionState> LocomotionState = new();
        public readonly ReactiveProperty<Vector3> Velocity = new(default, Vector3Comparer.Default);

        private HashSet<IDisposable> m_R3Disposable;

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

            m_R3Disposable = new HashSet<IDisposable>
            {
                PlayerInputMoveDir, PlayerInputRun, PlayerInputAttack, LocomotionState, Velocity
            };

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
            foreach (var item in m_R3Disposable)
            {
                item.Dispose();
            }
            m_R3Disposable.Clear();
        }

        public void PlayAnim(ClipTransition anim)
        {
            m_Animancer.Play(anim);
        }

        public void SimpleMove(Vector3 moveDir)
        {
            if (!CanMove) return;

            var velocity = (PlayerInputRun.Value ? RunSpeed : MoveSpeed) * moveDir;

            Velocity.Value = velocity;
            m_CCT.SimpleMove(velocity);
            if (!velocity.Approximately(Vector3.zero))
            {
                CachedTransform.rotation = Quaternion.RotateTowards(CachedTransform.rotation, Quaternion.LookRotation(velocity), 10);
            }
        }

        public void RecieveDamage(int damageValue)
        {
            // TODO: play sfx and vfx
            LocomotionState.Value = PlayerLocomotionState.Dead;
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
                    var deadAnimIdx = UnityEngine.Random.Range(0, 100) & 1;
                    m_Animancer.Play(deadAnimIdx == 0 ? m_AnimData.Dead1 : m_AnimData.Dead2);
                    break;
                case PlayerLocomotionState.Ability:
                    Debug.Log("Ability");
                    break;
            }
        }

        private void OnVelocityChange(Vector3 velocity)
        {
            velocity.y = 0f;
            m_AnimData.LocomotionMixer.State.Parameter = velocity.magnitude;
        }
    }
}