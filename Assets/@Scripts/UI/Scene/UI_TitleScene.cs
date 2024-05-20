using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class UI_StartScene : UI_Scene
{
    #region Enum
    enum GameObjects
    {
        Slider,
    }

    enum Buttons
    {
        StartButton,
    }

    enum Texts
    {
        TouchStartText,
    }
    #endregion

    bool isPreload = false;
    bool isFirstOpen = true;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Application.targetFrameRate = 60;
        // 오브젝트 바인딩
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetObject((int)GameObjects.Slider).GetComponent<Slider>().value = 0;

        GetButton((int)Buttons.StartButton).gameObject.BindEvent(() =>
        {
            if (isPreload)
            {
                UI_LoginPopup loginUI = Managers.UI.ShowPopupUI<UI_LoginPopup>();
                if (!isFirstOpen)
                    loginUI.SetInfo(1);

                // Managers.Scene.LoadScene(Define.Scene.LobbyScene, transform);
                GetButton((int)Buttons.StartButton).gameObject.SetActive(false);
            }

        });

        GetButton((int)Buttons.StartButton).GetComponent<Button>().gameObject.SetActive(false);

        return true;
    }

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        Managers.Resource.LoadAllAsync<Object>("SihoonTest", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count}/{totalCount}");
        });
        Managers.Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count}/{totalCount}");
            GetObject((int)GameObjects.Slider).GetComponent<Slider>().value = (float)count / totalCount;
            if (count == totalCount)
            {
                isPreload = true;
                DOTween.SetTweensCapacity(500, 125);
                Managers.Data.Init();
                // persistentDataPath에 Player.json파일이 있으면 이미 한번 언어설정 한거니깐 바로 로그인창으로
                if (Managers.Game.LoadGame())
                    isFirstOpen = false;
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[Managers.Game.LanguageNum];
                // 토큰값이 있으면 바로 자동로그인, 없으면 Start Text 보이기
                if (PlayerPrefs.HasKey("token"))
                {
                    StartCoroutine(PostJsonData(Define.PostType.autologin.ToString(), "", SuccessAutoLogin, FailAutoLogin));
                }
                else
                {
                    GetButton((int)Buttons.StartButton).gameObject.SetActive(true);
                    StartTextAnimation();
                }

                // Managers.Scene.LoadScene(Define.Scene.LobbyScene, transform);
            }
        });
    }

    void SuccessAutoLogin(string jsonStr)
    {
        ProtocolGetLogin data = JsonConvert.DeserializeObject<ProtocolGetLogin>(jsonStr);
        Managers.Game.PlayerLogin = data;
        Managers.Game.SaveGame();

        Managers.Scene.LoadScene(Define.Scene.LobbyScene, transform);
    }

    void FailAutoLogin()
    {
        GetButton((int)Buttons.StartButton).gameObject.SetActive(true);
        StartTextAnimation();
    }

    void StartTextAnimation()
    {
        // Locale currentLanguage = LocalizationSettings.SelectedLocale;
        // for (int i = 0; i < System.Enum.GetValues(typeof(Texts)).Length; i++)
        // {
        //     GetText(i).text = LocalizationSettings.StringDatabase.GetLocalizedString("MyTable", System.Enum.GetName(typeof(Texts), i), currentLanguage);
        // }
        GetText((int)Texts.TouchStartText).DOFade(0, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutCubic).Play();
    }
}
