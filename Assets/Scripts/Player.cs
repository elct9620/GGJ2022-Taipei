using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    private Vector2 boundBox = new Vector2(0.5f, 0.5f);
    private MoveDirection _direction = MoveDirection.None;
    private Vector3 nextPosition = Vector3.zero;

    public bool IsReverse;
    public Animator animator;

    public AudioSource SFX;
    public AudioClip MoveSFX;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    /**
     * Player Actions
     *
     * Before Move - Check Movable
     * Move
     * After Move - Check Event Trigger 
     */
    private void Update()
    {
        SetNextPosition();
        TryMove();
        EnterTile();
    }

    private void SetNextPosition()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (IsReverse)
            {
                SetRight();
            }
            else
            {
                SetLeft();
            }

            return;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SetUp();
            return;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (IsReverse)
            {
                SetLeft();
            }
            else
            {
                SetRight();
            }

            return;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SetDown();
            return;
        }

        _direction = MoveDirection.None;
    }

    void SetLeft()
    {
        nextPosition = transform.position + Vector3.left;
        _direction = MoveDirection.Left;
    }

    void SetUp()
    {
        nextPosition = transform.position + Vector3.up;
        _direction = MoveDirection.Up;
    }

    void SetRight()
    {
        nextPosition = transform.position + Vector3.right;
        _direction = MoveDirection.Right;
    }

    void SetDown()
    {
        nextPosition = transform.position + Vector3.down;
        _direction = MoveDirection.Down;
    }


    void TryMove()
    {
        if (_direction == MoveDirection.None)
        {
            return;
        }

        animator.SetTrigger("isMove");
        animator.SetInteger("direction", (int)_direction);

        if (HasCollider("Wall"))
        {
            _direction = MoveDirection.None;
            return;
        }

        LeaveTile();
        if (MoveSFX)
        {
            SFX.PlayOneShot(MoveSFX);
        }

        transform.position = nextPosition;
    }

    void EnterTile()
    {
        if (_direction == MoveDirection.None)
        {
            return;
        }

        Collider2D[] colliders = GetCollidersBy("Trigger");
        foreach (var collider in colliders)
        {
            var tile = collider.GetComponent<Tile>();
            tile.OnTileEnter();
        }
    }

    void LeaveTile()
    {
        Collider2D[] colliders = GetCollidersBy("Trigger");
        foreach (var collider in colliders)
        {
            var tile = collider.GetComponent<Tile>();
            tile.OnTileLeave();
        }
    }

    Collider2D[] GetCollidersBy(string type)
    {
        var point = new Vector2(transform.position.x, transform.position.y);
        return Physics2D.OverlapBoxAll(point, boundBox, 0, layerMask: LayerMask.GetMask(type));
    }

    Collider2D[] GetNextCollidersBy(string type)
    {
        var point = new Vector2(nextPosition.x, nextPosition.y);
        return Physics2D.OverlapBoxAll(point, boundBox, 0, layerMask: LayerMask.GetMask(type));
    }

    bool HasCollider(string type)
    {
        if (GetNextCollidersBy("Wall").Length > 0)
        {
            return true;
        }

        return false;
    }
}