using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class UI_StorePopup : UI_Popup
{
    enum Texts
    {
        StoreText,
        MoneyText
    }

    enum Buttons
    {
        ExitButton
    }

    enum GameObjects
    {
        BackgroundImage
    }

    private void Awake()
    {
        Init();
    }

    private float _duration = 0.2f;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindObject(typeof(GameObjects));

        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnClickExitButton);

        Refresh();

        return true;
    }

    public void SetInfo()
    {
        Refresh();
    }


    public void OpenStorePopup()
    {
        gameObject.SetActive(true);
        GetObject((int)GameObjects.BackgroundImage).GetOrAddComponent<RectTransform>().anchoredPosition = new Vector2(1, -2500);
        GetObject((int)GameObjects.BackgroundImage).GetOrAddComponent<RectTransform>().DOAnchorPos3DY(0, _duration);
    }

    public void CloseStorePopup()
    {
        GetObject((int)GameObjects.BackgroundImage).GetOrAddComponent<RectTransform>().DOAnchorPos3DY(-2500, _duration).OnComplete(() => gameObject.SetActive(false));
    }

    void Refresh()
    {

    }

    void OnClickExitButton()
    {
        CloseStorePopup();
    }

    protected override void LocalizeAllTexts()
    {
        Locale currentLanguage = LocalizationSettings.SelectedLocale;
        for (int i = 0; i < (int)Texts.StoreText; i++)
        {
            GetText(i).text = LocalizationSettings.StringDatabase.GetLocalizedString("MyTable", System.Enum.GetName(typeof(Texts), i), currentLanguage);
        }
    }
}
