using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InterfaceFinderInactive
{
    public static List<T> FindObjectsOfInterfaceIncludingInactive<T>() where T : class
    {
        List<T> results = new List<T>();

        // ������� ��� GameObject �� �����
        GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>(true);

        foreach (GameObject go in allGameObjects)
        {
            // �������� ��� ���������� �� GameObject
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
        // ������� ��� GameObject �� �����
        GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>(true);

        foreach (GameObject go in allGameObjects)
        {
            // �������� ��� ���������� �� GameObject
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
