using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private float _leftBound;
    [SerializeField] private float _rightBound;

    private void Update()
    {
        float x = Mathf.Clamp(_player.transform.position.x, _leftBound, _rightBound);

        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
}
