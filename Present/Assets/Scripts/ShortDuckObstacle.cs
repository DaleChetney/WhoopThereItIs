using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortDuckObstacle : ObstacleScript
{
    internal override void Despawn()
    {
        ObjectPoolService.Instance.ReleaseInstance<ShortDuckObstacle>(this);
    }
}
