using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Newtonsoft.Json;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Networking;
using UnityEngine.UI;

public abstract class UI_Base : MonoBehaviour
{
    protected Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();
    protected bool _init = false;

    public virtual bool Init()
    {
        if (_init)
            return false;

        _init = true;
        return true;
    }

    private void Start()
    {
        Init();
        LocalizeAllTexts();
    }

    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        _objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, names[i], true);
            else
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);

            if (objects[i] == null)
                Debug.Log($"Failed to bind({names[i]})");
        }
    }

    protected void BindObject(Type type) { Bind<GameObject>(type); }
    protected void BindImage(Type type) { Bind<Image>(type); }
    protected void BindText(Type type) { Bind<TMP_Text>(type); }
    protected void BindInputText(Type type) { Bind<TMP_InputField>(type); }
    protected void BindButton(Type type) { Bind<Button>(type); }
    protected void BindToggle(Type type) { Bind<Toggle>(type); }


    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[idx] as T;
    }

    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
    protected TMP_Text GetText(int idx) { return Get<TMP_Text>(idx); }
    protected TMP_InputField GetInputText(int idx) { return Get<TMP_InputField>(idx); }
    protected Button GetButton(int idx) { return Get<Button>(idx); }
    protected Image GetImage(int idx) { return Get<Image>(idx); }
    protected Toggle GetToggle(int idx) { return Get<Toggle>(idx); }

    public static void BindEvent(GameObject go, Action action = null, Action<BaseEventData> dragAction = null, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

        switch (type)
        {
            case Define.UIEvent.Click:
                evt.OnClickHandler -= action;
                evt.OnClickHandler += action;
                break;
            case Define.UIEvent.Pressed:
                evt.OnPressedHandler -= action;
                evt.OnPressedHandler += action;
                break;
            case Define.UIEvent.PointerDown:
                evt.OnPointerDownHandler -= action;
                evt.OnPointerDownHandler += action;
                break;
            case Define.UIEvent.PointerUp:
                evt.OnPointerUpHandler -= action;
                evt.OnPointerUpHandler += action;
                break;
                // case Define.UIEvent.Drag:
                //     evt.OnDragHandler -= dragAction;
                //     evt.OnDragHandler += dragAction;
                //     break;
                // case Define.UIEvent.BeginDrag:
                //     evt.OnBeginDragHandler -= dragAction;
                //     evt.OnBeginDragHandler += dragAction;
                //     break;
                // case Define.UIEvent.EndDrag:
                //     evt.OnEndDragHandler -= dragAction;
                //     evt.OnEndDragHandler += dragAction;
                //     break;
        }
    }

    public void PopupOpenAnimation(GameObject contentObject) // 팝업 오픈 연출
    {
        contentObject.transform.localScale = new Vector3(0.8f, 0.8f, 1);
        contentObject.transform.DOScale(1f, 0.1f).SetEase(Ease.InOutBack).SetUpdate(true);
    }

    protected virtual void LocalizeAllTexts()
    {
    }

    protected string ConvertToJson<T>(T data)
    {
        return JsonConvert.SerializeObject(data);
    }

    protected string _url = "http://218.232.112.104:3000/";

    protected IEnumerator GetJsonData(string jsonStr, Action haveData, Action noData)
    {
        string url = _url;
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(request.error);
                noData?.Invoke();
            }
            else
            {
                GameData data = JsonConvert.DeserializeObject<GameData>(request.downloadHandler.text);
                if (data != null)
                {
                    Managers.Game._gameData = data;
                    print("GameLoadedWell");
                    haveData?.Invoke();
                }
                else
                {
                    print("GameLoadedNotWell");
                    noData?.Invoke();
                }
            }
        }
    }

    protected IEnumerator PostJsonData(string id, string jsonStr, Action<string> haveData, Action noData)
    {
        string url = _url + id;
        print(url);

        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(url, jsonStr))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonStr);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            if (PlayerPrefs.HasKey("token"))
                request.SetRequestHeader("token", PlayerPrefs.GetString("token"));
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(request.responseCode);
                Debug.Log(request.result);
                Debug.Log(request.downloadHandler.text);
                noData?.Invoke();
            }
            else
            {
                Debug.Log(request.result);
                Debug.Log(request.downloadHandler.text);
                haveData?.Invoke(request.downloadHandler.text);
            }
        }
    }
}
