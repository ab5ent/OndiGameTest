using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityComponent
{
    public void Initialize(Entity owner);
}