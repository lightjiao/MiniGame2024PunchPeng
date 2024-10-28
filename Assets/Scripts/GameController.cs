using ConfigAuto;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace PunchPeng
{
    public class GameController : MonoBehaviour
    {
        public static GameController Inst;

        [HideInInspector] public List<Player> PlayerList = new List<Player>();
        [ReadOnly] public Player m_Player1;
        [ReadOnly] public Player m_Player2;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            Inst = this;
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
            m_Player1.PlayerInputMoveDir.Value = squreInput;
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

        public async UniTask StartLevelAsync(string levelName)
        {
            await LevelMgr.Inst.LoadLevelAsync(levelName);
            var cameraRes = Config_Global.Inst.data.LevelConfig.GetValueOrDefault(levelName)?.Camera;
            if (cameraRes != null)
            {
                await ResourceMgr.Inst.InstantiateAsync(cameraRes);
            }

            await SpawnPlayersAsync();
        }

        private async UniTask SpawnPlayersAsync()
        {
            foreach (var player in PlayerList)
            {
                Destroy(player);
            }
            PlayerList.Clear();

            var random = new System.Random();

            for (int i = 0; i < 15; i++)
            {
                var player = await ResourceMgr.Inst.InstantiateAsync<Player>(Config_Global.Inst.data.PlayerPrefab);

                player.Position = Vector3Ex.RandomRange(LevelArea.Inst.Min, LevelArea.Inst.Max);
                player.Forward = Vector3Ex.Rand2DDir();

                PlayerList.Add(player);
            }

            m_Player1 = PlayerList.RandomOne();
            do
            {
                m_Player2 = PlayerList.RandomOne();
            } while (m_Player1 == m_Player2);

            foreach (var item in PlayerList)
            {
                if (item != m_Player1 && item != m_Player2)
                {
                    item.SetIsAI();
                }
            }
        }
    }
}