using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellController : MonoBehaviour
{
    
    public int[] position;                              //Position of the cell
    public int currentRl = 0;                           //The rule that was last applied to the cell

    private SpriteRenderer _spriteRenderer;             //Reference to the cell's SpriteRenderer
    private int _state = 0;                             //state of the cell 1 - on, 0 - off    
    private float age = 0;                              //Age of the tile
    private int mode = 0;                               //Used for colour mode settings

    //Those colours are used when the rule colour mode is on
    Color[] col = new Color[]
    {
        Color.blue,
        Color.green,
        Color.black,
        Color.white
    };
    public void CreateCell(int[] pos, Sprite newSpr) {
        _state = 0;
        position = pos;
        //Set game objects position
        this.transform.position = new Vector2(position[0], position[1]);
        //Add the box collider used for the mouse raycast
        this.gameObject.AddComponent<BoxCollider>();
        //Add the SprieRenderer to the object for it to be seen in the scene
        this.gameObject.AddComponent<SpriteRenderer>();
        _spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        //Set cell's sprite
        SetSprite(newSpr);
    }
    public void SetState(int newStatus, int rules) {
        _state = newStatus;
        currentRl = rules;
        ChangeColour();
    }

    public int GetState() { return _state;  }

    void SetSprite(Sprite newSpr) {
        _spriteRenderer.sprite = newSpr;
        _spriteRenderer.color = new Color(_state, _state, _state);
    }
    public void SwitchState() {
        _state = _state == 0 ? 1 : 0;
        _spriteRenderer.color = new Color(_state, _state, _state);
    }

    ///<summary>
    ///Colouring modes
    ///0 - Colour of the cell is based on the rule applied
    ///1 - Colour based on age is aplied to cells when they are alive
    ///2 - Colour based on age is aplied to cells when they are alive, the intensity of the colour is bigger for the cells that are alive so they are more visible
    ///3 - Colour is based on the current state 1 - white, 0 - black
    ///</summary>
    public void ChangeColour() {
        switch (mode) {
            case 0:
                _spriteRenderer.color = new Color(_state, _state, _state);
                break;
            case 1:
                if (_state == 1) {
                    age += 0.03f;
                }
                _spriteRenderer.color = new Color(0, age, age);
                break;
            case 2:
                if (_state == 1) {
                    age += 0.03f;
                    _spriteRenderer.color = new Color(0, age + _state, age + _state);
                } else {
                    //age -= 0.01f;
                    _spriteRenderer.color = new Color(0, age, age);
                }
                break;
            case 3:
                _spriteRenderer.color = col[currentRl];
                break;
        }
    }
    public void SetMode(int m) {
        mode = m;
        ChangeColour();
    }
}
