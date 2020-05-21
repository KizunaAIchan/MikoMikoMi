using UnityEngine;
using System;

    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        protected static T _instance;

        public static T instance
        {
            get
            {
                return _instance;
            }
        }

        public static void InitSingletonInstance()
        {
            if (_instance == null)
            {
                _instance = Activator.CreateInstance<T>();
                if (_instance != null)
                {
                    (_instance as Singleton<T>).Init();
                }
            }
        }

        public abstract void Init();
    }

