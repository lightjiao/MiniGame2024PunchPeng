using PunchPeng;
using System.Collections.Generic;

public class BevParentTask : BevTask
{
    protected List<BevTask> m_Tasks = new();

    public virtual void AddTask(BevTask task)
    {
        m_Tasks.Add(task);
    }

    public override void OnBevAwake(Player player)
    {
        base.OnBevAwake(player);
        foreach (var item in m_Tasks)
        {
            item.OnBevAwake(player);
        }
    }
}
