using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public static class DI
    {
        private static Dictionary<Type, object> objectByType = new Dictionary<Type, object>();

        public static void Add<T>(T obj) where T : class
        {
            if (obj == null) return;
            Add(typeof(T), obj);
        }

        public static void Add(Type t, object obj)  {objectByType[t] = obj;             Debug.Log(t);
}

        public static T Get<T>() where T : class
        {
            var type = typeof(T);

            if (objectByType.TryGetValue(type, out var obj)) return obj as T;

            return null;
        }
    }
}