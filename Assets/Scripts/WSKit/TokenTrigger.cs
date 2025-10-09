using UC;
using UnityEngine;
using WSKit;

public class TokenTrigger : MonoBehaviour
{
    [SerializeField] private WSKit.EventType tokenChangeEvent;

    TokenManager tokenManager;

    private void Start()
    {
        tokenManager = GetComponent<TokenManager>();
        tokenManager.onChange += TokenManager_onChange;
    }

    private void TokenManager_onChange(bool add, Hypertag token, int quantity)
    {
        if (tokenChangeEvent != null)
        {
            OnEvent.TriggerEvent(tokenChangeEvent);
        }
    }
}
