using System.Collections;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class TutorialSystems : Feature
{
   public TutorialSystems(Contexts contexts) : base("Tutorial Systems")
    {
        Add(new DebugMessageSystem(contexts));
    }
}
