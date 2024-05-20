using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_FlowBase : UI_Base
{

    protected Data.FlowItemData _data;
    public Data.TextItemData _answerId;
    protected List<Data.TextItemData> _questionIds = new List<Data.TextItemData>();
    protected List<Data.TextItemData> _extraIds = new List<Data.TextItemData>();

    private void Awake()
    {
        Init();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public virtual void SetInfo(Data.FlowItemData data, Action<int> callback)
    {
    }

    protected void ChangeSprite(int imageNum, int textItemId, Define.UIType uiType)
    {
        if (Managers.Data.TextItemDataDic.TryGetValue(textItemId, out Data.TextItemData textData))
        {

            Image image;
            switch (uiType)
            {
                case Define.UIType.Text:
                    image = GetImage(imageNum);
                    break;
                case Define.UIType.Button:
                    image = GetButton(imageNum).GetOrAddComponent<Image>();
                    break;
                default:
                    image = GetImage(imageNum);
                    break;
            }
            image.sprite = Managers.Resource.Load<Sprite>(textData.Text);
            image.SetNativeSize();
        }
    }

    protected Data.TextItemData GetTextItem(int textId)
    {
        if (Managers.Data.TextItemDataDic.TryGetValue(textId, out Data.TextItemData textData))
        {
            return textData;
        }
        return null;
    }

    public virtual void ContinueClicked()
    {

    }

    public int GetAnswer()
    {
        if (Int32.TryParse(_answerId.AText, out int result))
        {
            return result;
        }
        return -1;
    }
}
