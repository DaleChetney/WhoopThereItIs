using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongJumpObstacle : ObstacleScript
{
    internal override void ReleaseSelf()
    {
        ObjectPoolService.Instance.ReleaseInstance<LongJumpObstacle>(this);
    }
}
