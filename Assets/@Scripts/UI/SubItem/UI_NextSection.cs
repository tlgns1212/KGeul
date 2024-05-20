using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class UI_NextSection : UI_Base
{
    enum Images
    {
        NextSectionLock,
    }

    enum Texts
    {
        NextSectionButtonText,
        NextSectionInformText,
        TitleText,
        DescriptionText,
    }

    enum Buttons
    {
        NextSectionButton
    }



    private void Awake()
    {
        Init();
    }

    public Data.SectionItemData _data;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));


        return true;
    }

    public void SetInfo(Data.SectionItemData data)
    {

        GetText((int)Texts.TitleText).text = data.TitleText;
        GetText((int)Texts.DescriptionText).text = data.DescriptionText;
        // GetImage((int)Images.NextSectionLock).gameObject.SetActive(isLock);
        GetButton((int)Buttons.NextSectionButton).gameObject.BindEvent(() =>
        {

        });

        LocalizeAfterSetInfo();
    }

    void LocalizeAfterSetInfo()
    {
        // Locale currentLanguage = LocalizationSettings.SelectedLocale;
        // for (int i = (int)Texts.TitleText; i < System.Enum.GetValues(typeof(Texts)).Length; i++)
        // {
        //     GetText(i).text = LocalizationSettings.StringDatabase.GetLocalizedString("SectionTable", _data.Id + System.Enum.GetName(typeof(Texts), i), currentLanguage);
        // }
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
