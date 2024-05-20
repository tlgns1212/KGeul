using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class UI_ProfilePopup : UI_Popup
{
    enum Texts
    {
        ProfileTitleText,
        FinishText
    }

    enum Buttons
    {
        FinishButton,
    }

    private void Awake()
    {
        Init();
    }

    public event Action PopupEnd;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.FinishButton).gameObject.BindEvent(OnClickFinishButton);
        Refresh();

        return true;
    }

    public void SetInfo()
    {
        Refresh();
    }

    void Refresh()
    {

    }

    void OnClickFinishButton()
    {
        PopupEnd?.Invoke();
        gameObject.SetActive(false);
    }

    protected override void LocalizeAllTexts()
    {
        Locale currentLanguage = LocalizationSettings.SelectedLocale;
        for (int i = 0; i < System.Enum.GetValues(typeof(Texts)).Length; i++)
        {
            GetText(i).text = LocalizationSettings.StringDatabase.GetLocalizedString("MyTable", System.Enum.GetName(typeof(Texts), i), currentLanguage);
        }
    }
}
