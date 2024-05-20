using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class UI_StepItemButton : UI_Base
{
    enum Texts
    {
        StepText,
        TitleText,
        DescriptionText
    }

    private void Awake()
    {
        Init();
    }

    public int _id;
    string _stepText;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));

        return true;
    }

    public void SetInfo(int id, int seq, string titleText, string descriptionText)
    {
        _id = id;

        Locale currentLanguage = LocalizationSettings.SelectedLocale;
        _stepText = LocalizationSettings.StringDatabase.GetLocalizedString("MyTable", "StepText", currentLanguage);
        GetText((int)Texts.StepText).text = _stepText + " " + seq + ". ";
        GetText((int)Texts.TitleText).text = titleText;
        GetText((int)Texts.DescriptionText).text = descriptionText;

        LocalizeAfterSetInfo();
    }

    void LocalizeAfterSetInfo()
    {
        Locale currentLanguage = LocalizationSettings.SelectedLocale;
        for (int i = 1; i < System.Enum.GetValues(typeof(Texts)).Length; i++)
        {
            GetText(i).text = LocalizationSettings.StringDatabase.GetLocalizedString("MyTable", _id + System.Enum.GetName(typeof(Texts), i), currentLanguage);
        }
    }
}
