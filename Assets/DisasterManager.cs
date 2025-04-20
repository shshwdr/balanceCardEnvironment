using System.Collections;
using System.Collections.Generic;
using Pool;
using UnityEngine;

public class DisasterManager : Singleton<DisasterManager>
{
    public List<DisasterInfo> disasters = new List<DisasterInfo>();

    public void AddDisaster(DisasterInfo info)
    {
        disasters.Add(info);
        EventPool.Trigger("DisasterChanged");
    }

    public void ClearDisaster()
    {
        disasters.Clear();
        EventPool.Trigger("DisasterChanged");
    }
}
