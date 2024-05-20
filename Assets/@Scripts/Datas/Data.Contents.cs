using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{

    #region  SectionItemData
    public class SectionItemData
    {
        public int Id;
        public int Seq;
        public int NextId;
        public string TitleText;
        public string DescriptionText;
        public List<int> StepIds;
    }
    [Serializable]
    public class SectionItemDataLoader : ILoader<int, SectionItemData>
    {
        public List<SectionItemData> SectionItems = new List<SectionItemData>();
        public Dictionary<int, SectionItemData> MakeDict()
        {
            Dictionary<int, SectionItemData> dict = new Dictionary<int, SectionItemData>();
            foreach (SectionItemData sectionItemData in SectionItems)
                dict.Add(sectionItemData.Id, sectionItemData);
            return dict;
        }
    }
    #endregion


    #region  StepItemData
    public class StepItemData
    {
        public int Pid;
        public int Id;
        public int Seq;
        public int NextId;
        public string TitleText;
        public string DescriptionText;
        public List<int> UnitIds;
        public List<int> DecoIds;
    }
    [Serializable]
    public class StepItemDataLoader : ILoader<int, StepItemData>
    {
        public List<StepItemData> StepItems = new List<StepItemData>();
        public Dictionary<int, StepItemData> MakeDict()
        {
            Dictionary<int, StepItemData> dict = new Dictionary<int, StepItemData>();
            foreach (StepItemData stepItemData in StepItems)
                dict.Add(stepItemData.Id, stepItemData);
            return dict;
        }
    }
    #endregion

    #region UnitItemData
    public class UnitItemData
    {
        public int Ppid;
        public int Pid;
        public int Id;
        public int Seq;
        public int NextId;
        public string TitleText;
        public string DescriptionText;
        public float PosX;
        public float PosY;
        public string ImageId;
        public string DisableImageId;
        public string LockImageId;
        public bool IsLocked;
        public bool IsRewarded;
        public int CurrentStage;
        public int TotalStage;
        public List<int> ActivityIds;
    }
    [Serializable]
    public class UnitItemDataLoader : ILoader<int, UnitItemData>
    {
        public List<UnitItemData> UnitItems = new List<UnitItemData>();
        public Dictionary<int, UnitItemData> MakeDict()
        {
            Dictionary<int, UnitItemData> dict = new Dictionary<int, UnitItemData>();
            foreach (UnitItemData unitItemData in UnitItems)
                dict.Add(unitItemData.Id, unitItemData);
            return dict;
        }
    }

    #endregion

    #region DecoItemData
    public class DecoItemData
    {
        public int Id;
        public string ImageId;
        public float PosX;
        public float PosY;
    }
    [Serializable]
    public class DecoItemDataLoader : ILoader<int, DecoItemData>
    {
        public List<DecoItemData> DecoItems = new List<DecoItemData>();
        public Dictionary<int, DecoItemData> MakeDict()
        {
            Dictionary<int, DecoItemData> dict = new Dictionary<int, DecoItemData>();
            foreach (DecoItemData decoItemData in DecoItems)
                dict.Add(decoItemData.Id, decoItemData);
            return dict;
        }
    }

    #endregion

    #region ActivityData
    public class ActivityItemData
    {
        public int Pid;
        public int Id;
        public int Seq;
        public List<int> FlowIds;
    }
    [Serializable]
    public class ActivityItemDataLoader : ILoader<int, ActivityItemData>
    {
        public List<ActivityItemData> ActivityItems = new List<ActivityItemData>();
        public Dictionary<int, ActivityItemData> MakeDict()
        {
            Dictionary<int, ActivityItemData> dict = new Dictionary<int, ActivityItemData>();
            foreach (ActivityItemData activityItemData in ActivityItems)
                dict.Add(activityItemData.Id, activityItemData);
            return dict;
        }
    }
    #endregion

    #region FlowItemData
    public class FlowItemData
    {
        public int Id;
        public int TypeId;
        public List<int> Questions;
        public List<int> Answers;
        public List<int> Extras;
    }
    [Serializable]
    public class FlowItemDataLoader : ILoader<int, FlowItemData>
    {
        public List<FlowItemData> FlowItems = new List<FlowItemData>();
        public Dictionary<int, FlowItemData> MakeDict()
        {
            Dictionary<int, FlowItemData> dict = new Dictionary<int, FlowItemData>();
            foreach (FlowItemData flowItemData in FlowItems)
                dict.Add(flowItemData.Id, flowItemData);
            return dict;
        }
    }
    #endregion

    #region TextItemData
    public class TextItemData
    {
        public int Id;
        public string Text;
        public string SText;
        public string AText;
        public int PosX;
        public int PosY;
    }
    [Serializable]
    public class TextItemDataLoader : ILoader<int, TextItemData>
    {
        public List<TextItemData> TextItems = new List<TextItemData>();
        public Dictionary<int, TextItemData> MakeDict()
        {
            Dictionary<int, TextItemData> dict = new Dictionary<int, TextItemData>();
            foreach (TextItemData textItemData in TextItems)
                dict.Add(textItemData.Id, textItemData);
            return dict;
        }
    }
    #endregion

}
