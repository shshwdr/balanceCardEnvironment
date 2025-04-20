using System.Collections;
using System.Collections.Generic;
using Pool;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public List<ItemInfo> items = new List<ItemInfo>();

    public void AddItem(ItemInfo info)
    {
        items.Add(info);
        EventPool.Trigger("ItemChanged");
    }

    public void Clear()
    {
        items.Clear();
        EventPool.Trigger("ItemChanged");
    }
}
