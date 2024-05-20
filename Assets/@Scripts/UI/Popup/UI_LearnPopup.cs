using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data;
using DG.Tweening;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_LearnPopup : UI_Popup
{
    enum GameObjects
    {
        Content,
        ContentObject
    }

    public Dictionary<int, UI_SectionGroup> _sectionItems = new Dictionary<int, UI_SectionGroup>();
    public Dictionary<int, UI_StepGroup> _stepItems = new Dictionary<int, UI_StepGroup>();
    public Dictionary<int, UI_UnitGroup> _unitItems = new Dictionary<int, UI_UnitGroup>();
    private UI_SelectToggle _selectToggleUI;
    public UI_SelectToggle SelectToggleUI
    {
        get { return _selectToggleUI; }
    }

    // private void OnDestroy()
    // {
    //     if (Managers.Game != null)
    //         Managers.Game.OnCurToggleChanged -= HideAndShowStart;
    // }

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));

        _selectToggleUI = Managers.UI.MakeSubItem<UI_SelectToggle>(transform);
        GetObject((int)GameObjects.ContentObject).GetOrAddComponent<ScrollRect>().onValueChanged.AddListener(HideSelectToggle);
        Refresh();

        return true;
    }

    public void SetInfo()
    {
        Refresh();

        foreach (Data.SectionItemData sectionData in Managers.Data.SectionItemDataDic.Values)
        {
            UI_SectionGroup sectionGroup = Managers.UI.MakeSubItem<UI_SectionGroup>(GetObject((int)GameObjects.Content).transform);
            sectionGroup.SetInfo(sectionData);
            _sectionItems.Add(sectionData.Id, sectionGroup);
        }

        foreach (UI_UnitGroup unitItem in _unitItems.Values)
        {
            unitItem.SetToggles();
        }
    }


    void Refresh()
    {

    }

    void HideSelectToggle(Vector2 value)
    {
        if (SelectToggleUI._isHiding)
            return;

        SelectToggleUI.Hide();
    }

    public IEnumerator ContentGoToUnit(UI_UnitGroup unitItem, Action callback)
    {
        GetObject((int)GameObjects.ContentObject).GetOrAddComponent<ScrollRect>().decelerationRate = 0;
        yield return null;
        float duration = 0.2f;
        if (_stepItems.TryGetValue(unitItem._data.Pid, out UI_StepGroup stepItem))
        {
            GetObject((int)GameObjects.Content).GetOrAddComponent<RectTransform>().DOAnchorPos3DY(-(unitItem.GetOrAddComponent<RectTransform>().anchoredPosition.y + stepItem.GetOrAddComponent<RectTransform>().anchoredPosition.y + 800), duration).SetEase(Ease.InOutSine)
            .OnComplete(() => callback?.Invoke());
        }
        GetObject((int)GameObjects.ContentObject).GetOrAddComponent<ScrollRect>().decelerationRate = 0.135f;
    }

    public IEnumerator ContentGoToY(UI_StepGroup stepItem)
    {
        GetObject((int)GameObjects.ContentObject).GetOrAddComponent<ScrollRect>().decelerationRate = 0;
        yield return null;
        int stepId = stepItem._data.Id;
        foreach (UI_SectionGroup section in _sectionItems.Values)
        {
            section.gameObject.SetActive(false);
            if (section._data.StepIds.Contains(stepId))
            {
                section.ShowAllSteps();
            }
        }
        yield return null;
        float duration = 0.2f;
        GetObject((int)GameObjects.Content).GetOrAddComponent<RectTransform>().DOAnchorPos3DY(-stepItem.GetOrAddComponent<RectTransform>().anchoredPosition.y, duration).SetEase(Ease.InOutSine);
        GetObject((int)GameObjects.ContentObject).GetOrAddComponent<ScrollRect>().decelerationRate = 0.135f;
    }

    public IEnumerator ContentGoToCurrentUnit()
    {
        GetObject((int)GameObjects.ContentObject).GetOrAddComponent<ScrollRect>().decelerationRate = 0;
        yield return null;
        GameObject curUnit = Managers.Game.CurrentUnitObject;
        int sectionId = curUnit.transform.parent.parent.parent.GetOrAddComponent<UI_SectionGroup>()._data.Id;
        foreach (UI_SectionGroup section in _sectionItems.Values)
        {
            section.gameObject.SetActive(false);
            if (section._data.Id == sectionId)
            {
                section.ShowAllSteps();
            }
        }
        float duration = 0.2f;
        float posY = -(curUnit.transform.parent.parent.GetOrAddComponent<RectTransform>().anchoredPosition.y
        + curUnit.GetOrAddComponent<RectTransform>().anchoredPosition.y);
        GetObject((int)GameObjects.Content).GetOrAddComponent<RectTransform>().DOAnchorPos3DY(posY, duration).SetEase(Ease.InOutSine);
        GetObject((int)GameObjects.ContentObject).GetOrAddComponent<ScrollRect>().decelerationRate = 0.135f;
    }

    private void OnDestroy()
    {
    }

}
