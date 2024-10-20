using UnityEngine;
using ConfigAuto;

namespace PunchPeng
{
    public class GameController : MonoBehaviour
    {
        private Player m_Player1;
        private Player m_Player2;
        private Player m_Player3;
        private Player m_Player4;

        private bool m_Init;

        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        private async void Start()
        {
            await LevelMgr.Inst.LoadLevelAsync(LevelMgr.TestLevelScene);

            // TODO: rand pos
            m_Player1 = await ResourceMgr.Inst.InstantiateAsync<Player>(Config_Player.Inst.PlayerPrefab);
            m_Player2 = await ResourceMgr.Inst.InstantiateAsync<Player>(Config_Player.Inst.PlayerPrefab);
            m_Player2.CachedTransform.position = (Vector3.one * 2).SetY(0);

            m_Init = true;
        }

        private void Update()
        {
            Check1PInput();
            Check2PInput();
        }

        public void Check1PInput()
        {
            if (m_Player1 == null) return;

            var rawInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            var squreInput = Vector3Ex.SquareToCircle(rawInput);
            //Debug.LogError(rawInput.ToStringEx() + squreInput.ToStringEx());
            m_Player1.PlayerInputMoveDir.Value = squreInput;
            //m_Player1.PlayerInputRun.Value = Input.GetButton("Run");
            //m_Player1.PlayerInputAttack.Value = Input.GetButtonDown("Attack");
            m_Player1.PlayerInputRun.Value = Input.GetKey(KeyCode.LeftShift);
            m_Player1.PlayerInputAttack.Value = Input.GetKey(KeyCode.Space);
        }

        public void Check2PInput()
        {
            if (m_Player2 == null) return;

            //var rawInput = new Vector3(Input.GetAxis("Horizontal2"), 0, Input.GetAxis("Vertical2"));
            //var squreInput = Vector3Ex.SquareToCircle(rawInput);
            ////Debug.LogError("Player2Input :" + rawInput.ToStringEx() + squreInput.ToStringEx());
            //m_Player2.PlayerInputMoveDir.Value = squreInput;
            //m_Player2.PlayerInputRun.Value = Input.GetButton("Run2");
            //m_Player2.PlayerInputAttack.Value = Input.GetButtonDown("Attack2");
            //m_Player2.PlayerInputAttack.Value = false;
        }
    }
}