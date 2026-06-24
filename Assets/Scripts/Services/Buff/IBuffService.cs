using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IBuffService
{
    public void AddBuff(Buff buff);
    public void RemoveBuffByGUID(string GUID);
     public void ClearBuffList();
    public void ReduceDuration();

}
