using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomController : MonoBehaviour {
    public float speed;
    private Rigidbody2D _mushroomBody;
    private bool _onGround;

    private bool _collidedWPlayer;

    private bool _movingRight= true;

    private bool _launched = false;
    // Start is called before the first frame update
    void Start()
    {
        _mushroomBody = GetComponent<Rigidbody2D>();
        _mushroomBody.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        _mushroomBody.AddForce(Vector2.right * 10, ForceMode2D.Impulse);
        _launched = true;
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void FixedUpdate() {
        if (!_launched || _collidedWPlayer) return;
        if (_movingRight) {
            _mushroomBody.velocity = new Vector2(speed, _mushroomBody.velocity.y);
        }else {
            _mushroomBody.velocity = new Vector2(-speed, _mushroomBody.velocity.y);
        }
    }

    private void ModifyPhysics() {
        if (_onGround && !_collidedWPlayer) {
            _mushroomBody.gravityScale = 0;
            _mushroomBody.drag = 0;
        }else {
            _mushroomBody.gravityScale = 5;
            _mushroomBody.drag = 1.5f;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (!_collidedWPlayer && other.gameObject.CompareTag("Player")) {
            print("hit player");
            _collidedWPlayer = true;
            _mushroomBody.velocity = Vector2.zero;
            return;
        }
        
        Vector2 dir = other.GetContact(0).normal;
        if (dir == Vector2.up) {
            if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Platform") ||
                other.gameObject.CompareTag("Brick")) {
                print("hit gnd plat brick");
                _onGround = true;
            }     
        }else {
            if (other.gameObject.CompareTag("Obstacles")) {
                print("hit obstacles");
                _movingRight = !_movingRight;
            }
        }
        ModifyPhysics();
    }
    
    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.CompareTag("Brick") || other.gameObject.CompareTag("Platform")) {
            print("exit brick or plat");
            _onGround = false;
        }
        ModifyPhysics();
    }
}
