using UC;
using UnityEngine;
using WSKit;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(OnEvent))]
[RequireComponent(typeof(Collider))]
public class CollisionTrigger : MonoBehaviour
{
    private enum Type { Enter, Stay, Exit };

    [SerializeField] private Type       type;
    [SerializeField] private Hypertag[] allowedTags;

    OnEvent eventHandler;

    void Start()
    {
        eventHandler = GetComponent<OnEvent>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if ((type == Type.Enter) && (eventHandler) && (HypertaggedObject.CheckTags(collision, allowedTags))) Trigger();
    }

    private void OnTriggerStay(Collider collision)
    {
        if ((type == Type.Stay) && (eventHandler) && (HypertaggedObject.CheckTags(collision, allowedTags))) Trigger();
    }

    private void OnTriggerExit(Collider collision)
    {
        if ((type == Type.Exit) && (eventHandler) && (HypertaggedObject.CheckTags(collision, allowedTags))) Trigger();
    }

    private void Trigger()
    {
        eventHandler._TriggerEvent(eventHandler.eventType);
    }
}
