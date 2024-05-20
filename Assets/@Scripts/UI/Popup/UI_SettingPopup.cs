using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class UI_SettingPopup : UI_Popup
{
    enum Texts
    {
        SettingTitleText,
        FinishText,
        LogoutText
    }

    enum Buttons
    {
        FinishButton,
        ExitButton,
        LogoutButton,
    }

    enum Images
    {
        TestImage,
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
        BindImage(typeof(Images));

        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnClickExitButton);
        GetButton((int)Buttons.FinishButton).gameObject.BindEvent(OnClickFinishButton);
        GetButton((int)Buttons.LogoutButton).gameObject.BindEvent(OnClickLogoutButton);


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

    void OnClickExitButton()
    {
        gameObject.SetActive(false);
    }

    void OnClickFinishButton()
    {
        gameObject.SetActive(false);
    }

    void OnClickLogoutButton()
    {
        PlayerPrefs.DeleteKey("token");
        Managers.Game.DeleteGame();
        Managers.Scene.LoadScene(Define.Scene.TitleScene, transform);
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
