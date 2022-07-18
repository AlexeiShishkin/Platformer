using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    private Vector2 _respawnPoint;
    private int _coinsCount = 0;

    public event UnityAction Died;

    public void CollectCoin()
    {
        _coinsCount++;
    }

    public void Die()
    {
        Debug.Log("Мы умерли");
        Died?.Invoke();
    }

    public void Restart()
    {
        gameObject.SetActive(false);
        transform.position = _respawnPoint;
        _coinsCount = 0;
        gameObject.SetActive(true);
    }

    private void Start()
    {
        _respawnPoint = transform.position;
    }
}
