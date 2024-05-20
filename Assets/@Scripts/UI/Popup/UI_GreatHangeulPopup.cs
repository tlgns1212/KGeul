using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class UI_GreatHangeulPopup : UI_Popup
{
    enum Texts
    {
        GreatHangeulTitleText,
        FinishText
    }

    enum Buttons
    {
        ExitButton,
        FinishButton
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

    protected override void LocalizeAllTexts()
    {
        Locale currentLanguage = LocalizationSettings.SelectedLocale;
        for (int i = 0; i < System.Enum.GetValues(typeof(Texts)).Length; i++)
        {
            GetText(i).text = LocalizationSettings.StringDatabase.GetLocalizedString("MyTable", System.Enum.GetName(typeof(Texts), i), currentLanguage);
        }
    }
}
