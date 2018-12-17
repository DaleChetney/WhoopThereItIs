using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingToilet : RunnerObject
{
    internal override void CollideEffects()
    {
        GameManagerScript.Instance.GoToLoseScreen();
    }

    internal override void Despawn()
    {
        ObjectPoolService.Instance.ReleaseInstance<EndingToilet>(this);
    }
}
