using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IBuffService
{
    public void AddBuff(Buff buff);
    public void RemoveBuffByGUID(Guid GUID);
     public void ClearBuffList();
    public void ReduceDuration();
    public Buff GetBuff(Guid owner, BuffType type);

}
