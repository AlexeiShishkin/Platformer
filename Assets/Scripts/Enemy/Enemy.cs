using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Vector2 _spawnPoint;

    public void Restart()
    {
        transform.position = _spawnPoint;
    }

    private void Start()
    {
        _spawnPoint = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            player.Die();
        }
    }
}
