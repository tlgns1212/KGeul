using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StepDecoItem : UI_Base
{
    private void Awake()
    {
        Init();
    }

    public Data.DecoItemData _data;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public void SetInfo(Data.DecoItemData data)
    {
        _data = data;
        gameObject.GetOrAddComponent<Image>().sprite = Managers.Resource.Load<Sprite>(data.ImageId);
        gameObject.GetOrAddComponent<Image>().SetNativeSize();
        RectTransform rectTransform = gameObject.GetOrAddComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(data.PosX, data.PosY);
    }

}
