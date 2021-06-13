using UnityEngine;

public class MushroomController : MonoBehaviour {
    public float speed;

    private bool _collidedWPlayer;

    private bool _launched = false;

    private bool _movingRight = true;
    private Rigidbody2D _mushroomBody;
    private bool _onGround;

    // Start is called before the first frame update
    void Start() {
        _mushroomBody = GetComponent<Rigidbody2D>();
        _mushroomBody.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        _launched = true;
    }


    private void FixedUpdate() {
        if (!_launched || _collidedWPlayer) return;
        var velocity = _mushroomBody.velocity;
        velocity = _movingRight ? new Vector2(speed, velocity.y) : new Vector2(-speed, velocity.y);
        _mushroomBody.velocity = velocity;
    }

    private void OnBecameInvisible() {
        Destroy(gameObject);
    }


    private void OnCollisionEnter2D(Collision2D other) {
        if (!_collidedWPlayer && other.gameObject.CompareTag("Player")) {
            print("hit player");
            _collidedWPlayer = true;
            _mushroomBody.velocity = Vector2.zero;
            return;
        }

        var dir = other.GetContact(0).normal;
        if (dir == Vector2.up) return;
        if (other.gameObject.CompareTag("Obstacles")) {
            _movingRight = !_movingRight;
        }
    }
}