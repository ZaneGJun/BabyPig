using System;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

class GameController : MonoBehaviour
{
    Systems _systems;

    void Start()
    {
        // 获取Entitas的上下文对象，类似一个单例管理器
        var contexts = Contexts.sharedInstance;

        _systems = new Feature("Systems").Add(new TutorialSystems(contexts));

        // 初始化System
        _systems.Initialize();

        // test
        contexts.game.CreateEntity().AddDebugMessage("Hello World!");
    }

    void Update()
    {
        // 调用System的Execute函数，这里并不是每帧都执行Execute逻辑，因为Syetem里Execute会在实体满足一定条件的情况下才执行的（GetTrigger和Filter函数的作用）
        _systems.Execute();
    }
}

