using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class UI_StepGroup : UI_Base
{

    enum Buttons
    {
        StepInfoButton
    }

    enum Images
    {
        UpDownImage
    }

    enum Texts
    {
        TitleText
    }

    enum GameObjects
    {
        StepProgressGroup
    }

    public Data.StepItemData _data;
    private UI_LearnPopup _learnPopupUI;
    public Vector2 _totalSize = new Vector2();
    bool _isHide = false;

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        _learnPopupUI = GameObject.Find("UI_LearnPopup").GetOrAddComponent<UI_LearnPopup>();
        _isHide = false;

        GetButton((int)Buttons.StepInfoButton).gameObject.BindEvent(OnClickUnitHideButton);

        return true;
    }

    public void SetInfo(Data.StepItemData stepData)
    {
        _data = stepData;
        float lastPosY = 0;
        GetText((int)Texts.TitleText).text = "Step " + stepData.Seq + ". " + stepData.TitleText;
        foreach (int unitId in stepData.UnitIds)
        {
            if (Managers.Data.UnitItemDataDic.TryGetValue(unitId, out Data.UnitItemData unitData))
            {
                UI_UnitGroup unitItem = Managers.UI.MakeSubItem<UI_UnitGroup>(GetObject((int)GameObjects.StepProgressGroup).transform);
                unitItem.gameObject.GetOrAddComponent<Toggle>().group = transform.parent.GetOrAddComponent<ToggleGroup>();
                unitItem.SetInfo(unitData);
                _learnPopupUI._unitItems.Add(unitData.Id, unitItem);
                lastPosY = -unitItem.GetOrAddComponent<RectTransform>().anchoredPosition.y;
            }
        }
        foreach (int decoId in stepData.DecoIds)
        {
            if (Managers.Data.DecoItemDataDic.TryGetValue(decoId, out Data.DecoItemData decoData))
            {
                Managers.UI.MakeSubItem<UI_StepDecoItem>(GetObject((int)GameObjects.StepProgressGroup).transform).SetInfo(decoData);
            }
        }
        _totalSize = new Vector2(1080, lastPosY + 400);
        GetObject((int)GameObjects.StepProgressGroup).GetOrAddComponent<RectTransform>().sizeDelta = _totalSize;
        GetComponent<RectTransform>().sizeDelta = _totalSize + new Vector2(0, 250);

        LocalizeAfterSetInfo();
    }

    void OnClickUnitHideButton()
    {
        if (_isHide)
            _isHide = false;
        else
            _isHide = true;
        float duration = 0.3f;
        RectTransform rectT = GetObject((int)GameObjects.StepProgressGroup).GetOrAddComponent<RectTransform>();
        float sectionY = 100;

        foreach (UI_SectionGroup section in _learnPopupUI._sectionItems.Values)
        {
            if (section.gameObject.IsValid())
            {
                foreach (int stepId in section._data.StepIds)
                {
                    if (_learnPopupUI._stepItems.TryGetValue(stepId, out UI_StepGroup step))
                    {
                        sectionY += step._isHide ? 250 : (step._totalSize.y + 250);
                    }
                }
            }
        }
        sectionY += 600;

        DOTween.Sequence().SetAutoKill(false)
        .Append(rectT.DOScaleY(_isHide ? 0 : 1, duration))
        .Join(GetComponent<RectTransform>().DOSizeDelta(_isHide ? new Vector2(1080, 250) : _totalSize + new Vector2(0, 250), duration))
        .Join(transform.parent.GetOrAddComponent<RectTransform>().DOSizeDelta(new Vector2(1080, sectionY), duration))
        .Join(transform.parent.parent.GetOrAddComponent<RectTransform>().DOSizeDelta(new Vector2(1080, sectionY), duration))
        .SetEase(Ease.OutQuint);
    }

    public void Show()
    {
        _isHide = false;
        GetObject((int)GameObjects.StepProgressGroup).GetOrAddComponent<RectTransform>().localScale = Vector3.one;
        GetComponent<RectTransform>().sizeDelta = _totalSize + new Vector2(0, 250);
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
