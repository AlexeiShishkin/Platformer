using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _gravityModifier;
    [SerializeField] private float _minGroundNormal;

    private MoveState _moveState = MoveState.Idle;
    private Rigidbody2D _rigidbody;
    private Vector2 _velocity;
    private Vector2 _targetVelocity;
    private Vector2 _groundNormal;
    private bool _grounded;
    private ContactFilter2D _contactFilter;
    private readonly RaycastHit2D[] _hitBuffer = new RaycastHit2D[16];

    private const float _minMoveDistance = 0.001f;
    private const float _shellRadius = 0.01f;

    public event UnityAction Idled;
    public event UnityAction Ran;
    public event UnityAction Jumped;
    public event UnityAction TurnedLeft;
    public event UnityAction TurnedRight;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        _contactFilter.useTriggers = false;
    }

    private void Update()
    {
        float oldVelocityX = _targetVelocity.x;
        float targetVelosityX = Input.GetAxis("Horizontal");
        _targetVelocity = new Vector2(targetVelosityX, 0);

        if (oldVelocityX <= 0 && targetVelosityX > 0)
        {
            TurnedRight?.Invoke();
        }
        else if (oldVelocityX >= 0 && targetVelosityX < 0)
        {
            TurnedLeft?.Invoke();
        }

        if (_grounded)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                _velocity.y = _jumpForce;
                _moveState = MoveState.Jump;
                Jumped?.Invoke();
            }
            else if (_moveState != MoveState.Run && targetVelosityX != 0)
            {
                _moveState = MoveState.Run;
                Ran?.Invoke();
            }
            else if (_moveState != MoveState.Idle && targetVelosityX == 0)
            {
                _moveState = MoveState.Idle;
                Idled?.Invoke();
            }
        }
    }

    private void FixedUpdate()
    {
        _velocity += _gravityModifier * Time.fixedDeltaTime * Physics2D.gravity;
        _velocity.x = _targetVelocity.x * _speed;

        _grounded = false;

        Vector2 positionDelta = _velocity * Time.fixedDeltaTime;
        Vector2 alongGroundMove = new Vector2(_groundNormal.y, -_groundNormal.x);
        Vector2 move = alongGroundMove * positionDelta.x;
        Move(move, false);

        move = Vector2.up * positionDelta.y;
        Move(move, true);
    }

    private void Move(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (distance > _minMoveDistance)
        {
            int count = _rigidbody.Cast(move, _contactFilter, _hitBuffer, distance + _shellRadius);

            for (int i = 0; i < count; i++)
            {
                Vector2 currentNormal = _hitBuffer[i].normal;

                if (currentNormal.y > _minGroundNormal)
                {
                    _grounded = true;

                    if (yMovement)
                    {
                        _groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(_velocity, currentNormal);

                if (projection < 0)
                {
                    _velocity -= projection * currentNormal;
                }

                float modifiedDistance = _hitBuffer[i].distance - _shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }

        _rigidbody.position += move.normalized * distance;
    }

    enum MoveState
    {
        Idle,
        Run,
        Jump
    }
}
