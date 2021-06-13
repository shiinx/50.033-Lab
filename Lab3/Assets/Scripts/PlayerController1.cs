using UnityEngine;
using UnityEngine.UI;

public class PlayerController1 : MonoBehaviour {
    public float speed;
    public float upSpeed;
    public float maxSpeed;
    public float linearDrag;
    public float gravity;
    public Transform enemyLocation;
    public Text scoreText;
    public Button startButton;
    private bool _countScoreState;
    private bool _faceRightState = true;
    private Rigidbody2D _marioBody;
    private SpriteRenderer _marioSprite;

    private bool _onGroundState = true;
    private int _score;

    // Start is called before the first frame update
    private void Start() {
        Application.targetFrameRate = 30;
        _marioBody = GetComponent<Rigidbody2D>();
        _marioSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update() {
        if (!_onGroundState && _countScoreState) {
            if (Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f) {
                _countScoreState = false;
                _score++;
            }
        }
    }

    private void FixedUpdate() {
        var moveHorizontal = Input.GetAxis("Horizontal");
        var marioVelocity = _marioBody.velocity;
        MoveMario(moveHorizontal, marioVelocity);

        if (Input.GetKeyDown("space") && _onGroundState) {
            Jump();
            _countScoreState = true;
        }

        ModifyDrag(moveHorizontal, marioVelocity);
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Ground")) {
            _onGroundState = true;
            _countScoreState = false;
            scoreText.text = "Score: " + _score;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Enemy")) {
            Debug.Log("Collided with Goomba!");
            Time.timeScale = 0.0f;
            startButton.gameObject.SetActive(true);
        }
    }

    private void Jump() {
        _marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
        _onGroundState = false;
    }

    private void MoveMario(float moveHorizontal, Vector2 marioVelocity) {
        // move by add force
        var movement = new Vector2(moveHorizontal, 0);
        _marioBody.AddForce(movement * speed);

        // change sprite looking direction
        if (moveHorizontal < 0 && _faceRightState) {
            _faceRightState = false;
            _marioSprite.flipX = true;
        }
        else if (moveHorizontal > 0 && !_faceRightState) {
            _faceRightState = true;
            _marioSprite.flipX = false;
        }

        // set max speed 
        if (Mathf.Abs(marioVelocity.x) > maxSpeed) {
            _marioBody.velocity = new Vector2(Mathf.Sign(marioVelocity.x) * maxSpeed, marioVelocity.y);
        }
    }

    private void ModifyDrag(float moveHorizontal, Vector2 marioVelocity) {
        var changingDirections = moveHorizontal > 0 && marioVelocity.x < 0 || moveHorizontal < 0 && marioVelocity.x > 0;

        if (_onGroundState) {
            if (Mathf.Abs(moveHorizontal) < 0.4f || changingDirections) {
                _marioBody.gravityScale = 1;
                _marioBody.drag = linearDrag;
            }
            else {
                _marioBody.gravityScale = 0f;
                _marioBody.drag = 0f;
            }
        }
        else {
            _marioBody.gravityScale = gravity;
            _marioBody.drag = linearDrag * 0.15f;
        }
    }
}