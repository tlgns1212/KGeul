using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class UI_VowelPrinciplePopup : UI_Popup
{
    enum Texts
    {
        VowelTitleText,
        FinishText,
        VowelFirstText,
        VowelSecondText
    }

    enum Buttons
    {
        FirstButton,
        SecondButton,
        ExitButton,
        FinishButton,
    }

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(HandleExitButtonClick);
        GetButton((int)Buttons.FinishButton).gameObject.BindEvent(HandleFinishButtonClick);
        GetButton((int)Buttons.FirstButton).gameObject.BindEvent(HandleFirstButtonClick);
        GetButton((int)Buttons.SecondButton).gameObject.BindEvent(HandleSecondButtonClick);


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

    void HandleExitButtonClick()
    {
        Managers.UI.ClosePopupUI(this);
    }

    void HandleFinishButtonClick()
    {
        Managers.UI.ClosePopupUI(this);
    }

    void HandleFirstButtonClick()
    {
        Managers.UI.ShowPopupUI<UI_VowelOnePopup>();
    }

    void HandleSecondButtonClick()
    {
        Managers.UI.ShowPopupUI<UI_VowelTwoPopup>();
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
