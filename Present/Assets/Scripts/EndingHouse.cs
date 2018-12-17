using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingHouse : RunnerObject
{
    internal override void CollideEffects()
    {
        GameManagerScript.Instance.GoToWinScreen();
    }

    internal override void Despawn()
    {
        ObjectPoolService.Instance.ReleaseInstance<EndingHouse>(this);
    }
}
