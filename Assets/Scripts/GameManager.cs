using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameOptions    _gameOptions;

    static GameManager _instance;

    public static GameManager Instance => _instance;
    public static GameOptions GameOptions => _instance._gameOptions;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
}
