using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PhysicMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _gravityModifier;
    [SerializeField] private float _minGroundNormal;

    private Rigidbody2D _rigidbody;
    private Vector2 _velocity;
    private Vector2 _targetVelocity;
    private Vector2 _groundNormal;
    private bool _grounded;
    private ContactFilter2D _contactFilter;
    private readonly RaycastHit2D[] _hitBuffer = new RaycastHit2D[16];

    private const float MinMoveDistance = 0.001f;
    private const float ShellRadius = 0.01f;

    public bool Grounded => _grounded;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();

        _contactFilter.useTriggers = false;
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

    public void Jump()
    {
        _velocity.y = _jumpForce;
    }

    public void SetTargetVelocity(Vector2 targetVelocity)
    {
        _targetVelocity = targetVelocity;
    }

    private void Move(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (distance > MinMoveDistance)
        {
            int count = _rigidbody.Cast(move, _contactFilter, _hitBuffer, distance + ShellRadius);

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

                float modifiedDistance = _hitBuffer[i].distance - ShellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }

        _rigidbody.position += move.normalized * distance;
    }
}
