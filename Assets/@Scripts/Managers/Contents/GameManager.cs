using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using static Define;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using UnityEngine.Networking;


[Serializable]
public class MissionInfo
{
    public int SectionId = 1000000001;
    public int StepId = 1000010001;
    public int UnitId = 1010001001;
    public int ActivityId = 1020000001;
}

[Serializable]
public class GameData
{
    public string UserName = "Player";
    public string Email = "Player@naver.com";
    public string PhoneNumber = "010-0101-0101";
    public int LanguageNum = 1;
    public MissionInfo CurrentUserState = new MissionInfo();
}

public class PlayData
{
    public GameObject CurrentUnitObject = null;
    public UI_LobbyScene LobbySceneUI = null;

}

[Serializable]
public class ProtocolPostLogin
{
    public string UserName = "DefaultID";
    public string Password = "DefaultPassword";
}

[Serializable]
public class ProtocolPostRegister
{
    public string UserName = "DefaultID";
    public string Password = "DefaultPassword";
    public string Email = "DefaultEmail";
    public int LanguageNumber = 1;
    public string PhoneNumber = "010-1111-2222";
}

[Serializable]
public class ProtocolGetLogin
{
    public string UserName;
    public string Email;
    public int LanguageNumber;
    public int SectionId;
    public int StepId;
    public int UnitId;
    public int ActivityId;
    public string Token;
}

[Serializable]
public class ProtocolPostActivityClear
{
    public string UserName;
    public int SectionId;
    public int StepId;
    public int UnitId;
    public int ActivityId;
}

public class GameManager
{
    #region GameData
    public GameData _gameData = new GameData();
    public PlayData _playData = new PlayData();
    public string UserName
    {
        get { return _gameData.UserName; }
        set { _gameData.UserName = value; }
    }
    public MissionInfo CurrentUserState
    {
        get { return _gameData.CurrentUserState; }
        set { _gameData.CurrentUserState = value; }
    }
    public int LanguageNum
    {
        get { return _gameData.LanguageNum - 1; }
        set { _gameData.LanguageNum = value + 1; }
    }
    public string Email
    {
        get { return _gameData.Email; }
        set { _gameData.Email = value; }
    }
    public GameObject CurrentUnitObject
    {
        get { return _playData.CurrentUnitObject; }
        set { _playData.CurrentUnitObject = value; }
    }
    public UI_LobbyScene LobbyScene
    {
        get { return _playData.LobbySceneUI; }
        set { _playData.LobbySceneUI = value; }
    }
    public ProtocolGetLogin PlayerLogin
    {
        set
        {
            UserName = value.UserName;
            Email = value.Email;
            LanguageNum = value.LanguageNumber;
            CurrentUserState.SectionId = value.SectionId;
            CurrentUserState.StepId = value.StepId;
            CurrentUserState.UnitId = value.UnitId;
            CurrentUserState.ActivityId = value.ActivityId;
            PlayerPrefs.SetString("token", value.Token);
        }
    }

    #region Action
    #endregion

    #region Option
    public bool EffectSoundOn
    {
        get { return EffectSoundOn; }
        set { EffectSoundOn = value; }
    }

    public bool BGMOn
    {
        get { return BGMOn; }
        set
        {
            if (BGMOn == value)
                return;
            BGMOn = value;
            if (BGMOn == false)
            {
                Managers.Sound.Stop(Define.Sound.Bgm);
            }
            else
            {
                string name = "Bgm_Lobby";
                if (Managers.Scene.CurrentScene.SceneType == Define.Scene.GameScene)
                    name = "Bgm_Game";

                Managers.Sound.Play(Define.Sound.Bgm, name);
            }
        }
    }

    #endregion
    #endregion

    // [Serializable]
    // public class TempClass
    // {
    //     public int Pid;
    //     public int Id;
    //     public int Seq;
    //     public List<int> FlowIds;
    // }

    // [Serializable]
    // public class TempTempClass
    // {
    //     public List<TempClass> Temp = new List<TempClass>();
    // }

    // public void TempMakeFile()
    // {
    //     _path = Application.persistentDataPath + "/ActivityItemData.json";
    //     TempTempClass TempTemp = new TempTempClass();


    //     int tens = 0;
    //     foreach (UnitItemData unitData in Managers.Data.UnitItemDataDic.Values)
    //     {
    //         int i = 1;
    //         foreach (int activityId in unitData.ActivityIds)
    //         {
    //             TempTemp.Temp.Add(new TempClass() { Pid = unitData.Id, Id = activityId, Seq = i++, FlowIds = new List<int>() { 2000000000 + tens + 1, 2000000000 + tens + 2, 2000000000 + tens + 3, 2000000000 + tens + 4, 2000000000 + tens + 5 } });
    //             tens += 10;
    //         }

    //     }

    //     string jsonStr = JsonConvert.SerializeObject(TempTemp);
    //     File.WriteAllText(_path, jsonStr);
    //     Debug.Log($"TempMakeFile Completed : {_path}");

    // }

    public void Init()
    {
        _path = Application.persistentDataPath + "/" + UserName + ".json";

        if (File.Exists(_path))
        {
            string fileStr = File.ReadAllText(_path);
            GameData data = JsonConvert.DeserializeObject<GameData>(fileStr);
            if (data != null)
                _gameData = data;
        }

        SaveGame();
    }

    #region Save&Load
    public string _path = "";
    public void SaveGame()
    {
        _path = Application.persistentDataPath + "/" + UserName + ".json";
        string jsonStr = JsonConvert.SerializeObject(_gameData);
        File.WriteAllText(_path, jsonStr);
        Debug.Log($"Save Game Completed : {_path}");
    }
    public void SaveGameToPlayer()
    {
        _path = Application.persistentDataPath + "/Player.json";
        string jsonStr = JsonConvert.SerializeObject(_gameData);
        File.WriteAllText(_path, jsonStr);
        Debug.Log($"Save Game Completed : {_path}");
    }

    public bool LoadGame()
    {
        _path = Application.persistentDataPath + "/" + UserName + ".json";

        if (File.Exists(_path) == false)
        {
            Init();
            return false;
        }

        string fileStr = File.ReadAllText(_path);
        GameData data = JsonConvert.DeserializeObject<GameData>(fileStr);
        if (data != null)
            _gameData = data;

        Debug.Log($"Save Game Loaded : {_path}");
        return true;
    }

    public void DeleteGame()
    {
        _path = Application.persistentDataPath + "/Player.json";
        if (File.Exists(_path))
        {
            File.Delete(_path);
        }
        return;
    }

    #endregion

}
