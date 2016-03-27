using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public int actionpointsLeft = 3;

    public enum enemyState
    {
        idle,
        playTurn,
        inactive
    }
    public enemyState state = enemyState.idle;

}