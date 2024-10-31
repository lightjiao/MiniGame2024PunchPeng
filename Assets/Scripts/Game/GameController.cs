using ConfigAuto;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace PunchPeng
{
    public class GameController : SingletonMono<GameController>
    {
        // 包含了AI与玩家的列表
        [HideInInspector] public List<Player> PlayerList = new();
        [ReadOnly] public Player m_Player1;
        [ReadOnly] public Player m_Player2;

        // 纯玩家
        private Dictionary<int, Player> m_Players = new();

        protected override void OnAwake()
        {
            Application.targetFrameRate = Config_Global.Inst.data.TargetFrameRate;
            Inst = this;
            GameEvent.Inst.OnGameStart += OnGameStartAsync;
            GameEvent.Inst.OnPlayerDead += OnPlayerDeadToFinishGame;
        }

        private void Update()
        {
            Check1PInput();
            Check2PInput();
            VfxManager.Inst.OnUpdate(Time.deltaTime);
        }

        public void Check1PInput()
        {
            if (m_Player1 == null) return;

            var rawInput = new Vector3(Input.GetAxis("Player1_Horizontal"), 0, Input.GetAxis("Player1_Vertical"));
            var squreInput = Vector3Util.SquareToCircle(rawInput);
            m_Player1.InputMoveDir.Value = squreInput;
            m_Player1.InputRun.Value = Input.GetButton("Player1_Run");
            m_Player1.InputAttack.Value = Input.GetButtonDown("Player1_Attack");
        }

        public void Check2PInput()
        {
            if (m_Player2 == null) return;

            var rawInput = new Vector3(Input.GetAxis("Player2_Horizontal"), 0, Input.GetAxis("Player2_Vertical"));
            var squreInput = Vector3Util.SquareToCircle(rawInput);
            m_Player2.InputMoveDir.Value = squreInput;
            m_Player2.InputRun.Value = !Input.GetAxis("Player2_Run").Approximately(0);
            m_Player2.InputAttack.Value = Input.GetButtonDown("Player2_Attack");
        }

        private async UniTask OnGameStartAsync()
        {
            VfxManager.Inst.ReleaseAll();

            var randomLevel = Config_Global.Inst.data.LevelNames.RandomOne();
            var playBGM = AudioManager.Inst.PlayLevelBGM(randomLevel);
            await LevelMgr.Inst.LoadLevelAsync(randomLevel);
            await SpawnPlayersAsync();

            await playBGM;
        }

        private async UniTask EndGameAsync()
        {
            GameEvent.Inst.OnGameEnd?.Invoke();

            m_Player1 = null;
            m_Player2 = null;
            foreach (var item in PlayerList)
            {
                GameObjectUtil.DestroyGo(item.gameObject);
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

            var testOneAI = true;
            //if (testOneAI)
            //{
            //    for (int i = 0; i < 2; i++)
            //    {
            //        var player = await ResourceMgr.Inst.InstantiateAsync<Player>(Config_Global.Inst.data.PlayerPrefab);

            //        player.Position = Vector3Util.RandomRange(LevelArea.Inst.Min, LevelArea.Inst.Max);
            //        player.Forward = Vector3Util.Rand2DDir();

            //        PlayerList.Add(player);
            //    }
            //}
            //else
            {
                for (int i = 0; i < Config_Global.Inst.data.TotalPlayerCount; i++)
                {
                    var player = await ResourceMgr.Inst.InstantiateAsync<Player>(Config_Global.Inst.data.PlayerPrefab);

                    player.Position = Vector3Util.RandomRange(LevelArea.Inst.Min, LevelArea.Inst.Max);
                    player.Forward = Vector3Util.Rand2DDir();

                    PlayerList.Add(player);
                }

                m_Player1 = PlayerList.RandomOne();
                do
                {
                    m_Player2 = PlayerList.RandomOne();
                } while (m_Player1 == m_Player2);
                m_Player1.PlayerId = 1;
                m_Player2.PlayerId = 2;
            }


            var aiId = -1;
            foreach (var item in PlayerList)
            {
                if (item != m_Player1 && item != m_Player2)
                {
                    item.PlayerId = aiId;
                    aiId--;
                    item.name += $" [{aiId}]";
                    item.SetIsAI();
                }
            }
        }

        private void OnPlayerDeadToFinishGame(int killer, int deadPlayer)
        {
            if (deadPlayer <= 0) return;

            foreach (var item in PlayerList)
            {
                item.EndGameStop();
            }

            Player winPlayer = null;
            if (deadPlayer == m_Player1.PlayerId)
            {
                winPlayer = m_Player2;
            }
            if (deadPlayer == m_Player2.PlayerId)
            {
                winPlayer = m_Player1;
            }

            VfxManager.Inst.PlayVfx(Config_Global.Inst.data.WinnerVfx, winPlayer.Position, 10).Forget();
            AudioManager.Inst.PauseBGMPlaySfx(Config_Global.Inst.data.WinSfx).Forget();

            WaitToFinishGame(winPlayer).Forget();
        }

        private async UniTask WaitToFinishGame(Player winPlayer)
        {
            // wait attack anim to finish
            await UniTask.Delay(0.5f.ToMilliSec());
            winPlayer.PlayAnim(winPlayer.m_AnimData.Cheer);

            await UniTask.Delay(5f.ToMilliSec());
            await EndGameAsync();
        }
    }
}