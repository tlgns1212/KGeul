using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class UI_SelectToggle : UI_Base
{
    enum Texts
    {
        TitleText,
        LessonText,
        Start10Text
    }
    enum Images
    {
        BackgroundImage
    }

    enum Buttons
    {
        StartButton
    }

    private void Awake()
    {
        Init();
    }

    Action _action;
    public bool _isHiding = true;
    string _lesson = "Lesson";

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));

        return true;
    }

    public void SetInfo(int id, int curStage, int totalStage, bool isLocked, Action callback)
    {
        _action = callback;
        // GetImage((int)Images.BackgroundImage).sprite = 
        GetText((int)Texts.TitleText).text = id.ToString();
        GetText((int)Texts.LessonText).text = "레슨 " + curStage + "/" + totalStage;
        GetButton((int)Buttons.StartButton).gameObject.BindEvent(HandleOnClickStartButton);

        if (isLocked)
        {
            GetButton((int)Buttons.StartButton).GetOrAddComponent<Image>().color = Color.black;
            GetText((int)Texts.Start10Text).alpha = 0;
            GetButton((int)Buttons.StartButton).GetOrAddComponent<UI_EventHandler>().enabled = false;
        }
        else
        {
            GetButton((int)Buttons.StartButton).GetOrAddComponent<Image>().color = Color.white;
            GetText((int)Texts.Start10Text).alpha = 1;
            GetButton((int)Buttons.StartButton).GetOrAddComponent<UI_EventHandler>().enabled = true;
        }
        LocalizeAfterSetInfo(id, curStage, totalStage);
    }

    void HandleOnClickStartButton()
    {
        _action?.Invoke();
    }

    public void Hide()
    {
        transform.DOScale(0f, 0.1f).SetEase(Ease.InOutBack);
        _isHiding = true;
    }

    void LocalizeAfterSetInfo(int id, int curStage, int totalStage)
    {
        Locale currentLanguage = LocalizationSettings.SelectedLocale;
        GetText((int)Texts.TitleText).text = LocalizationSettings.StringDatabase.GetLocalizedString("MyTable", id + "TitleText", currentLanguage);
        GetText((int)Texts.LessonText).text = _lesson + " " + curStage + "/" + totalStage;
    }

    protected override void LocalizeAllTexts()
    {
        Locale currentLanguage = LocalizationSettings.SelectedLocale;

        for (int i = 1; i < System.Enum.GetValues(typeof(Texts)).Length; i++)
        {
            if (i == 1) _lesson = LocalizationSettings.StringDatabase.GetLocalizedString("MyTable", System.Enum.GetName(typeof(Texts), i), currentLanguage);
            GetText(i).text = LocalizationSettings.StringDatabase.GetLocalizedString("MyTable", System.Enum.GetName(typeof(Texts), i), currentLanguage);
        }
    }
}
