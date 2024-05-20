using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FlowItem : UI_FlowBase
{

    enum Images
    {
        AnswerImage,
        Guess1Image,
        Guess2Image,
        Guess3Image,
    }

    enum Toggles
    {
        Guess1Toggle,
        Guess2Toggle,
        Guess3Toggle,
    }

    Action<int> _action;

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindToggle(typeof(Toggles));
        BindImage(typeof(Images));

        GetToggle((int)Toggles.Guess1Toggle).gameObject.BindEvent(HandleGuess1ToggleClicked);
        GetToggle((int)Toggles.Guess2Toggle).gameObject.BindEvent(HandleGuess2ToggleClicked);
        GetToggle((int)Toggles.Guess3Toggle).gameObject.BindEvent(HandleGuess3ToggleClicked);

        return true;
    }

    public override void SetInfo(Data.FlowItemData data, Action<int> callback)
    {
        _data = data;
        _answerId = GetTextItem(data.Answers[0]);
        foreach (int question in data.Questions)
        {
            _questionIds.Add(GetTextItem(question));
        }

        int answerCount = data.Answers.Count;
        for (int i = 0; i < answerCount; i++)
        {
            ChangeSprite(i, data.Answers[i], Define.UIType.Text);
        }
        int questionCount = data.Questions.Count;
        for (int i = 0; i < questionCount; i++)
        {
            ChangeSprite(answerCount + i, data.Questions[i], Define.UIType.Text);
        }

        _action = callback;
    }

    void HandleGuess1ToggleClicked()
    {
        _action?.Invoke(_data.Questions[0]);
    }

    void HandleGuess2ToggleClicked()
    {
        _action?.Invoke(_data.Questions[1]);
    }

    void HandleGuess3ToggleClicked()
    {
        _action?.Invoke(_data.Questions[2]);
    }
}
