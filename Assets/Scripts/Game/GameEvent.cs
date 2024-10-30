using Cysharp.Threading.Tasks;
using System;

namespace PunchPeng
{
    public class GameEvent : Singleton<GameEvent>
    {
        protected override void OnInit()
        {
        }

        public Func<UniTask> OnGameStart;
        public Action OnGameEnd;
        public Action<int, int> OnPlayerDead; // <killerId, deadPlayerId>
    }
}