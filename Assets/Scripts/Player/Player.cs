using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    private Vector2 _respawnPoint;
    private int _coinsCount = 0;

    public event UnityAction Died;

    private void Start()
    {
        _respawnPoint = transform.position;
    }

    public void CollectCoin()
    {
        _coinsCount++;
    }

    public void Die()
    {
        Died?.Invoke();
    }

    public void Restart()
    {
        gameObject.SetActive(false);
        transform.position = _respawnPoint;
        _coinsCount = 0;
        gameObject.SetActive(true);
    }
}
