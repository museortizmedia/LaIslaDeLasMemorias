using UnityEngine;

public class AnimationEventManager : MonoBehaviour
{
    [System.Serializable]
    public class AnimationEventInfo
    {
        public string eventName;
        public UnityEngine.Events.UnityEvent unityEvent;
    }
    public AnimationEventInfo[] animationEvents;

    public void InvokeAnimationEvent(string eventName)
    {
        foreach (var animationEvent in animationEvents)
        {
            if (animationEvent.eventName == eventName)
            {
                animationEvent.unityEvent.Invoke();
                break;
            }
        }
    }
}
