using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class UI_UnitGroup : UI_Base
{
    enum Texts
    {
        StartText
    }

    enum Images
    {
        UnitImage,
        CheckImage,
        DisableUnitImage,
        LockImage,
        StartImage
    }

    public Data.UnitItemData _data;
    public int _currentActivity;
    public int _totalActivity;

    private UI_LearnPopup _learnPopupUI;
    private bool _isLocked;
    public bool IsLocked
    {
        get { return _isLocked; }
        set
        {
            _isLocked = value;
            GetImage((int)Images.DisableUnitImage).gameObject.SetActive(_isLocked);
        }
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
        BindImage(typeof(Images));
        _learnPopupUI = FindObjectOfType<UI_LearnPopup>();

        return true;
    }

    public void SetInfo(Data.UnitItemData unitData)
    {
        _data = unitData;

        GetImage((int)Images.UnitImage).gameObject.SetActive(true);
        GetImage((int)Images.DisableUnitImage).gameObject.SetActive(false);

        GetImage((int)Images.UnitImage).sprite = Managers.Resource.Load<Sprite>(_data.ImageId);
        GetImage((int)Images.DisableUnitImage).sprite = Managers.Resource.Load<Sprite>(_data.DisableImageId);
        GetImage((int)Images.LockImage).sprite = Managers.Resource.Load<Sprite>(_data.LockImageId);
        GetImage((int)Images.StartImage).gameObject.SetActive(false);

        gameObject.BindEvent(HandleOnClickToggleItem);

        if (Managers.Game.CurrentUserState.UnitId >= _data.Id)
        {
            _data.IsLocked = false;
        }

        _totalActivity = _data.ActivityIds.Count;
        if (Managers.Game.CurrentUserState.UnitId > _data.Id)
        {
            _currentActivity = _data.ActivityIds.Count;
        }
        else if (Managers.Game.CurrentUserState.UnitId == _data.Id)
        {
            _currentActivity = _data.ActivityIds.IndexOf(Managers.Game.CurrentUserState.ActivityId) + 1;
        }
        else
        {
            _currentActivity = 1;
        }

        // if (Managers.Game.DicMission.TryGetValue(_data.Id, out MissionInfo mInfo))
        // {
        //     _data.IsLocked = mInfo.IsLocked;
        //     _data.IsRewarded = mInfo.IsRewarded;
        // }
        GetComponent<RectTransform>().anchoredPosition = new Vector3(_data.PosX, _data.PosY, 0);

        IsLocked = _data.IsLocked;

        LocalizeAfterSetInfo();
    }

    public bool SetToggles()
    {
        if (!IsLocked)
        {
            gameObject.GetOrAddComponent<Toggle>().isOn = true;
            Managers.Game.CurrentUnitObject = gameObject;
            return true;
        }
        else
        {
            return false;
        }
    }

    void SuccessUnit()
    {
        if (_currentActivity == _totalActivity)
        {
            if (_learnPopupUI._unitItems.TryGetValue(_data.NextId, out UI_UnitGroup unitItem))
            {
                unitItem.IsLocked = false;
                Managers.Game.CurrentUnitObject = unitItem.gameObject;
            }
        }
        else
        {
            _currentActivity += 1;
            Managers.Game.CurrentUnitObject = gameObject;
        }

        UI_UnitGroup curUnit = Managers.Game.CurrentUnitObject.GetOrAddComponent<UI_UnitGroup>();

        ProtocolPostActivityClear activity = new ProtocolPostActivityClear()
        {
            UserName = Managers.Game.UserName,
            SectionId = curUnit._data.Ppid,
            StepId = curUnit._data.Pid,
            UnitId = curUnit._data.Id,
            ActivityId = curUnit._data.ActivityIds[curUnit._currentActivity - 1]
        };
        Managers.Game.SaveGame();
        StartCoroutine(PostJsonData(Define.PostType.complete.ToString(), ConvertToJson(activity), HandleSuccessActivityClear, HandleFailActivityClear));

    }

    void HandleSuccessActivityClear(string jsonStr)
    {
        // GetImage((int)Images.DisableUnitImage).gameObject.SetActive(false);
    }

    void HandleFailActivityClear()
    {
        // 서버말고 그냥 개인 데이터에 저장
    }

    void HandleOnClickToggleItem()
    {
        UI_SelectToggle selectToggleUI = _learnPopupUI.SelectToggleUI;
        selectToggleUI.transform.DOScale(0f, 0.1f).SetEase(Ease.InOutBack);
        selectToggleUI._isHiding = true;
        Vector2 tempPos = gameObject.GetOrAddComponent<RectTransform>().position;
        StartCoroutine(_learnPopupUI.ContentGoToUnit(this,
        () =>
        {
            selectToggleUI.SetInfo(_data.Id, _currentActivity, _totalActivity, _isLocked, () =>
                    {
                        // Local에서 가져오는 방법
                        int activityId = _data.ActivityIds[_currentActivity - 1];
                        print(_data.Id + " " + activityId);
                        if (Managers.Data.ActivityItemDataDic.TryGetValue(activityId, out Data.ActivityItemData activityData))
                        {
                            print(_data.Id + " " + activityId);
                            UI_ActivityPopup activityPopupUI = Managers.UI.ShowPopupUI<UI_ActivityPopup>();
                            activityPopupUI.SetInfo(activityData, () =>
                            {
                                SuccessUnit();
                                Managers.UI.ClosePopupUI(activityPopupUI);
                            });
                        }

                        selectToggleUI._isHiding = true;
                        selectToggleUI.transform.DOScale(0f, 0.2f).SetEase(Ease.InOutBack);
                    });

            // 위치 이동 관련
            Vector2 pos = gameObject.GetOrAddComponent<RectTransform>().position;
            float yPos = GetYPosition(transform);
            if (selectToggleUI._isHiding)
            {
                if (yPos >= -800)
                {
                    selectToggleUI.gameObject.GetOrAddComponent<RectTransform>().position = new Vector2(pos.x, pos.y + (yPos * 1.4f) + 860);
                    selectToggleUI.transform.DOScale(1f, 0.2f).SetEase(Ease.InOutBack)
                    .OnComplete(() => { selectToggleUI._isHiding = false; });
                }
                else
                {
                    selectToggleUI.gameObject.GetOrAddComponent<RectTransform>().position = new Vector2(pos.x, pos.y - 250);
                    selectToggleUI.transform.DOScale(1f, 0.2f).SetEase(Ease.InOutBack)
                    .OnComplete(() => { selectToggleUI._isHiding = false; });
                }
            }
        }));
    }

    float GetYPosition(Transform unitGroupT)
    {
        float yPos = 0;
        yPos += unitGroupT.GetComponent<RectTransform>().anchoredPosition.y;
        yPos += unitGroupT.parent.parent.GetComponent<RectTransform>().anchoredPosition.y;
        return yPos;
    }

    void LocalizeAfterSetInfo()
    {
        Locale currentLanguage = LocalizationSettings.SelectedLocale;
        for (int i = 0; i < System.Enum.GetValues(typeof(Texts)).Length; i++)
        {
            GetText(i).text = LocalizationSettings.StringDatabase.GetLocalizedString("MyTable", System.Enum.GetName(typeof(Texts), i), currentLanguage);
        }
    }
}
