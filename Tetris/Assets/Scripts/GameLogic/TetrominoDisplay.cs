using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TetrominoDisplay : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private TetrominoSprite[] tetrominoSprites;

    [SerializeField]
    private TetrominoStorer tetrominoStorer;
     

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        tetrominoStorer.TetrominoChanged += Display;
    }

    private void Display(Tetromino tetromino)
    {
        TetrominoSprite tetrominoSprite = Array.
            Find(tetrominoSprites, element => element.Tetromino.name == tetromino.name);
        spriteRenderer.sprite = tetrominoSprite == null ? null : tetrominoSprite.Sprite;
    }

    private void OnDisable()
    {
        tetrominoStorer.TetrominoChanged -= Display;
    }

}
