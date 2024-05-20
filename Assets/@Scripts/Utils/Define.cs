using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public static readonly Dictionary<Type, Array> _enumDict = new Dictionary<Type, Array>();

    public enum Scene
    {
        Unknown,
        TitleScene,
        LobbyScene,
        GameScene,
    }

    public enum ObjectType
    {
        Player,
    }

    public enum UIType
    {
        Text,
        Button
    }

    public enum UIEvent
    {
        Click,
        Pressed,
        PointerDown,
        PointerUp,
        Drag,
        BeginDrag,
        EndDrag,
    }

    public enum Sound
    {
        Bgm,
        SubBgm,
        Effect,
        Max,
    }

    public enum PostType
    {
        login,
        register,
        complete,
        autologin
    }


    public enum MissionTarget // 미션 조건
    {
        Level,      // 총 레벨
        Study,      // 총 공부시간
        XP,         // 총 경험치
        DoneQuest,  // 수행완료 퀘스트
        ADWatchIng, // 광고 시청
        StageEnter, // 스테이지 입장
        StageClear, // 스테이지 클리어
    }


    public const int PLAYER_DATA_ID = 1;
}
