using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Should not use MonoBehaviour, try scriptable objects....
public class TetrominoSprite : MonoBehaviour
{
    [SerializeField]
    private Tetromino tetromino;
    public Tetromino Tetromino { get => tetromino; }
    [SerializeField]
    private Sprite sprite;
    public Sprite Sprite { get => sprite; }
}
