
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class MultiMonitor : MonoBehaviour
{
    int monitor_count;

    void Start()
    {

        monitor_count = Display.displays.Length;

#if UNITY_EDITOR
        Debug.Log("Editor only fix: set 3 monitors");
        monitor_count = 3;
#endif
#if !UNITY_EDITOR
            // activate monitors
             for (int i = 1; i < monitor_count; i++)
             {
                 Display.displays[i].Activate();
             }
#endif

        Debug.Log("monitor_count:" + monitor_count);

    }
}