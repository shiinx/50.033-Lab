using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private float _originalX;
    public float maxOffset;
    public float enemyPatrolTime;

    private int _direction = 1;
    private Vector2 _velocity;

    private Rigidbody2D _enemyBody;
    
    // Start is called before the first frame update
    private void Start() {
        _enemyBody = GetComponent<Rigidbody2D>();
        _originalX = transform.position.x;
        ComputeVelocity();
    }

    private void ComputeVelocity() {
        _velocity = new Vector2(_direction * maxOffset / enemyPatrolTime, 0);
    }

    private void MoveEnemy() {
        _enemyBody.MovePosition(_enemyBody.position + _velocity * Time.fixedDeltaTime);
    }
    

    private void FixedUpdate() {
        if (Mathf.Abs(_enemyBody.position.x - _originalX) < maxOffset) {
            MoveEnemy();
        }
        else {
            _direction *= -1;
            ComputeVelocity();
            MoveEnemy();
        }
    }
}
