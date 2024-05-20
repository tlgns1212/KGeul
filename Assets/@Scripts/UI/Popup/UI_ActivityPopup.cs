using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class UI_ActivityPopup : UI_Popup
{
    enum Texts
    {
        FinishText,
        CheckText,
        ContinueText,
        RightDesText,
        WrongText,
        RightText,
        ConfirmText,
        HeartNumText,
        LessonAnswerText,
    }

    enum Buttons
    {
        CheckButton,
        ContinueButton,
        FinishButton,
        ExitButton,
        RightContinueButton,
        WrongConfirmButton,
    }

    enum GameObjects
    {
        FlowGroup,
        ProgressBar,
        RightGroup,
        WrongGroup,
        NoInteractiveImage
    }

    private int _currentFlow = 0;
    private int _totalFlow;
    private int _inputFlowAnswerId;
    private Data.ActivityItemData _data;
    private List<UI_FlowBase> _flowItems = new List<UI_FlowBase>();
    private List<int> _wrongFlows = new List<int>();

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
        BindObject(typeof(GameObjects));

        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnClickExitButton);
        GetButton((int)Buttons.ContinueButton).gameObject.BindEvent(OnClickContinueButton);
        GetButton((int)Buttons.RightContinueButton).gameObject.BindEvent(OnClickRightContinueButton);
        GetButton((int)Buttons.WrongConfirmButton).gameObject.BindEvent(OnClickWrongConfirmButton);

        // Refresh();

        return true;
    }

    public void SetInfo(Data.ActivityItemData data, Action OnClickFinish)
    {
        _data = data;
        _currentFlow = 0;
        _totalFlow = data.FlowIds.Count;

        UI_EventHandler ev = GetButton((int)Buttons.FinishButton).gameObject.GetOrAddComponent<UI_EventHandler>();
        ev.OnClickHandler = OnClickFinish;

        foreach (int flowId in data.FlowIds)
        {
            print(flowId);
            if (Managers.Data.FlowItemDataDic.TryGetValue(flowId, out Data.FlowItemData flowData))
            {
                print(flowId);
                MakeFlowItem(flowData);
            }
        }
        Refresh();
    }

    void Refresh()
    {
        foreach (UI_FlowBase flowItem in _flowItems)
        {
            flowItem.gameObject.SetActive(false);
        }
        _flowItems[_currentFlow].gameObject.SetActive(true);
        GetButton((int)Buttons.FinishButton).gameObject.SetActive(false);
        GetButton((int)Buttons.ContinueButton).gameObject.SetActive(false);
        GetObject((int)GameObjects.RightGroup).gameObject.SetActive(false);
        GetObject((int)GameObjects.WrongGroup).gameObject.SetActive(false);
        GetObject((int)GameObjects.NoInteractiveImage).SetActive(false);
    }

    void MakeFlowItem(Data.FlowItemData flowData)
    {

        switch (flowData.TypeId)
        {
            case 1:
                UI_FlowItem flowItem = Managers.UI.MakeSubItem<UI_FlowItem>(GetObject((int)GameObjects.FlowGroup).transform);
                _flowItems.Add(flowItem);
                break;
            case 2:
                UI_FlowItem1 flowItem1 = Managers.UI.MakeSubItem<UI_FlowItem1>(GetObject((int)GameObjects.FlowGroup).transform);
                _flowItems.Add(flowItem1);
                break;
            case 3:
                break;
            default:
                break;
        }
        // UI_FlowItem flowItem = Managers.UI.MakeSubItem<UI_FlowItem>(GetObject((int)GameObjects.FlowGroup).transform);
        int idx = _flowItems.Count - 1;
        _flowItems[idx].SetInfo(flowData, (value) =>
        {
            _inputFlowAnswerId = value;
            GetButton((int)Buttons.ContinueButton).gameObject.SetActive(true);
        });

        RectTransform rectT = _flowItems[idx].GetOrAddComponent<RectTransform>();
        rectT.offsetMin = Vector2.zero;
        rectT.offsetMax = Vector2.zero;

        _flowItems[idx].gameObject.SetActive(false);
    }

    void OnClickExitButton()
    {
        gameObject.SetActive(false);
    }

    void OnClickContinueButton()
    {
        GetObject((int)GameObjects.NoInteractiveImage).SetActive(true);
        _flowItems[_currentFlow].ContinueClicked();
        if (_flowItems[_currentFlow].GetAnswer() == _inputFlowAnswerId)
        {
            GetObject((int)GameObjects.RightGroup).gameObject.SetActive(true);
        }
        else
        {
            GetObject((int)GameObjects.WrongGroup).gameObject.SetActive(true);
            GetText((int)Texts.LessonAnswerText).text = LocalizationSettings.StringDatabase.GetLocalizedString("MyTable", "RightText", LocalizationSettings.SelectedLocale);
            _wrongFlows.Add(_currentFlow);
        }
        IncreaseCurrentFlow();
    }

    void OnClickRightContinueButton()
    {
        if (_currentFlow == _totalFlow)
        {
            GetButton((int)Buttons.FinishButton).gameObject.SetActive(true);
            GetObject((int)GameObjects.RightGroup).gameObject.SetActive(false);
        }
        else
        {
            Refresh();
        }
    }

    void OnClickWrongConfirmButton()
    {
        if (_currentFlow == _totalFlow)
        {
            GetButton((int)Buttons.FinishButton).gameObject.SetActive(true);
            GetObject((int)GameObjects.WrongGroup).gameObject.SetActive(false);
        }
        else
        {
            Refresh();
        }
    }

    void IncreaseCurrentFlow()
    {
        _currentFlow++;
        GetObject((int)GameObjects.ProgressBar).GetOrAddComponent<Slider>().value = (float)_currentFlow / _totalFlow;
    }

    protected override void LocalizeAllTexts()
    {
        Locale currentLanguage = LocalizationSettings.SelectedLocale;
        for (int i = 0; i < (int)Texts.ConfirmText; i++)
        {
            GetText(i).text = LocalizationSettings.StringDatabase.GetLocalizedString("MyTable", Enum.GetName(typeof(Texts), i), currentLanguage);
        }
    }
}
