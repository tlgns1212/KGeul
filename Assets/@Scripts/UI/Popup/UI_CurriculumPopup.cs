using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UI_CurriculumPopup : UI_Popup
{
    enum Images
    {
        CurriculumImage,
    }

    enum Buttons
    {
        ExitButton,
        BackgroundButton,
    }

    enum GameObjects
    {
        SectionGroup,
    }

    public bool _isHideOrShowing = false;
    List<UI_SectionItem> sectionItems = new List<UI_SectionItem>();

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
        BindObject(typeof(GameObjects));

        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnClickExitButton);
        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickExitButton);

        Refresh();
        SetInfo();

        return true;
    }

    private void Update()
    {
        if (!_isHideOrShowing)
            return;

        float height = 0;
        sectionItems.ForEach((item) =>
        {
            height += item.gameObject.GetOrAddComponent<RectTransform>().sizeDelta.y;
        });
        GetObject((int)GameObjects.SectionGroup).GetOrAddComponent<RectTransform>().sizeDelta = new Vector2(1080, height);
    }

    public void SetInfo()
    {
        Refresh();

        float height = 0;
        foreach (Data.SectionItemData sectionData in Managers.Data.SectionItemDataDic.Values)
        {
            UI_SectionItem item = Managers.UI.MakeSubItem<UI_SectionItem>(GetObject((int)GameObjects.SectionGroup).transform);
            item.SetInfo(sectionData);
            height += item._height;
            sectionItems.Add(item);
        }

        GetObject((int)GameObjects.SectionGroup).GetOrAddComponent<RectTransform>().sizeDelta = new Vector2(1080, height);
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.SectionGroup).GetOrAddComponent<RectTransform>());
    }

    void Refresh()
    {

    }

    private void OnEnable()
    {
        GameObject go = GetImage((int)Images.CurriculumImage).gameObject;
        go.transform.localScale = Vector3.right;
        DOTween.Sequence().SetAutoKill(false)
        .OnStart(() =>
        {
            GetButton((int)Buttons.ExitButton).interactable = false;
        })
        .Append(go.transform.DOScale(1f, 0.5f).SetEase(Ease.OutBounce))
        .OnComplete(() =>
        {
            GetButton((int)Buttons.ExitButton).interactable = true;
        });
    }

    public void OnClickExitButton()
    {
        GameObject go = GetImage((int)Images.CurriculumImage).gameObject;

        DOTween.Sequence().SetAutoKill(false)
        .OnStart(() =>
        {
            GetButton((int)Buttons.ExitButton).interactable = false;
        })
        .Append(go.transform.DOScaleY(0f, 0.5f).SetEase(Ease.OutBounce))
        .OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

}
