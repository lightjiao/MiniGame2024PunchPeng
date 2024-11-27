using Cysharp.Threading.Tasks;
using System;

namespace PunchPeng
{
    public class GameEvent : Singleton<GameEvent>
    {
        protected override void OnInit()
        {
        }

        public Action AfterLevelPreload;
        public Action AfterLevelStart;
        public Action BeforeLevelEnd;
        public Action<int, int> OnPlayerDead; // <killerId, deadPlayerId>
    }
}