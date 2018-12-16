using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortJumpObstacle : ObstacleScript
{
    internal override void ReleaseSelf()
    {
        ObjectPoolService.Instance.ReleaseInstance<ShortJumpObstacle>(this);
    }
}
