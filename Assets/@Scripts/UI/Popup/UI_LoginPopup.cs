using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UI_LoginPopup : UI_Popup
{

    enum GameObjects
    {
        Guide1,
        Guide2,
        Guide3,
        EndInteractiveImage,
        LoginInputBackground,
        RegisterInputBackground
    }

    enum Buttons
    {
        Guide1ContinueButton,
        Guide2LoginButton,
        Guide2SkipButton,
        Guide2RegisterButton,
        Guide3Button1,
        Guide3Button2,
        Guide3Button3,
        LoginSubmitButton,
        RegisterSubmitButton,
        LoginBackButton,
        RegisterBackButton
    }

    enum Toggles
    {
        Guide1Toggle1,
        Guide1Toggle2,
        Guide1Toggle3,
        Guide2Toggle1,
        Guide2Toggle2,
        Guide2Toggle3,
    }

    enum Texts
    {
        Guide1Text1,
        Guide1Toggle1Text,
        Guide1Toggle2Text,
        Guide1Toggle3Text,
        ContinueText,
        Guide2Text1,
        Guide2Toggle1Text,
        Guide2Toggle2Text,
        Guide2Toggle3Text,
        Guide2Text2,
        LoginText,

        RegisterText,
        Guide3Text1,
        Guide3Button1Text,
        Guide3Button2Text,
        Guide3Button3Text,

        SkipText,
        IDText,
        InputIDPlaceholder,
        PWText,
        InputPWPlaceholder,
        LanguageText,
        EmailText,
        InputEmailPlaceholder,
    }

    enum InputTexts
    {
        LoginInputID,
        LoginInputPW,
        RegisterInputID,
        RegisterInputPW,
        RegisterInputLanguage,
        RegisterInputEmail,
    }

    int _selectedIndex;
    int _startIndex = (int)GameObjects.Guide1;
    int _lastIndex = (int)GameObjects.Guide3;
    bool _shouldGoToFirst = false;

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindInputText(typeof(InputTexts));
        BindToggle(typeof(Toggles));

        _selectedIndex = _startIndex;

        GetObject((int)GameObjects.LoginInputBackground).SetActive(false);
        GetObject((int)GameObjects.RegisterInputBackground).SetActive(false);
        GetObject((int)GameObjects.EndInteractiveImage).SetActive(false);
        GetButton((int)Buttons.Guide1ContinueButton).gameObject.BindEvent(SaveLocalization);
        GetButton((int)Buttons.Guide2SkipButton).gameObject.BindEvent(GetNextGuide);
        GetButton((int)Buttons.Guide2RegisterButton).gameObject.BindEvent(HandleRegisterButton);
        GetButton((int)Buttons.Guide2LoginButton).gameObject.BindEvent(HandleLoginButton);
        GetButton((int)Buttons.Guide3Button1).gameObject.BindEvent(GetNextGuide);
        GetButton((int)Buttons.Guide3Button2).gameObject.BindEvent(GetNextGuide);
        GetButton((int)Buttons.Guide3Button3).gameObject.BindEvent(GetNextGuide);
        GetButton((int)Buttons.LoginSubmitButton).gameObject.BindEvent(HandleLoginSubmitButton);
        GetButton((int)Buttons.RegisterSubmitButton).gameObject.BindEvent(HandleRegisterSubmitButton);
        GetButton((int)Buttons.LoginBackButton).gameObject.BindEvent(HandleLoginBackButtonPressed);
        GetButton((int)Buttons.RegisterBackButton).gameObject.BindEvent(HandleRegisterBackButtonPressed);

        GetToggle((int)Toggles.Guide1Toggle1).onValueChanged.AddListener(delegate { ToggleLocalizationChanged(1); });
        GetToggle((int)Toggles.Guide1Toggle2).onValueChanged.AddListener(delegate { ToggleLocalizationChanged(0); });
        GetToggle((int)Toggles.Guide1Toggle3).onValueChanged.AddListener(delegate { ToggleLocalizationChanged(2); });

        Refresh();

        return true;
    }

    public void SetInfo(int selectIndex)
    {
        _selectedIndex = selectIndex;
        Refresh();
    }

    void Refresh()
    {
        GetObject((int)GameObjects.Guide1).SetActive(false);
        GetObject((int)GameObjects.Guide2).SetActive(false);
        GetObject((int)GameObjects.Guide3).SetActive(false);

        GetObject(_selectedIndex).SetActive(true);
    }

    void GetNextGuide()
    {
        if (_init == false)
            return;


        if (_selectedIndex + 1 > _lastIndex)
        {
            if (_shouldGoToFirst)
                Managers.UI.ShowPopupUI<UI_FirstPopup>();
            else
            {
                GetObject((int)GameObjects.EndInteractiveImage).SetActive(true);
                Managers.Scene.LoadScene(Define.Scene.LobbyScene, transform);
            }
            // gameObject.SetActive(false);
        }

        GetObject((int)GameObjects.Guide1).SetActive(false);
        GetObject((int)GameObjects.Guide2).SetActive(false);
        GetObject((int)GameObjects.Guide3).SetActive(false);

        _selectedIndex = Mathf.Min(_selectedIndex + 1, _lastIndex);
        GetObject(_selectedIndex).SetActive(true);
    }

    void HandleLoginBackButtonPressed()
    {
        GetObject((int)GameObjects.LoginInputBackground).SetActive(false);
    }
    void HandleRegisterBackButtonPressed()
    {
        GetObject((int)GameObjects.RegisterInputBackground).SetActive(false);
    }

    void SaveLocalization()
    {
        Managers.Game.SaveGame();
        GetNextGuide();
    }

    void HandleLoginButton()
    {
        GetObject((int)GameObjects.LoginInputBackground).SetActive(true);
    }

    void HandleRegisterButton()
    {
        GetObject((int)GameObjects.RegisterInputBackground).SetActive(true);
    }

    void ToggleLocalizationChanged(int i)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[i];
        LocalizeAllTexts();
        Managers.Game.LanguageNum = i;
    }

    protected override void LocalizeAllTexts()
    {
        Locale currentLanguage = LocalizationSettings.SelectedLocale;
        for (int i = 0; i < System.Enum.GetValues(typeof(Texts)).Length; i++)
        {
            GetText(i).text = LocalizationSettings.StringDatabase.GetLocalizedString("MyTable", Enum.GetName(typeof(Texts), i), currentLanguage);
        }
    }

    void HandleLoginSubmitButton()
    {
        string id = GetInputText((int)InputTexts.LoginInputID).text;
        string pw = GetInputText((int)InputTexts.LoginInputPW).text;

        ProtocolPostLogin login = new ProtocolPostLogin() { UserName = id, Password = pw };
        StartCoroutine(PostJsonData(Define.PostType.login.ToString(), ConvertToJson(login), SuccessLogin, FailLogin));
    }

    void HandleRegisterSubmitButton()
    {
        string id = GetInputText((int)InputTexts.RegisterInputID).text;
        string pw = GetInputText((int)InputTexts.RegisterInputPW).text;
        int lan = int.Parse(GetInputText((int)InputTexts.RegisterInputLanguage).text);
        string em = GetInputText((int)InputTexts.RegisterInputEmail).text;

        ProtocolPostRegister register = new ProtocolPostRegister() { UserName = id, Password = pw, Email = em, LanguageNumber = lan };

        StartCoroutine(PostJsonData(Define.PostType.register.ToString(), ConvertToJson(register), SuccessRegister, FailRegister));
    }

    void SuccessLogin(string jsonStr)
    {
        print("LOOOGGGIIINNN");
        ProtocolGetLogin data = JsonConvert.DeserializeObject<ProtocolGetLogin>(jsonStr);
        print(jsonStr);
        print("LOOOGGGIIINNN");
        Managers.Game.PlayerLogin = data;
        ToggleLocalizationChanged(data.LanguageNumber - 1);
        Managers.Game.SaveGame();
        Managers.Game.SaveGameToPlayer();

        _shouldGoToFirst = false;
        GetObject((int)GameObjects.EndInteractiveImage).SetActive(true);
        Managers.Scene.LoadScene(Define.Scene.LobbyScene, transform);
    }

    void FailLogin()
    {
        print("로그인 실패");
        // GetObject((int)GameObjects.RegisterInputBackground).SetActive(true);
    }

    void SuccessRegister(string jsonStr)
    {
        ProtocolGetLogin data = JsonConvert.DeserializeObject<ProtocolGetLogin>(jsonStr);
        Managers.Game.PlayerLogin = data;
        ToggleLocalizationChanged(data.LanguageNumber - 1);
        Managers.Game.SaveGame();
        Managers.Game.SaveGameToPlayer();

        _shouldGoToFirst = true;
        GetNextGuide();
    }

    void FailRegister()
    {
        print("회원가입 실패");
    }
}
