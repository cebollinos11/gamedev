using UnityEngine;
using System.Collections;

public class CheckPointManager : Singleton<CheckPointManager> {

    bool used;
    public int level;
    public Vector3 position;
    

	public static void SaveCheckPoint(int lvl, Vector3 pos)
    {
        Instance.used = true;
        Instance.level = lvl;
        Instance.position = pos;
    }

    public static int GetLevel() {

        if (!Instance.used)
            return -2;
        return Instance.level;
    }

    public static Vector3 GetPosition()
    {
        return Instance.position;
    }

    public static void init() { 
        
    }
}
