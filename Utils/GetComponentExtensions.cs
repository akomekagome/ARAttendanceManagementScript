using System.Collections.Generic;
using UnityEngine;

namespace ARAM.Utils
{
    public static class GetComponentExtensions
    {
        public static List<Transform> GetAllChildren(this GameObject obj)
        {
            if (obj == null)
                return default;
            var allChildren = new List<Transform>();
            GetChildren(obj.transform, allChildren);
            return allChildren;
        }

        public static List<Transform> GetAllChildren(this Component obj)
        {
            if (obj == null)
                return default;
            var allChildren = new List<Transform>();
            GetChildren(obj, allChildren);
            return allChildren;
        }
        
        public static List<T> GetAllChildren<T>(this GameObject obj) where T : UnityEngine.Object
        {
            if (obj == null)
                return default;
            var allChildren = new List<T>();
            GetChildren(obj.transform, allChildren);
            return allChildren;
        }

        public static List<T> GetAllChildren<T>(this Component obj) where T : UnityEngine.Object
        {
            if (obj == null)
                return default;
            var allChildren = new List<T>();
            GetChildren(obj, allChildren);
            return allChildren;
        }
        
        private static void GetChildren(Component obj, List<Transform> allChildren)
        {
            var children = obj.GetComponentInChildren<Transform>();
            
            if (children.childCount == 0)
                return;
            
            foreach (Transform ob in children)
            {
                allChildren.Add(ob);
                GetChildren(ob, allChildren);
            }
        }
        
        private static void GetChildren<T>(Component obj, List<T> allChildren) where T : UnityEngine.Object
        {
            var children = obj.GetComponentInChildren<Transform>();
            
            if (children.childCount == 0)
                return;
            
            foreach (Transform ob in children)
            {
                var com = ob.GetComponent<T>();
                if (com != null)
                    allChildren.Add(com);
                GetChildren(ob, allChildren);
            }
        }
    }
}