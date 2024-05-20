using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class UI_SectionItem : UI_Base
{
    enum Images
    {
        UpDownImage,
    }

    enum Buttons
    {
        SectionButton,
    }

    enum Texts
    {
        TitleText,
    }

    public Data.SectionItemData _data;
    public float _height = 350;
    private UI_LearnPopup _learnPopupUI;
    private UI_CurriculumPopup _curriculumPopupUI;
    private List<UI_StepItemButton> _stepItemButtons = new List<UI_StepItemButton>();
    private bool _isHide = false;

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        _learnPopupUI = FindFirstObjectByType<UI_LearnPopup>();
        _curriculumPopupUI = FindFirstObjectByType<UI_CurriculumPopup>();

        GetButton((int)Buttons.SectionButton).gameObject.BindEvent(HandleUpdownSectionButton);

        return true;
    }

    public void SetInfo(Data.SectionItemData sectionData)
    {
        _data = sectionData;
        GetText((int)Texts.TitleText).text = sectionData.TitleText;

        foreach (int stepId in sectionData.StepIds)
        {
            if (Managers.Data.StepItemDataDic.TryGetValue(stepId, out Data.StepItemData stepData))
            {
                UI_StepItemButton item = Managers.UI.MakeSubItem<UI_StepItemButton>(transform);
                item.SetInfo(stepData.Id, stepData.Seq, stepData.TitleText, stepData.DescriptionText);
                _stepItemButtons.Add(item);
                item.gameObject.BindEvent(() =>
                {
                    if (_learnPopupUI._stepItems.TryGetValue(item._id, out UI_StepGroup stepItem))
                    {
                        _curriculumPopupUI.OnClickExitButton();
                        Managers.Game.LobbyScene.OnClickLearnButton();
                        StartCoroutine(_learnPopupUI.ContentGoToY(stepItem));
                    }
                });
                _height += 250;
            }
        }
        gameObject.GetOrAddComponent<RectTransform>().sizeDelta = new Vector2(1080, _height);
        LayoutRebuilder.ForceRebuildLayoutImmediate(gameObject.GetOrAddComponent<RectTransform>());
        LocalizeAfterSetInfo();
    }

    void HandleUpdownSectionButton()
    {
        if (!_isHide)
        {
            _isHide = true;
            float duration = 0.2f;
            _stepItemButtons.ForEach((section) =>
            {
                section.transform.DOScaleY(0, duration);
            });
            RectTransform rectT = gameObject.GetOrAddComponent<RectTransform>();
            rectT.DOSizeDelta(new Vector2(1080, 250), duration)
            .OnStart(() => { _curriculumPopupUI._isHideOrShowing = true; })
            .OnComplete(() => { _curriculumPopupUI._isHideOrShowing = false; });
        }
        else
        {
            _isHide = false;
            float duration = 0.2f;
            _stepItemButtons.ForEach((section) =>
            {
                section.transform.DOScaleY(1, duration);
            });
            RectTransform rectT = gameObject.GetOrAddComponent<RectTransform>();
            rectT.DOSizeDelta(new Vector2(1080, _height), duration)
            .OnStart(() => { _curriculumPopupUI._isHideOrShowing = true; })
            .OnComplete(() => { _curriculumPopupUI._isHideOrShowing = false; });
        }
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
