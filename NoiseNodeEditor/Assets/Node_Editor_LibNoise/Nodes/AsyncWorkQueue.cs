using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class AsyncWorkQueue : MonoBehaviour
{
    //public int ThreadCount = 2;

    //Queue<Action> _asyncWorkQueue = new Queue<Action>();
    Queue<Action> _mainWorkQueue = new Queue<Action>();

    private static AsyncWorkQueue Instance { get; set; }
    
    AsyncWorkQueue()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
    }

    private void OnEnable()
    {
#if UNITY_EDITOR
        EditorApplication.update -= ServiceMainThread;
        EditorApplication.update += ServiceMainThread;
#endif
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        EditorApplication.update -= ServiceMainThread;
#endif
    }
    
    void ServiceMainThread()
    {
        lock (_mainWorkQueue)
        {
            if (_mainWorkQueue.Count > 0)
            {
                var func = _mainWorkQueue.Dequeue();
                if (func != null)
                    func();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        ServiceMainThread();
    }

    static void ThreadProc(object stateInfo)
    {
        WorkUnit unit = stateInfo as WorkUnit;
        
        unit.DoWork();
    }

    class WorkUnit
    {
        public string Name;
        public Action DoWork;
    }

    static public void QueueBackgroundWork(string name, Action doWork)
    {
        if (Instance != null)
        {
            WorkUnit unit = new WorkUnit();
            unit.Name = name;
            unit.DoWork = doWork;

            ThreadPool.QueueUserWorkItem(ThreadProc, unit);
        }
        else
            Debug.LogErrorFormat("QueueBackgroundWork called with no AsyncWorkQueue instantiated");
    }

    static public void QueueMainThreadWork(Action doWork)
    {
        if (Instance != null)
        {
            lock (Instance._mainWorkQueue)
            {
                Instance._mainWorkQueue.Enqueue(doWork);
            }
        }
        else
            Debug.LogErrorFormat("QueueMainThreadWork called with no AsyncWorkQueue instantiated");
    }
}
