using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Pld
{
    class UtilUI
    {
        /// <summary>
        /// 获取最上层点击对象        /// </summary>        /// <returns>点击对象GameObject</returns>        public static GameObject GetTopTouchGameObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            if (results.Count > 0)
            {
                return results[0].gameObject;
            }
            else
                return null;
        }

    }
}
