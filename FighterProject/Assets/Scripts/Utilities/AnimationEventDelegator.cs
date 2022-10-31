using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class AnimationEventDelegator : MonoBehaviour
{
    public UnityEvent unityEvent_01;

    public void AnimationTriggered_01()
    { unityEvent_01.Invoke(); }
}
