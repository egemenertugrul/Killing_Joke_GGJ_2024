using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace KillingJoke.Core.Utils
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Creates a child GameObject based on a parent transform. Returns the new GameObject.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public static GameObject CreateChild(this Transform parent, GameObject prefab)
        {
            return UnityEngine.Object.Instantiate(prefab, parent);
        }

        /// <summary>
        /// Creates a child GameObject based on a parent transform. Returns the specified component of the new GameObject.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public static T CreateChild<T>(this Transform parent, GameObject prefab) where T : MonoBehaviour
        {
            return UnityEngine.Object.Instantiate(prefab, parent).GetComponent<T>();
        }

        public static object CallGenericMethodThroughReflection(object o, string a, Type t, params object[] arguments)
        {
            MethodInfo genericMethod = o.GetType().GetGenericMethod(a);
            return genericMethod.MakeGenericMethod(new Type[] { t }).Invoke(o, arguments);

            //MethodInfo method = t1.GetMethod(a, BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance);

            // Build a method with the specific type argument you're interested in
            //method = method.MakeGenericMethod(t2);
            // The "null" is because it's a static method
            //return method.Invoke(null, arguments);
        }
    }


    public static class TypeExtensions
    {
        private class SimpleTypeComparer : IEqualityComparer<Type>
        {
            public bool Equals(Type x, Type y)
            {
                return x.Assembly == y.Assembly &&
                    x.Namespace == y.Namespace &&
                    x.Name == y.Name;
            }

            public int GetHashCode(Type obj)
            {
                throw new NotImplementedException();
            }
        }

        public static MethodInfo GetGenericMethod(this Type type, string name)
        {
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var method in methods.Where(m => m.Name == name))
            {
                //var methodParameterTypes = method.GetParameters().Select(p => p.ParameterType).ToArray();

                //if (methodParameterTypes.SequenceEqual(parameterTypes, new SimpleTypeComparer()))
                //{
                //    return method;
                //}
                if (method.IsGenericMethod)
                    return method;
            }

            return null;
        }
    }
}
