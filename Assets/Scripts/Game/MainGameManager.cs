using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : MonoBehaviour
{
    public enum Diffculty
    {
        None,
        Easy,
        Normal,
        Hard,
    }

    private static MainGameManager instance;

    public static MainGameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MainGameManager();
                return instance;
            }
            return instance;
        }
    }

    public Diffculty currentDiffcult = Diffculty.None;

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

    public void SetDifficult(Diffculty _dif)
    {
        if (currentDiffcult != Diffculty.None)
            return;

        currentDiffcult = _dif;
    }
}
