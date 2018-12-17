using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : RunnerObject
{
    internal override void CollideEffects()
    {
        ResponseManager.Instance.ScrambleCollectedResponses();
        GameObject.Find("DaydreamPlayer").GetComponent<PlayerController2D>().Knockback();
        RunnerManager.Instance.InterruptScrolling();
    }
}
