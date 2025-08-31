using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InterfaceFinderInactive
{
    public static List<T> FindObjectsOfInterfaceIncludingInactive<T>() where T : class
    {
        List<T> results = new List<T>();

        // Находим все GameObject на сцене
        GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>(true);

        foreach (GameObject go in allGameObjects)
        {
            // Получаем все компоненты на GameObject
            MonoBehaviour[] components = go.GetComponents<MonoBehaviour>();

            foreach (MonoBehaviour component in components)
            {
                if (component is T interfaceComponent)
                {
                    results.Add(interfaceComponent);
                }
            }
        }

        return results;
    }

    public static T FindObjectOfInterfaceIncludingInactive<T>() where T : class
    {
        // Находим все GameObject на сцене
        GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>(true);

        foreach (GameObject go in allGameObjects)
        {
            // Получаем все компоненты на GameObject
            MonoBehaviour[] components = go.GetComponents<MonoBehaviour>();

            foreach (MonoBehaviour component in components)
            {
                if (component is T interfaceComponent)
                {
                    return interfaceComponent;
                }
            }
        }

        return null;
    }
}
