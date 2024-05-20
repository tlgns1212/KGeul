using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectManager
{
    // public PlayerController Player { get; private set; }
    // public HashSet<MonsterController> Monsters { get; } = new HashSet<MonsterController>();

    // public Transform SkillTransform
    // {
    //     get
    //     {
    //         GameObject root = GameObject.Find("@Skill");
    //         if (root == null)
    //         {
    //             root = new GameObject { name = "@Skill" };
    //         }
    //         return root.transform;
    //     }
    // }

    public ObjectManager()
    {
        Init();
    }

    public void Init()
    {

    }

    public void Clear()
    {
        // Monsters.Clear();
    }

    public T Spawn<T>(Vector3 position, int templateID = 0, string prefabName = "") where T : BaseController
    {
        System.Type type = typeof(T);

        // if (type == typeof(PlayerController))
        // {
        //     GameObject go = Managers.Resource.Instantiate(Managers.Data.CreatureDic[templateID].PrefabLabel);
        //     go.transform.position = position;
        //     PlayerController pc = go.GetOrAddComponent<PlayerController>();
        //     pc.SetInfo(templateID);
        //     Player = pc;
        //     Managers.Game.Player = pc;

        //     return pc as T;
        // }
        // else if (type == typeof(MonsterController))
        // {
        //     Data.CreatureData cd = Managers.Data.CreatureDic[templateID];
        //     GameObject go = Managers.Resource.Instantiate($"{cd.PrefabLabel}", pooling: true);
        //     MonsterController mc = go.GetOrAddComponent<MonsterController>();
        //     go.transform.position = position;
        //     mc.SetInfo(templateID);
        //     go.name = cd.PrefabLabel;
        //     Monsters.Add(mc);

        //     return mc as T;
        // }

        return null;
    }

    public void Despawn<T>(T obj) where T : BaseController
    {
        System.Type type = typeof(T);

        // if (type == typeof(PlayerController))
        // {
        //     // ?
        // }

        // else if (type == typeof(MonsterController))
        // {
        //     Monsters.Remove(obj as MonsterController);
        //     Managers.Resource.Destroy(obj.gameObject);
        // }

    }

}
