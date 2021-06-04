using UnityEngine;

public class EnemyController : MonoBehaviour {
    public float maxOffset;
    public float enemyPatrolTime;

    private int _direction = 1;

    private Rigidbody2D _enemyBody;
    private float _originalX;
    private Vector2 _velocity;

    // Start is called before the first frame update
    private void Start() {
        _enemyBody = GetComponent<Rigidbody2D>();
        _originalX = transform.position.x;
        ComputeVelocity();
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

    private void ComputeVelocity() {
        _velocity = new Vector2(_direction * maxOffset / enemyPatrolTime, 0);
    }

    private void MoveEnemy() {
        _enemyBody.MovePosition(_enemyBody.position + _velocity * Time.fixedDeltaTime);
    }
}