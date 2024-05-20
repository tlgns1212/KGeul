using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class UI_ChartPopup : UI_Popup
{
    enum Texts
    {
        ChartTitleText,
        ChartTopFirstText,
        ChartTopSecondText,
        ChartBottomFirstText,
        ChartBottomSecondText,
        ChartBottomThirdText,
    }

    enum Buttons
    {
        TopFirstButton,
        TopSecondButton,
        BottomFirstButton,
        BottomSecondButton,
        BottomThirdButton,
    }

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetButton((int)Buttons.TopFirstButton).gameObject.BindEvent(HandleTopFirstButtonClick);
        GetButton((int)Buttons.TopSecondButton).gameObject.BindEvent(HandleTopSecondButtonClick);
        GetButton((int)Buttons.BottomFirstButton).gameObject.BindEvent(HandleBottomFirstButtonClick);
        GetButton((int)Buttons.BottomSecondButton).gameObject.BindEvent(HandleBottomSecondButtonClick);
        GetButton((int)Buttons.BottomThirdButton).gameObject.BindEvent(HandleBottomThirdButtonClick);

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

    void HandleTopFirstButtonClick()
    {
        Managers.UI.ShowPopupUI<UI_VowelTwoPopup>();
    }

    void HandleTopSecondButtonClick()
    {
        Managers.UI.ShowPopupUI<UI_ConsonantTwoPopup>();
    }

    void HandleBottomFirstButtonClick()
    {
        Managers.UI.ShowPopupUI<UI_ChartOnePopup>();
    }

    void HandleBottomSecondButtonClick()
    {
        Managers.UI.ShowPopupUI<UI_ChartTwoPopup>();
    }

    void HandleBottomThirdButtonClick()
    {
        Managers.UI.ShowPopupUI<UI_ChartThreePopup>();
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
