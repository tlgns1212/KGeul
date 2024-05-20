using System.Collections;
using System.Collections.Generic;
using Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;


public class UI_SectionGroup : UI_Base
{
    enum Images
    {
        SectionInfoImage
    }

    enum Texts
    {
        TitleText
    }

    public SectionItemData _data;
    private UI_LearnPopup _learnPopupUI;
    public List<UI_StepGroup> _stepItemList = new List<UI_StepGroup>();
    UI_NextSection _nextSection;

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindImage(typeof(Images));

        _learnPopupUI = FindAnyObjectByType<UI_LearnPopup>();

        return true;
    }

    public void SetInfo(SectionItemData sectionData)
    {
        _data = sectionData;
        GetText((int)Texts.TitleText).text = sectionData.TitleText;

        float _totalSize = 0;
        foreach (int numId in sectionData.StepIds)
        {
            if (Managers.Data.StepItemDataDic.TryGetValue(numId, out StepItemData stepData))
            {
                UI_StepGroup stepItem = Managers.UI.MakeSubItem<UI_StepGroup>(transform);
                stepItem.SetInfo(stepData);
                _learnPopupUI._stepItems.Add(stepData.Id, stepItem);
                _stepItemList.Add(stepItem);
                _totalSize += stepItem.gameObject.GetOrAddComponent<RectTransform>().sizeDelta.y;
            }
        }
        _nextSection = Managers.UI.MakeSubItem<UI_NextSection>(transform);
        _nextSection.SetInfo(sectionData);

        GetComponent<RectTransform>().sizeDelta = new Vector2(1080, _totalSize);
        LocalizeAfterSetInfo();
    }

    public void ShowAllSteps()
    {
        gameObject.SetActive(true);
        float totalY = 350;
        _stepItemList.ForEach((step) =>
        {
            step.Show();
            totalY += step._totalSize.y + 250;
        });
        transform.GetOrAddComponent<RectTransform>().sizeDelta = new Vector2(1080, totalY + 450);
        transform.parent.GetOrAddComponent<RectTransform>().sizeDelta = new Vector2(1080, totalY + 450);
    }

    void LocalizeAfterSetInfo()
    {
        Locale currentLanguage = LocalizationSettings.SelectedLocale;
        for (int i = 0; i < System.Enum.GetValues(typeof(Texts)).Length; i++)
        {
            GetText(i).text = LocalizationSettings.StringDatabase.GetLocalizedString("MyTable", _data.Id + System.Enum.GetName(typeof(Texts), i), currentLanguage);
        }
    }
}