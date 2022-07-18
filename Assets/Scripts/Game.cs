using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Player _player;

    private void OnEnable()
    {
        _player.Died += Restart;
    }

    private void OnDisable()
    {
        _player.Died -= Restart;
    }

    private void Restart()
    {
        _player.Restart();
    }
}
