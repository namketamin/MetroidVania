using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GroundType { Grass,Dirt}
public class GroundCheck : MonoBehaviour
{
    PlayerController player;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask interactableLayer;
    BoxCollider2D col;
    private void Awake()
    {
        player=GetComponentInParent<PlayerController>();
        col = GetComponent<BoxCollider2D>();
    }
    private void Start()
    {
        
    }
    public bool IsGrounded()
    {
        Vector2 size = new Vector2(col.bounds.size.x * 0.9f, col.bounds.size.y);
        RaycastHit2D hit = Physics2D.BoxCast(
            col.bounds.center,
            size,
            0f,
            Vector2.down,
            0.05f,
            groundLayer|interactableLayer
        );
        if(!hit) return false;
        return true;
    }
    public GroundType GetGroundType()
    {
        Vector2 size = new Vector2(col.bounds.size.x*0.9f, 0.1f);
        RaycastHit2D hit = Physics2D.BoxCast(
            col.bounds.center,
            size,
            0f,
            Vector2.down,
            0.2f,
            groundLayer
        );
        if(hit.collider == null ) return GroundType.Dirt;
        TilemapGround tilemapGround =hit.collider.GetComponent<TilemapGround>();
        return tilemapGround.groundType;
    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Interactable"))
    //    {
    //        player.SetGrounded(true);
    //    }
    //}
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Interactable"))
    //    {
    //        player.SetGrounded(false);
    //    }
    //}
}
