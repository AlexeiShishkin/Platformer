using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private float _speed;
    [SerializeField] private float _leftBound;
    [SerializeField] private float _rightBound;

    private void Update()
    {
        float x = _player.transform.position.x;

        if (x < _leftBound)
        {
            x = _leftBound;
        }
        else if (x > _rightBound)
        {
            x = _rightBound;
        }

        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
}
