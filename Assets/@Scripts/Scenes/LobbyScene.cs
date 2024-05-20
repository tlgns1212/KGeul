using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.LobbyScene;

        Managers.UI.ShowSceneUI<UI_LobbyScene>();
        Screen.sleepTimeout = SleepTimeout.SystemSetting;
    }


    public override void Clear()
    {
    }

}
