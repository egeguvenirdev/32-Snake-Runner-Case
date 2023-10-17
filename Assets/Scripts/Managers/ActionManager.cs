using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public static class ActionManager
{
    //Game Actions
    public static Action GameStart { get; set; }

    public static Action<bool> GameEnd { get; set; }

    public static Action<float> ManagerUpdate { get; set; }

    public static Action MiniGameStarting { get; set; }

    public static Action MiniGameStarted { get; set; }
    public static Action BossMove { get; set; }


    //Collectables Actions
    public static Action<Transform> BallCollected { get; set; }


    //Grid Actions
    public static Action<bool> MergeColorCheck { get; set; }

    public static Action<GameObject> FindEmptyGrid { get; set; }

    public static Action<bool> ButtonCheck { get; set; }

    public static Action<int> BallSelect { get; set; }

    public static void ResetAllStaticVariables()
    {
        Type type = typeof(ActionManager);
        var fields = type.GetFields(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.Public);
        foreach (var fieldInfo in fields)
        {
            fieldInfo.SetValue(null, GetDefault(type));
        }
    }

    public static object GetDefault(Type type)
    {
        if (type.IsValueType)
        {
            return Activator.CreateInstance(type);
        }
        return null;
    }
}
