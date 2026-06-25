using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffService : IBuffService
{
    List<Buff> _buffs;
    IEventService _eventService;
    public BuffService() { 
        _buffs = new List<Buff>();
        _eventService = AppContainer.Get<IEventService>();
    }
    public void AddBuff(Buff buff)
    {
        _buffs.RemoveAll(b =>
            b.Owner == buff.Owner &&
            b.buffType == buff.buffType);

        _buffs.Add(buff);
        _eventService.Publish(new UpdatePlayerUI());
    }
    public void AddBuff(Buff buff, bool dontDestoy)
    {
        if(!dontDestoy) _buffs.RemoveAll(b =>
                                        b.Owner == buff.Owner &&
                                        b.buffType == buff.buffType);

        _buffs.Add(buff);
        _eventService.Publish(new UpdatePlayerUI());
    }
    public void ClearBuffList()
    {
       _buffs.Clear();
       _eventService.Publish(new UpdatePlayerUI());

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

    public void RemoveBuffByGUID(Guid GUID)
    {
        _buffs.RemoveAll(b => b.Owner == GUID);
    }
    public Buff GetBuff(Guid owner, BuffType type)
    {
        return _buffs.FirstOrDefault(b =>
            b.Owner == owner &&
            b.buffType == type);
    }
}
