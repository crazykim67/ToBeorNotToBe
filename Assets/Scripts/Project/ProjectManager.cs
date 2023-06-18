using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectManager : MonoBehaviour
{
    private static ProjectManager instance;

    public static ProjectManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ProjectManager();
                return instance;
            }
            return instance;
        }
    }

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    
}
