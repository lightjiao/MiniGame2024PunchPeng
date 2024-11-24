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
        [ReadOnly] public bool GameIsStart;

        // 纯玩家
        public Config_Global.LevelCfg m_CurLevelCfg;

        protected override void OnAwake()
        {
            Inst = this;
            Application.targetFrameRate = Config_Global.Inst.data.TargetFrameRate;
            GameEvent.Inst.OnGameStart += OnGameStartAsync;
            GameEvent.Inst.OnPlayerDead += OnPlayerDeadToFinishGame;
            _ = ScoreboardManager.Inst;

            GameIsStart = false;
        }

        private void Update()
        {
            PlayerInputManagerHelper.Inst.OnUpdate();
            VfxManager.Inst.OnUpdate(Time.deltaTime);
        }

        private async UniTask OnGameStartAsync()
        {
            VfxManager.Inst.ReleaseAll();

            m_CurLevelCfg = Config_Global.Inst.data.LevelCfg[0];
            var playBGM = AudioManager.Inst.PlayBGM(m_CurLevelCfg.BGMRes);
            await LevelMgr.Inst.LoadLevelAsync(m_CurLevelCfg.Scene);
            await SpawnPlayersAsync();

            await playBGM;

            GameIsStart = true;
        }

        private async UniTask EndGameAsync()
        {
            GameIsStart = false;

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
                Destroy(player.gameObject);
            }
            PlayerList.Clear();

            //var testOneAI = true;
            //if (testOneAI)
            //{
            //    for (int i = 0; i < 1; i++)
            //    {
            //        var player = await ResourceMgr.Inst.InstantiateAsync<Player>(Config_Global.Inst.data.PlayerPrefab);
            //        PlayerList.Add(player);
            //    }
            //}
            //else
            {
                for (int i = 0; i < Config_Global.Inst.data.TotalPlayerCount; i++)
                //for (int i = 0; i < 3; i++)
                {
                    var player = await ResourceMgr.Inst.InstantiateAsync<Player>(Config_Global.Inst.data.PlayerPrefab);
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
            foreach (var player in PlayerList)
            {
                player.Position = Vector3Util.RandomRange(LevelArea.Inst.Min, LevelArea.Inst.Max);
                player.Forward = Vector3Util.Rand2DDir();

                if (player != m_Player1 && player != m_Player2)
                {
                    player.PlayerId = aiId;
                    aiId--;
                    player.name += $" AI:[{aiId}]";
                    player.SetIsAI();
                }
                else
                {
                    player.name += $" Player:[{player.PlayerId}]";
                }
            }
        }

        private void OnPlayerDeadToFinishGame(int killer, int deadPlayer)
        {
            if (deadPlayer <= 0) return;

            foreach (var item in PlayerList)
            {
                item.OnGameEnd();
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

            VfxManager.Inst.PlayVfx(Config_Global.Inst.data.Vfx.WinnerVfx, winPlayer.Position, 10).Forget();
            AudioManager.Inst.Play2DSfx(Config_Global.Inst.data.Sfx.WinSfx, true, 0.1f).Forget();

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