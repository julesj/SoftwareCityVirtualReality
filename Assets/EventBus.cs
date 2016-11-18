using UnityEngine;
using System.Collections;
using System.Reflection;
using System;
using System.Collections.Generic;

public class EventBus : MonoBehaviour {

    private static EventBus instance;

    private Dictionary<string, List<System.Object>> registeredTargets = new Dictionary<string, List<System.Object>>();

    public void Awake()
    {
        EventBus.instance = this;
    }

    public static void Register(System.Object target)
    {
        EventBus.instance.RegisterInternal(target);
    }

    public static void Unregister(System.Object target)
    {
        EventBus.instance.UnregisterInternal(target);
    }

    public static void Post(System.Object eventObject)
    {
        EventBus.instance.PostInternal(eventObject);
    } 

    private void PostInternal(System.Object eventObject)
    {
        if (eventObject != null)
        {
            String objectTypeName = GetTypeName(eventObject.GetType());
            if (registeredTargets.ContainsKey(objectTypeName))
            {
                object[] param = new object[] { eventObject };
                foreach(System.Object target in registeredTargets[objectTypeName])
                {
                    //TODO Cache Method Objects
                    List<Type> typeList = new List<Type>();
                    typeList.Add(eventObject.GetType());
                    Type[] types = typeList.ToArray(); //FIXME das muss ja wohl auch besser gehen
                    MethodInfo method = target.GetType().GetMethod("OnEvent", types);
                    if (method != null)
                    {
                        try
                        {
                            method.Invoke(target, param);
                        } catch(Exception e)
                        {
                            Debug.Log("Exception occured while dispatching event " + objectTypeName + ": " + e.Message);
                        }
                       
                    }
                }
            }
        }
    }

    private void RegisterInternal(System.Object target)
    {
        MethodInfo[] methods = target.GetType().GetMethods();
        foreach(MethodInfo method in methods) {
            if (method.Name.Equals("OnEvent")) {
                ParameterInfo[] parameters = method.GetParameters();
                if (parameters.Length == 1)
                {
                    ParameterInfo parameter = parameters[0];
                    if (!parameter.IsOptional)
                    {
                        string parameterTypeName = GetTypeName(parameter.ParameterType);
                        List<System.Object> targets;
                        if (registeredTargets.ContainsKey(parameterTypeName))
                        {
                            targets = registeredTargets[parameterTypeName];
                        } else
                        {
                            targets = new List<System.Object>();
                            registeredTargets.Add(parameterTypeName, targets);
                        }
                        targets.Add(target);
                    }
                }
            }
        }

    }

    private void UnregisterInternal(System.Object target)
    {
        foreach(List<System.Object> targets in registeredTargets.Values) {
            targets.Remove(target);
        }
    }

    private string GetTypeName(Type type)
    {
        return type.Namespace + "." + type.Name;
    }

    



}
