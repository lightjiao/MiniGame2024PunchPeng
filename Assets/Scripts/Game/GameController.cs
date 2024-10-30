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
            Application.targetFrameRate = Config_Global.Inst.data.TargetFrameRate;
            Inst = this;
            GameEvent.Inst.OnGameStart += OnGameStartAsync;
        }

        private void Update()
        {
            Check1PInput();
            Check2PInput();
        }

        public void Check1PInput()
        {
            if (m_Player1 == null) return;

            var rawInput = new Vector3(Input.GetAxis("Player1_Horizontal"), 0, Input.GetAxis("Player1_Vertical"));
            var squreInput = Vector3Util.SquareToCircle(rawInput);
            m_Player1.PlayerInputMoveDir.Value = squreInput;
            m_Player1.PlayerInputRun.Value = Input.GetButton("Player1_Run");
            m_Player1.PlayerInputAttack.Value = Input.GetButtonDown("Player1_Attack");
        }

        public void Check2PInput()
        {
            if (m_Player2 == null) return;

            var rawInput = new Vector3(Input.GetAxis("Player2_Horizontal"), 0, Input.GetAxis("Player2_Vertical"));
            var squreInput = Vector3Util.SquareToCircle(rawInput);
            m_Player2.PlayerInputMoveDir.Value = squreInput;
            m_Player2.PlayerInputRun.Value = Input.GetButton("Player2_Run");
            m_Player2.PlayerInputAttack.Value = Input.GetButtonDown("Player2_Attack");
        }

        private async UniTask OnGameStartAsync()
        {
            var levelName = Config_Global.Inst.data.LevelPunchPengScene;
            await LevelMgr.Inst.LoadLevelAsync(levelName);
            await SpawnPlayersAsync();
        }

        private async UniTask EndGame()
        {
            GameEvent.Inst.OnGameEnd?.Invoke();

            m_Player1 = null;
            m_Player2 = null;
            foreach (var item in PlayerList)
            {
                Destroy(item);
            }
            PlayerList.Clear();

            await LevelMgr.Inst.UnLoadCurLevel();
        }

        private async UniTask SpawnPlayersAsync()
        {
            foreach (var player in PlayerList)
            {
                Destroy(player);
            }
            PlayerList.Clear();

            var random = new System.Random();

            for (int i = 0; i < Config_Global.Inst.data.TotalPlayerCount; i++)
            {
                var player = await ResourceMgr.Inst.InstantiateAsync<Player>(Config_Global.Inst.data.PlayerPrefab);

                player.Position = Vector3Util.RandomRange(LevelArea.Inst.Min, LevelArea.Inst.Max);
                player.Forward = Vector3Util.Rand2DDir();
                //Debug.Log("PlayerPos:" + player.Position.ToStringEx());

                PlayerList.Add(player);
            }

            m_Player1 = PlayerList.RandomOne();
            do
            {
                m_Player2 = PlayerList.RandomOne();
            } while (m_Player1 == m_Player2);
            m_Player1.PlayerId = 1;
            m_Player2.PlayerId = 2;

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