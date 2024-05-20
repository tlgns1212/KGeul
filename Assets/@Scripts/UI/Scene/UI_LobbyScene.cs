using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_LobbyScene : UI_Scene
{
    #region Enum

    enum Images
    {
        LearnImage,
        KgeulImage,
        ChartImage,
        ProfileImage,
        HeartImage,
        MoneyImage,
    }

    enum Toggles
    {
        LearnToggle,
        KgeulToggle,
        ChartToggle,
        ProfileToggle,
    }

    enum Buttons
    {
        PremiumButton,
        MoreButton,
        SettingButton,
        HeartRefillButton,
        HeartInfiniteButton,
        HeartPracticeButton
    }

    enum GameObjects
    {
        MenuToggleGroup,
        CheckLearnImageObject,
        CheckKgeulImageObject,
        CheckChartImageObject,
        CheckProfileImageObject,
        HeartGroup,
        MoneyGroup,
        HeartItemGroup,
        HeartItemBackground
    }

    enum Texts
    {
        HeartLeftText,
        HeartRefillText,
        HeartInfiniteText,
        HeartPracticeText,
        HeartText,
        MoneyText,
        HeartLeftNumTimeText
    }

    #endregion

    UI_LearnPopup _learnPopupUI;
    bool _isSelectedLearn = false;
    UI_KgeulPopup _kgeulPopupUI;
    bool _isSelectedKgeul = false;
    UI_ChartPopup _chartPopupUI;
    bool _isSelectedChart = false;
    UI_ProfilePopup _profilePopupUI;
    bool _isSelectedProfile = false;
    UI_CurriculumPopup _curriculumPopupUI;
    bool _isSelectedCurriculum = false;
    UI_MembershipPopup _membershipPopupUI;
    UI_SettingPopup _settingPopupUI;
    UI_StorePopup _storePopupUI;
    bool _isSelectedStore = false;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        gameObject.GetOrAddComponent<Canvas>().sortingOrder = 14;
        Application.targetFrameRate = 60;

        BindImage(typeof(Images));
        BindToggle(typeof(Toggles));
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));

        GetToggle((int)Toggles.LearnToggle).gameObject.BindEvent(OnClickLearnButton);
        GetToggle((int)Toggles.KgeulToggle).gameObject.BindEvent(OnClickKgeulButton);
        GetToggle((int)Toggles.ChartToggle).gameObject.BindEvent(OnClickChartButton);
        GetToggle((int)Toggles.ProfileToggle).gameObject.BindEvent(OnClickProfileButton);
        GetObject((int)GameObjects.HeartGroup).BindEvent(OnClickHeartObject);
        GetObject((int)GameObjects.MoneyGroup).BindEvent(OnClickMoneyObject);
        GetObject((int)GameObjects.HeartItemBackground).BindEvent(OnClickHeartItemBackground);
        GetObject((int)GameObjects.HeartItemGroup).BindEvent(OnClickHeartItemGroup);

        Managers.Game.LobbyScene = this;

        _learnPopupUI = Managers.UI.ShowPopupUI<UI_LearnPopup>();
        _learnPopupUI.SetInfo();
        _kgeulPopupUI = Managers.UI.ShowPopupUI<UI_KgeulPopup>();
        _chartPopupUI = Managers.UI.ShowPopupUI<UI_ChartPopup>();
        _profilePopupUI = Managers.UI.ShowPopupUI<UI_ProfilePopup>();
        _curriculumPopupUI = Managers.UI.ShowPopupUI<UI_CurriculumPopup>();
        _membershipPopupUI = Managers.UI.ShowPopupUI<UI_MembershipPopup>();
        _settingPopupUI = Managers.UI.ShowPopupUI<UI_SettingPopup>();
        _storePopupUI = Managers.UI.ShowPopupUI<UI_StorePopup>();

        _profilePopupUI.PopupEnd += HandleProfilePopupEndEvent;

        GetButton((int)Buttons.MoreButton).gameObject.BindEvent(OnClickMoreButton);
        GetButton((int)Buttons.SettingButton).gameObject.BindEvent(OnClickSettingButton);
        GetButton((int)Buttons.PremiumButton).gameObject.BindEvent(OnClickPremiumButton);


        TogglesInit();
        ButtonsInit();
        PopupInit();
        GetToggle((int)Toggles.LearnToggle).isOn = true;
        OnClickLearnButton();
        OnClickLearnButton();
        Refresh();

        return true;
    }

    void Refresh()
    {
        // 등등 Refresh 해야하는 데이터 


        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.MenuToggleGroup).GetComponent<RectTransform>());
    }

    void PopupInit()
    {
        _membershipPopupUI.SetInfo();
        _membershipPopupUI.gameObject.SetActive(false);
        _settingPopupUI.SetInfo();
        _settingPopupUI.gameObject.SetActive(false);
        _storePopupUI.SetInfo();
        _storePopupUI.gameObject.SetActive(false);
        GetObject((int)GameObjects.HeartItemBackground).GetOrAddComponent<RectTransform>().anchoredPosition = new Vector2(0, 1000);
        GetObject((int)GameObjects.HeartItemGroup).SetActive(false);
        _isSelectedStore = false;
    }

    void ButtonsInit()
    {
        _curriculumPopupUI.gameObject.SetActive(false);
        _isSelectedCurriculum = false;
    }

    void TogglesInit()
    {
        #region 팝업 초기화
        _learnPopupUI.gameObject.SetActive(false);
        _kgeulPopupUI.gameObject.SetActive(false);
        _chartPopupUI.gameObject.SetActive(false);
        _profilePopupUI.gameObject.SetActive(false);
        #endregion

        #region 토글 버튼 초기화
        // 토글 관련된거 초기화
        _isSelectedLearn = false;
        _isSelectedKgeul = false;
        _isSelectedChart = false;
        _isSelectedProfile = false;

        GetObject((int)GameObjects.CheckLearnImageObject).SetActive(false);
        GetObject((int)GameObjects.CheckKgeulImageObject).SetActive(false);
        GetObject((int)GameObjects.CheckChartImageObject).SetActive(false);
        GetObject((int)GameObjects.CheckProfileImageObject).SetActive(false);

        GetObject((int)GameObjects.CheckLearnImageObject).GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        GetObject((int)GameObjects.CheckKgeulImageObject).GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        GetObject((int)GameObjects.CheckChartImageObject).GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        GetObject((int)GameObjects.CheckProfileImageObject).GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

        GetImage((int)Images.LearnImage).gameObject.SetActive(true);
        GetImage((int)Images.KgeulImage).gameObject.SetActive(true);
        GetImage((int)Images.ChartImage).gameObject.SetActive(true);
        GetImage((int)Images.ProfileImage).gameObject.SetActive(true);

        #endregion
    }

    void ShowUI(GameObject contentPopup, Toggle toggle, GameObject checkImage, GameObject disableImage, float duration = 0.1f)
    {
        TogglesInit();

        contentPopup.SetActive(true);
        disableImage.SetActive(false);
        toggle.isOn = true;
        checkImage.SetActive(true);
        checkImage.GetComponent<RectTransform>().DOScale(new Vector3(1.1f, 1.1f, 1.1f), duration).SetEase(Ease.InOutQuad);

        Refresh();
    }

    public void OnClickLearnButton()
    {

        if (_isSelectedLearn == true)
        {
            StartCoroutine(_learnPopupUI.ContentGoToCurrentUnit());
            return;
        }

        ShowUI(_learnPopupUI.gameObject, GetToggle((int)Toggles.LearnToggle), GetObject((int)GameObjects.CheckLearnImageObject), GetImage((int)Images.LearnImage).gameObject);

        _isSelectedLearn = true;
    }

    public void OnClickKgeulButton()
    {
        if (_isSelectedKgeul == true)
            return;

        ShowUI(_kgeulPopupUI.gameObject, GetToggle((int)Toggles.KgeulToggle), GetObject((int)GameObjects.CheckKgeulImageObject), GetImage((int)Images.KgeulImage).gameObject);
        _isSelectedKgeul = true;
    }

    public void OnClickChartButton()
    {
        if (_isSelectedChart == true)
            return;

        ShowUI(_chartPopupUI.gameObject, GetToggle((int)Toggles.ChartToggle), GetObject((int)GameObjects.CheckChartImageObject), GetImage((int)Images.ChartImage).gameObject);
        _isSelectedChart = true;
    }

    public void OnClickProfileButton()
    {
        if (_isSelectedProfile == true)
            return;

        ShowUI(_profilePopupUI.gameObject, GetToggle((int)Toggles.ProfileToggle), GetObject((int)GameObjects.CheckProfileImageObject), GetImage((int)Images.ProfileImage).gameObject);
        _isSelectedProfile = true;
    }

    void HandleProfilePopupEndEvent()
    {
        OnClickLearnButton();
    }

    void OnClickMoreButton()
    {
        if (_isSelectedCurriculum)
            return;
        _curriculumPopupUI.gameObject.SetActive(true);
    }

    void OnClickPremiumButton()
    {
        _membershipPopupUI.gameObject.SetActive(true);
    }

    void OnClickSettingButton()
    {
        _settingPopupUI.gameObject.SetActive(true);
    }

    void OnClickHeartObject()
    {
        if (_isSelectedStore)
        {
            OnClickHeartItemGroup();
        }
        else
        {
            GetObject((int)GameObjects.HeartItemGroup).SetActive(true);
            GetObject((int)GameObjects.HeartItemBackground).GetOrAddComponent<RectTransform>().DOAnchorPos3DY(0, 0.3f).OnComplete(() => { _isSelectedStore = true; });
        }

    }

    void OnClickMoneyObject()
    {
        _storePopupUI.OpenStorePopup();
    }

    void OnClickHeartItemBackground()
    {
    }

    void OnClickHeartItemGroup()
    {
        GetObject((int)GameObjects.HeartItemBackground).GetOrAddComponent<RectTransform>().DOAnchorPos3DY(1000, 0.3f).OnComplete(() => { GetObject((int)GameObjects.HeartItemGroup).SetActive(false); _isSelectedStore = false; });
    }

    private void OnDestroy()
    {
        _profilePopupUI.PopupEnd -= HandleProfilePopupEndEvent;
    }
}
