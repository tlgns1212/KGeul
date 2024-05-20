using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class UI_FirstPopup : UI_Popup
{

    enum GameObjects
    {
        Intro1,
        Intro2,
        Intro3,
    }

    enum Texts
    {
        Intro1Text1,
        Intro1Text2,
        Intro2Text1,
        Intro2Text2,
        Intro3Text1,
        Intro3Text2,
        StartText,
    }

    enum Images
    {
        Intro1Image,
        Intro2Image,
        Intro3Image,
    }

    enum Buttons
    {
        Intro1,
        Intro2,
        Button
    }

    int _selectedIndex;
    int _startIndex = (int)GameObjects.Intro1;
    int _lastIndex = (int)GameObjects.Intro3;

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
        BindImage(typeof(Images));
        BindText(typeof(Texts));

        Refresh();

        _selectedIndex = _startIndex - 1;
        GoToNextIntro();

        GetButton((int)Buttons.Intro1).gameObject.BindEvent(GoToNextIntro);
        GetButton((int)Buttons.Intro2).gameObject.BindEvent(GoToNextIntro);
        GetButton((int)Buttons.Button).gameObject.BindEvent(GoToNextIntro);

        return true;
    }

    public void SetInfo()
    {
        Refresh();
    }


    void Refresh()
    {

    }

    void GoToNextIntro()
    {
        if (_selectedIndex + 1 > _lastIndex)
        {
            Managers.Scene.LoadScene(Define.Scene.LobbyScene, transform);
            return;
        }

        GetObject((int)GameObjects.Intro1).SetActive(false);
        GetObject((int)GameObjects.Intro2).SetActive(false);
        GetObject((int)GameObjects.Intro3).SetActive(false);

        _selectedIndex += 1;
        GetObject(_selectedIndex).SetActive(true);
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
