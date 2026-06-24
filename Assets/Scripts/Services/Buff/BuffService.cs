using System.Collections.Generic;
using UnityEngine;

public class BuffService : IBuffService
{
   List<Buff> _buffs;
    public BuffService() { 
    _buffs = new List<Buff>();
    }
    public void AddBuff(Buff buff)
    {
        _buffs.Add(buff);
    }

    public void ClearBuffList()
    {
       _buffs.Clear();

    }
    public List<Buff> GetBuffList()
    {
        return _buffs;
    }

    public void ReduceDuration()
    {
        foreach (Buff buff in _buffs)
        {
            buff.duration--;
        }

        _buffs.RemoveAll(buff => buff.duration <= 0);
    }

    public void RemoveBuffByGUID(string GUID)
    {
        _buffs.RemoveAll(b => b.Owner == GUID);
    }
}
