using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] AudioClip breakSound;
    [SerializeField] GameObject BlockVFX;
    [SerializeField] Sprite[] hitSprites;

    Level level;
    int timesHit;

    private void Start()
    {
        level = FindObjectOfType<Level>();

        CountBreakableBlocks();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.CompareTag("Breakable"))
        {
            HandleHit();
        }
    }

    private void HandleHit()
    {
        FindObjectOfType<GameSession>().AddToScore();

        timesHit++;
        int maxHits = hitSprites.Length + 1;
        if (timesHit >= maxHits)
        {
            DestroyBlock();
        }
        else
        {
            ShowNextHitSprite();
        }
    }

    private void ShowNextHitSprite()
    {
        int spriteIndex = timesHit - 1;
        if (hitSprites[spriteIndex] != null)
        {
            GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
        }
        else
        {
            Debug.LogError("Block sprite missing from array" + gameObject.name);
        }
    }

    private void DestroyBlock()
    {
        PlayBlockSFX();
        Destroy(gameObject);
        level.BlockDestroyed();
        TriggerSparkles();
    }

    private void PlayBlockSFX()
    {
        AudioSource.PlayClipAtPoint(breakSound, Camera.main.transform.position);
    }

    private void TriggerSparkles()
    {
        GameObject sparkles = Instantiate(BlockVFX, transform.position, transform.rotation);
        Destroy(sparkles, 2f); // Destroys particle system after 2 seconds
    }
    
    private void CountBreakableBlocks()
    {
        if (gameObject.CompareTag("Breakable"))
        {
            level.CountBlocks();
        }
    }
}
