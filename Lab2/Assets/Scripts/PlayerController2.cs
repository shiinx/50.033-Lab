using UnityEngine;

public class PlayerController2 : MonoBehaviour {
    public float speed;
    public float upSpeed;
    public float maxSpeed;
    public float linearDrag;
    public ContactFilter2D contactFilter;
    private bool _faceRightState = true;
    private Animator _marioAnimator;
    private AudioSource _marioAudio;
    private Rigidbody2D _marioBody;
    private SpriteRenderer _marioSprite;

    private Vector2 _movement;

    private bool _shouldJump;

    private bool IsGrounded => _marioBody.IsTouching(contactFilter);

    // Start is called before the first frame update
    private void Start() {
        Application.targetFrameRate = 30;
        _marioBody = GetComponent<Rigidbody2D>();
        _marioSprite = GetComponent<SpriteRenderer>();
        _marioAnimator = GetComponent<Animator>();
        _marioAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update() {
        _marioAnimator.SetBool("onGround", IsGrounded);
        if (Input.GetKeyDown(KeyCode.Space)) {
            _shouldJump = true;
        }

        _movement = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
    }

    private void FixedUpdate() {
        MoveMario(_movement);
        if (_shouldJump && IsGrounded) {
            Jump();
        }

        _shouldJump = false;
        ModifyPhysics(_movement);
    }

    private void MoveMario(Vector2 movement) {
        // move by add force
        _marioBody.AddForce(movement * speed);

        // change sprite looking direction
        if (movement.x < 0 && _faceRightState) {
            _faceRightState = false;
            _marioSprite.flipX = true;
            if (Mathf.Abs(_marioBody.velocity.x) > 1.0 && IsGrounded) {
                _marioAnimator.SetTrigger("onSkid");
            }
        }
        else if (movement.x > 0 && !_faceRightState) {
            _faceRightState = true;
            _marioSprite.flipX = false;
            if (Mathf.Abs(_marioBody.velocity.x) > 1.0 && IsGrounded) {
                _marioAnimator.SetTrigger("onSkid");
            }
        }

        // set max speed 
        if (Mathf.Abs(_marioBody.velocity.x) > maxSpeed) {
            _marioBody.velocity = new Vector2(Mathf.Sign(_marioBody.velocity.x) * maxSpeed, _marioBody.velocity.y);
        }

        _marioAnimator.SetFloat("xSpeed", Mathf.Abs(movement.x));
    }

    private void Jump() {
        _marioAudio.PlayOneShot(_marioAudio.clip);
        _marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
    }

    private void ModifyPhysics(Vector2 movement) {
        var velocity = _marioBody.velocity;
        var changingDirections = movement.x > 0 && velocity.x < 0 || movement.x < 0 && velocity.x > 0;

        if (IsGrounded) {
            if (Mathf.Abs(movement.x) < 0.4f || changingDirections) {
                _marioBody.drag = linearDrag * 0.15f;
            }
            else {
                _marioBody.drag = 0f;
            }
        }
        else {
            _marioBody.drag = linearDrag * 0.15f;
        }
    }
}