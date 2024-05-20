using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<int, Data.SectionItemData> SectionItemDataDic { get; private set; } = new Dictionary<int, Data.SectionItemData>();
    public Dictionary<int, Data.StepItemData> StepItemDataDic { get; private set; } = new Dictionary<int, Data.StepItemData>();
    public Dictionary<int, Data.UnitItemData> UnitItemDataDic { get; private set; } = new Dictionary<int, Data.UnitItemData>();
    public Dictionary<int, Data.ActivityItemData> ActivityItemDataDic { get; private set; } = new Dictionary<int, Data.ActivityItemData>();
    public Dictionary<int, Data.DecoItemData> DecoItemDataDic { get; private set; } = new Dictionary<int, Data.DecoItemData>();
    public Dictionary<int, Data.FlowItemData> FlowItemDataDic { get; private set; } = new Dictionary<int, Data.FlowItemData>();
    public Dictionary<int, Data.TextItemData> TextItemDataDic { get; private set; } = new Dictionary<int, Data.TextItemData>();

    public void Init()
    {
        if (SectionItemDataDic.Count > 1)
        {
            ReLogin();
            return;
        }

        SectionItemDataDic = LoadJson<Data.SectionItemDataLoader, int, Data.SectionItemData>("SectionItemData").MakeDict();
        StepItemDataDic = LoadJson<Data.StepItemDataLoader, int, Data.StepItemData>("StepItemData").MakeDict();
        UnitItemDataDic = LoadJson<Data.UnitItemDataLoader, int, Data.UnitItemData>("UnitItemData").MakeDict();
        ActivityItemDataDic = LoadJson<Data.ActivityItemDataLoader, int, Data.ActivityItemData>("ActivityItemData").MakeDict();
        DecoItemDataDic = LoadJson<Data.DecoItemDataLoader, int, Data.DecoItemData>("DecoItemData").MakeDict();
        FlowItemDataDic = LoadJson<Data.FlowItemDataLoader, int, Data.FlowItemData>("FlowItemData").MakeDict();
        TextItemDataDic = LoadJson<Data.TextItemDataLoader, int, Data.TextItemData>("TextItemData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"{path}");
        return JsonConvert.DeserializeObject<Loader>(textAsset.text);
    }

    public void ReLogin()
    {
        UnitItemDataDic.Clear();
        UnitItemDataDic = LoadJson<Data.UnitItemDataLoader, int, Data.UnitItemData>("UnitItemData").MakeDict();
    }
}