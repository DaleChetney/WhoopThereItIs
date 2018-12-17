using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortJumpObstacle : ObstacleScript
{
    internal override void Despawn()
    {
        ObjectPoolService.Instance.ReleaseInstance<ShortJumpObstacle>(this);
    }
}
