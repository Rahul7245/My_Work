using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    static ImpactManager shotInvoker;
    static UnityAction<int>[] shotListeners=new UnityAction<int>[2];
    static ImpactManager cameraSwitchInvoker;
    static UnityAction<bool> cameraSwitchListener;

    static TrackSpawner reloadWeaponInvoker;
    static UnityAction reloadWeaponListener;
   static int i=0;
    public static void  AddShotInvoker(ImpactManager script) 
    {
        shotInvoker = script;
        if (shotListeners != null) {
            foreach(var shotListener in shotListeners)
            shotInvoker.AddShotEventListener(shotListener);
        }
    }

    public static void AddShootListener(UnityAction<int> listener_to_register)
    {
        Console.WriteLine("shotListeners::");
        shotListeners[i] = listener_to_register;
        i++;
        if (shotInvoker != null)
        {
            shotInvoker.AddShotEventListener(shotListeners[shotListeners.Length]);
        }
    }
    public static void AddCameraSwitchInvoker(ImpactManager script)
    {
        cameraSwitchInvoker = script;
        if (cameraSwitchListener != null)
        {
            cameraSwitchInvoker.AddSwitchListener(cameraSwitchListener);
        }
    }

    public static void AddCameraSwitchtListener(UnityAction<bool> listener_to_register)
    {
        cameraSwitchListener = listener_to_register;
        if (cameraSwitchInvoker != null)
        {
            cameraSwitchInvoker.AddSwitchListener(cameraSwitchListener);
        }
    }



    public static void AddReloadWeaponInvoker(TrackSpawner script)
    {
        reloadWeaponInvoker = script;
        if (reloadWeaponListener != null)
        {
            reloadWeaponInvoker.AddResetWeaponListener(reloadWeaponListener);
        }
    }

    public static void AddReloadWeapontListener(UnityAction listener_to_register)
    {
        reloadWeaponListener = listener_to_register;
        if (reloadWeaponInvoker != null)
        {
            
            reloadWeaponInvoker.AddResetWeaponListener(reloadWeaponListener);
        }
    }
}
