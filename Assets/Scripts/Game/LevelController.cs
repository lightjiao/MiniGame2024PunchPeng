using ConfigAuto;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PunchPeng
{
    public class LevelController : SingletonMono<LevelController>
    {
        // 包含了AI与玩家的列表
        [HideInInspector] public List<Player> PlayerList = new();
        [ReadOnly] public Player m_Player1;
        [ReadOnly] public Player m_Player2;
        [ReadOnly] public bool GameIsStart;

        public Config_Global.LevelCfg CurLevelCfg { get; private set; }

        [ReadOnly] public ReferenceBool HasCopyAI = new();

        public BuffContainer m_BuffContainer = new();
        public ReferenceBool DisableAIBevAttack = new();

        protected override void OnAwake()
        {
            Inst = this;
            Application.targetFrameRate = Config_Global.Inst.data.TargetFrameRate;
            GameEvent.Inst.OnPlayerDead += OnPlayerDeadToFinishLevel;
            _ = ScoreboardManager.Inst;

            GameIsStart = false;
        }

        private void Update()
        {
            m_BuffContainer.Update(Time.deltaTime);
            PlayerInputManagerHelper.Inst.OnUpdate();
            VfxManager.Inst.OnUpdate(Time.deltaTime);
        }

        public async UniTask LevelPreload()
        {
            VfxManager.Inst.ReleaseAll();

            CurLevelCfg = Config_Global.Inst.data.LevelCfg[GameFlowController.Inst.CurLevel];
            await LevelMgr.Inst.LoadLevelAsync(CurLevelCfg.Scene);

            GameEvent.Inst.AfterLevelPreload?.Invoke();
        }

        public async UniTask LevelStart()
        {
            var playBGM = AudioManager.Inst.PlayBGM(CurLevelCfg.BGMRes);
            await playBGM;

            await SpawnPlayersAsync();
            foreach (var item in CurLevelCfg.BuffIds ?? Enumerable.Empty<int>())
            {
                m_BuffContainer.AddBuff(item);
            }
            GameIsStart = true;
        }

        private async UniTask LevelEnd()
        {
            GameEvent.Inst.BeforeLevelEnd?.Invoke();

            GameIsStart = false;

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

                var protectedLoopCnt = 100;
                m_Player1 = PlayerList.RandomOne();
                do
                {
                    protectedLoopCnt--;
                    m_Player2 = PlayerList.RandomOne();
                } while (m_Player1 == m_Player2 && protectedLoopCnt > 0);

                if (protectedLoopCnt <= 0)
                {
                    Log.Error("初始化玩家失败，死循环了");
                }

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

        private void OnPlayerDeadToFinishLevel(int killer, int deadPlayer)
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

            WaitToFinishLevel(winPlayer).Forget();
        }

        private async UniTask WaitToFinishLevel(Player winPlayer)
        {
            // wait attack anim to finish
            await UniTask.Delay(0.5f.ToMilliSec());
            winPlayer.PlayAnim(winPlayer.m_AnimData.Cheer);

            await UniTask.Delay(5f.ToMilliSec());
            await LevelEnd();
        }
    }
}