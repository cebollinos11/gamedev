using UnityEngine;
using System.Collections;
using System;

public class Dialog : MonoBehaviour, IComparable {

    public int dialogID;
    public string dialogText;

    public enum talker
    {
        boss,
        player
    }
    public talker dialogTalker = talker.boss;

    public int CompareTo(object other)
    {
        return dialogID.CompareTo(((Dialog)other).dialogID);
    }
}
