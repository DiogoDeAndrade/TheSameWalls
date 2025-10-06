using UnityEngine;
using WSKit;

[CreateAssetMenu(fileName = "Interaction Verb", menuName = "WSKit/InteractionVerb")]
public class InteractionVerb : ScriptableObject
{
    public string       displayName;
    public CursorDef    cursorDef;
    public int          priority = 0;
}
