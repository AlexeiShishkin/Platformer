using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PhysicMovement))]
public class PlayerMovement : MonoBehaviour
{
    private PhysicMovement _physicMovement;
    private MoveState _moveState = MoveState.Idle;
    private float _targetVelocityX;

    private const string Horizontal = nameof(Horizontal);

    public event UnityAction Idled;
    public event UnityAction Ran;
    public event UnityAction Jumped;
    public event UnityAction TurnedLeft;
    public event UnityAction TurnedRight;

    private void Awake()
    {
        _physicMovement = GetComponent<PhysicMovement>();
    }

    private void OnEnable()
    {
        _physicMovement.Grounded += OnGrounded;
    }

    private void OnDisable()
    {
        _physicMovement.Grounded -= OnGrounded;
    }

    private void Update()
    {
        float oldVelocityX = _targetVelocityX;
        _targetVelocityX = Input.GetAxis(Horizontal);
        _physicMovement.SetTargetVelocity(new Vector2(_targetVelocityX, 0));

        if (oldVelocityX <= 0 && _targetVelocityX > 0)
        {
            TurnedRight?.Invoke();
        }
        else if (oldVelocityX >= 0 && _targetVelocityX < 0)
        {
            TurnedLeft?.Invoke();
        }

        if (_moveState != MoveState.Jump)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                _physicMovement.Jump();
                _moveState = MoveState.Jump;
                Jumped?.Invoke();
            }
            else
            {
                ChangeState();
            }
        }
    }

    private void OnGrounded()
    {
        ChangeState();
    }

    private void ChangeState()
    {
        if (_moveState != MoveState.Run && _targetVelocityX != 0)
        {
            _moveState = MoveState.Run;
            Ran?.Invoke();
        }
        else if (_moveState != MoveState.Idle && _targetVelocityX == 0)
        {
            _moveState = MoveState.Idle;
            Idled?.Invoke();
        }
    }

    enum MoveState
    {
        Idle,
        Run,
        Jump
    }
}
