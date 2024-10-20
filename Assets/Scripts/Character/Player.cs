using Animancer;
using R3;
using UnityEngine;

namespace PunchPeng
{
    public enum PlayerLocomotionState
    {
        Locomotion,
        Dead,
        Ability,
    }

    public class Player : MonoBehaviour
    {
        private readonly float MaxMoveSpeed = 4.9f;
        private readonly float MaxRotateDeg = 0.3f;

        [SerializeField] private CharacterController m_CCT;
        [SerializeField] private AnimancerComponent m_Animancer;
        [SerializeField] private AnimationData m_AnimData;

        public Transform CachedTransform;
        public ReactiveProperty<PlayerLocomotionState> LocomotionState = new();
        public ReactiveProperty<Vector3> Velocity = new(default, Vector3Comparer.Default);

        private void Awake()
        {
            CachedTransform = transform;

            LocomotionState.Subscribe(OnLocomotionChange);
            Velocity.Subscribe(OnVelocityChange);
        }

        private void Start()
        {
            LocomotionState.Value = PlayerLocomotionState.Locomotion;
        }

        private void Update()
        {
            PlayerInputMove();
        }

        private void OnDestroy()
        {

        }

        private void PlayerInputMove()
        {
            var moveDir = Vector3Ex.SquareToCircle(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
            SimpleMove(moveDir);
        }

        public void SimpleMove(Vector3 moveDir)
        {
            var velocity = MaxMoveSpeed * moveDir;
            Velocity.Value = velocity;
            //if (Velocity.Value.Approximately(Vector3.zero))
            //{
            //    return;
            //}

            m_CCT.SimpleMove(velocity);
            if (!velocity.Approximately(Vector3.zero))
            {
                CachedTransform.rotation = Quaternion.RotateTowards(CachedTransform.rotation, Quaternion.LookRotation(velocity), 10);
            }
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
                    m_Animancer.Play(m_AnimData.Dead1);
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