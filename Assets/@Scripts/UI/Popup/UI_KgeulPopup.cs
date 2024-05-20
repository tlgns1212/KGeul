using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class UI_KgeulPopup : UI_Popup
{
    enum Buttons
    {
        FirstButton,
        SecondButton,
        ThirdButton,
        FourthButton
    }

    enum Texts
    {
        KgeulTitleText,
        KgeulFirstText,
        KgeulSecondText,
        KgeulThirdText,
        KgeulFourthText,
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

        GetButton((int)Buttons.FirstButton).gameObject.BindEvent(HandleFirstButtonClick);
        GetButton((int)Buttons.SecondButton).gameObject.BindEvent(HandleSecondButtonClick);
        GetButton((int)Buttons.ThirdButton).gameObject.BindEvent(HandleThirdButtonClick);
        GetButton((int)Buttons.FourthButton).gameObject.BindEvent(HandleFourthButtonClick);

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

    void HandleFirstButtonClick()
    {
        Managers.Game.LobbyScene.OnClickChartButton();
    }

    void HandleSecondButtonClick()
    {
        Managers.UI.ShowPopupUI<UI_GreatHangeulPopup>();
    }

    void HandleThirdButtonClick()
    {
        Managers.UI.ShowPopupUI<UI_VowelPrinciplePopup>();
    }

    void HandleFourthButtonClick()
    {
        Managers.UI.ShowPopupUI<UI_ConsonantPrinciplePopup>();
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
