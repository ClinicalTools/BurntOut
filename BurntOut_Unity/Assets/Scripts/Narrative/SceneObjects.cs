using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneObjects
{
    public static T[] FindObjectsOfType<T>()
    {
        var list = new List<T>();
        var rootObjs = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var rootObj in rootObjs)
            list.AddRange(rootObj.GetComponentsInChildren<T>(true));

        return list.ToArray();
    }

    public static GameObject[] FindGameObjectsWithTag(string tag)
    {
        var list = new List<GameObject>();

        var rootObjs = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (var rootObj in rootObjs)
        {
            if (rootObj.tag == tag)
                list.Add(rootObj.gameObject);

            list.AddRange(FindGameObjectsWithTag(rootObj.transform, tag));
        }

        return list.ToArray();
    }

    private static List<GameObject> FindGameObjectsWithTag(Transform parent, string tag)
    {
        var list = new List<GameObject>();

        foreach (Transform child in parent)
        {
            if (child.tag == tag)
                list.Add(child.gameObject);
            list.AddRange(FindGameObjectsWithTag(child, tag));
        }

        return list;
    }
}
