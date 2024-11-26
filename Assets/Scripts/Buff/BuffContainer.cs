namespace PunchPeng
{
    // TODO: 
    // Ability、Buff、Behavior，每一个独特的buff、ability 对 owner 要求的接口都不一样
    // 以buff系统为例，可以尝试把每个buff需要的接口单独声明，但buff里将owner强转为对应的interface，并调用方法
    public class BuffContainer
    {
        protected Player m_Player;
        private int UidGen;

        public void AddBuff()
        {
            UidGen++;
            // create buff
        }

        public void RemoveBuff(int buffUid)
        {

        }
    }
}