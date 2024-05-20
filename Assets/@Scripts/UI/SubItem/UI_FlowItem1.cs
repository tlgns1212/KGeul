using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_FlowItem1 : UI_FlowBase
{

    enum Images
    {
        AnswerImage1,
    }

    enum Buttons
    {
        QuestionButton1,
        QuestionButton2,
        QuestionButton3,
    }

    Action<int> _action;
    Sequence _sequence;
    int _answerinImagesInt = 0;
    Coroutine _blinkCoroutine;

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.QuestionButton1).gameObject.BindEvent(HandleQuestionButton1Clicked);
        GetButton((int)Buttons.QuestionButton2).gameObject.BindEvent(HandleQuestionButton2Clicked);
        GetButton((int)Buttons.QuestionButton3).gameObject.BindEvent(HandleQuestionButton3Clicked);

        return true;
    }

    public override void SetInfo(Data.FlowItemData data, Action<int> callback)
    {
        _data = data;
        _action = callback;

        _answerId = GetTextItem(data.Answers[0]);

        foreach (int question in data.Questions)
        {
            _questionIds.Add(GetTextItem(question));
        }

        for (int i = 0; i < data.Answers.Count; i++)
        {
            ChangeSprite(i, data.Answers[i], Define.UIType.Text);
        }
        for (int i = 0; i < data.Questions.Count; i++)
        {
            ChangeSprite(i, data.Questions[i], Define.UIType.Button);
        }
        FindAnswerInImages();
    }

    void FindAnswerInImages()
    {
        for (int i = 0; i < _questionIds.Count; i++)
        {
            if (_questionIds[i].AText == _answerId.AText)
            {
                _answerinImagesInt += i;
            }
        }
        return;
    }

    private void OnEnable()
    {
        _blinkCoroutine = StartCoroutine(CoTurnOnAndOff());
    }

    IEnumerator CoTurnOnAndOff()
    {
        yield return new WaitForSeconds(5f);

        _sequence = DOTween.Sequence().SetAutoKill(false);
        _sequence.Append(GetButton(_answerinImagesInt).image.DOFade(0.0f, 1).SetLoops(int.MaxValue, LoopType.Restart));
    }

    void StopSequence(bool shouldRepeat = true)
    {
        if (_sequence == null)
            return;

        _sequence.Kill(true);
        if (_blinkCoroutine != null)
            StopCoroutine(_blinkCoroutine);
        if (shouldRepeat)
            _blinkCoroutine = StartCoroutine(CoTurnOnAndOff());
    }


    void HandleQuestionButton1Clicked()
    {
        StopSequence();
        print(_questionIds[0].SText);
        int aText = 0;
        if (Int32.TryParse(_questionIds[0].AText, out int result))
        {
            aText = result;
        }
        _action?.Invoke(aText);
    }

    void HandleQuestionButton2Clicked()
    {
        StopSequence();
        print(_questionIds[1].SText);
        int aText = 0;
        if (Int32.TryParse(_questionIds[1].AText, out int result))
        {
            aText = result;
        }
        _action?.Invoke(aText);
    }

    void HandleQuestionButton3Clicked()
    {
        StopSequence();
        print(_questionIds[2].SText);
        int aText = 0;
        if (Int32.TryParse(_questionIds[2].AText, out int result))
        {
            aText = result;
        }
        _action?.Invoke(aText);
    }

    public override void ContinueClicked()
    {
        StopSequence(false);
    }
}
