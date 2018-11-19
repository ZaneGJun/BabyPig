using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;
using System;

public class DebugMessageSystem : ReactiveSystem<GameEntity>
{
    public DebugMessageSystem(Contexts contexts) : base(contexts.game)
    {

    }

    protected override void Execute(List<GameEntity> entities)
    {
        //满足GetTrigger和Filter的实体保存在entities列表里
        foreach(var e in entities)
        {
            Debug.Log(e.debugMessage.message);
        }
    }

    protected override bool Filter(GameEntity entity)
    {
        // 只有hasDebugMessage为true的实体才会触发Execute函数
        return entity.hasDebugMessage;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
    {
        //只关心有DebugMessage组件的实体
        return context.CreateCollector(GameMatcher.DebugMessage);
    }
}
