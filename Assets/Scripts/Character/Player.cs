using Animancer;
using ConfigAuto;
using Cysharp.Threading.Tasks;
using R3;
using Sirenix.OdinInspector;
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

    public interface ICanAttack
    {
        public IntAsBool CanAttack { get; set; }
    }


    public class Player : MonoEntity, IBuffOwner, ICanAttack, IInputSlowDown, IIceWalkAccScale
    {
        /// <summary>
        /// 玩家ID, 正常玩家ID为 1 或者 2，0表示AI
        /// </summary>
        public int PlayerId;

        [SerializeField] private float m_CfgMaxMoveSpeed = 2.4f;
        [SerializeField] private float m_CfgMaxRunSpeed = 3.5f;
        [SerializeField] private float m_CfgMaxSlideSpeed = 4f;
        [SerializeField] private float m_CfgAcceleration = 20f;
        [SerializeField] private float m_CfgRotateDeg = 10f;

        [SerializeField] private CharacterController m_CCT;
        [SerializeField] private AnimancerComponent m_Animancer;
        [SerializeField] public PlayerAnimData m_AnimData;

        [SerializeField] public TriggerHelper m_PunchAttackTrigger;
        [SerializeField] public TriggerHelper m_HeadAttackTrigger;

        private List<PlayerAbility> m_Abilities = new();
        private BevTree m_BehaviorTree;

        public Vector3 InputMoveDir;
        public bool InputRun;
        public bool InputAttack;
        public bool InputUseSkill;

        public readonly ReactiveProperty<PlayerLocomotionState> LocomotionState = new();
        public readonly ReactiveProperty<Vector3> LocomotionVelocity = new(); // 动画速度，代表人物本身的速度, 这个需要保存下来用于计算动画的平滑过渡
        [ReadOnly][ShowInInspector] public Vector3 Velocity { get; private set; } // 玩家实际在移动的速度，用于CCT Move 以及计算地面摩擦

        public bool IsDead => LocomotionState.Value == PlayerLocomotionState.Dead;
        public bool IsAI => m_BehaviorTree != null;
        [ReadOnly] public IntAsBool CanMove;
        [ReadOnly][ShowInInspector] public IntAsBool CanAttack { get; set; }
        [ReadOnly][ShowInInspector] public float InputSlowDownScale { get; set; }
        [ShowInInspector] public float SpeedUpReduction { get; set; }
        [ShowInInspector] public float SpeedDownScale { get; set; }
        public BuffContainer BuffContainer { get; private set; }

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
            BuffContainer = new BuffContainer(this);
            m_Abilities.Add(new PlayerPunchAttackAbility());

            foreach (var ability in m_Abilities)
            {
                ability.Init(this);
            }

            LocomotionState.Subscribe(OnLocomotionStateChange);
            LocomotionVelocity.Subscribe(OnLocomotionVelocityChange);

            m_PunchAttackTrigger.SetActiveEx(false);
            m_HeadAttackTrigger.SetActiveEx(false);
            GameEvent.Inst.LevelBooyahPostAction += LevelBooyahPost;
        }

        private void OnEnable()
        {
            m_Animancer.Play(m_AnimData.LocomotionMixer);
        }

        private void OnDisable()
        {

        }

        private void Start()
        {
            CanMove++;
            CanAttack++;
            LocomotionState.Value = PlayerLocomotionState.Locomotion;
        }

        private void Update()
        {
            BuffContainer.Update(Time.deltaTime);

            if (!IsDead)
            {
                m_BehaviorTree?.OnUpdate(Time.deltaTime);

                foreach (var ability in m_Abilities)
                {
                    ability.Update(Time.deltaTime);
                }

                UpdateMoveCommand();
            }
        }

        private void LateUpdate()
        {
            InputAttack = false;
        }

        private void LevelBooyahPost()
        {
            CanMove--;
            CanAttack--;
            LocomotionVelocity.Value = Vector3.zero;
        }

        public void SetIsAI()
        {
            m_BehaviorTree = BevTree.CreateBevTree();
            m_BehaviorTree.Init(this);
        }

        public void PlayAnim(ClipTransition anim)
        {
            m_Animancer.Play(anim, fadeDuration: 0.15f, mode: FadeMode.FromStart);
        }

        private void UpdateMoveCommand()
        {
            if (!CanMove || IsDead) return;
            var input = InputMoveDir;
            if (input.sqrMagnitude < 0.2) input = Vector3.zero;
            input *= (1 - InputSlowDownScale);

            var inputVelocity = CalMotionVelocity(input);
            LocomotionVelocity.Value = inputVelocity; // 角色的移动动画表现以玩家输入为准

            var moveVelocity = CalGroundFrictionVelocity(inputVelocity);
            Velocity = moveVelocity;

            m_CCT.SimpleMove(moveVelocity);
            if (!inputVelocity.Approximately(Vector3.zero))
            {
                var targetRot = Quaternion.LookRotation(inputVelocity);
                var angle = Quaternion.Angle(CachedTransform.rotation, targetRot);

                if (angle < 120)
                {
                    CachedTransform.rotation = Quaternion.RotateTowards(CachedTransform.rotation, Quaternion.LookRotation(inputVelocity), m_CfgRotateDeg);
                }
                else
                {
                    CachedTransform.rotation = targetRot;
                }
            }
        }

        private Vector3 CalGroundFrictionVelocity(Vector3 motionVelocity)
        {
            // TODO_OR_NEVER: 角色的移动速度和转向速度要加入地面摩擦系数
            // 但目前这样的实现效果其实也还不错

            motionVelocity *= (1 - SpeedUpReduction);
            var oldVelocity = Velocity * SpeedDownScale;
            var velocity = motionVelocity + oldVelocity;
            return velocity.ClampMagnitude(0, m_CfgMaxSlideSpeed);
        }

        private Vector3 CalMotionVelocity(Vector3 inputMoveDir)
        {
            var inputVelocity = inputMoveDir * m_CfgMaxMoveSpeed;
            var inputSpeed = inputVelocity.magnitude;
            var moveDir = !inputMoveDir.ApproximatelyZero() ? inputMoveDir.normalized : LocomotionVelocity.Value.normalized;

            float targetSpeed;
            var playerCanRun = InputRun && inputSpeed.Approximately(m_CfgMaxMoveSpeed);
            if (playerCanRun)
            {
                targetSpeed = Mathf.MoveTowards(LocomotionVelocity.Value.magnitude, m_CfgMaxRunSpeed, m_CfgAcceleration * Time.deltaTime);
                targetSpeed = Mathf.Min(targetSpeed, m_CfgMaxRunSpeed);
            }
            else
            {
                targetSpeed = Mathf.MoveTowards(LocomotionVelocity.Value.magnitude, inputSpeed, m_CfgAcceleration * Time.deltaTime);
            }

            return moveDir * targetSpeed;
        }

        public void RecieveDamage(Player damager, int damageValue)
        {
            if (IsDead || damager.IsDead || LevelController.Inst.IsBooyah) return;

            AudioManager.Inst.Play2DSfx(Config_Global.Inst.data.Sfx.PlayerBeHitSfxs.RandomOne()).Forget();
            VfxManager.Inst.PlayVfx(Config_Global.Inst.data.Vfx.BeHitVfx, CachedTransform, 2f).Forget();

            var dir = damager.Position - Position;
            CachedTransform.rotation = Quaternion.LookRotation(dir);

            LocomotionState.Value = PlayerLocomotionState.Dead;

            GameEvent.Inst.PlayerDeadPostAction?.Invoke(damager.PlayerId, PlayerId);
        }

        private void OnLocomotionStateChange(PlayerLocomotionState state)
        {
            switch (state)
            {
                case PlayerLocomotionState.Locomotion:
                    m_Animancer.Play(m_AnimData.LocomotionMixer);
                    break;
                case PlayerLocomotionState.Dead:
                    m_Animancer.Play(m_AnimData.Dead);
                    break;
                case PlayerLocomotionState.Ability:
                    break;
            }
        }

        private void OnLocomotionVelocityChange(Vector3 value)
        {
            m_AnimData.LocomotionMixer.State.Parameter = value.magnitude;
        }
    }
}