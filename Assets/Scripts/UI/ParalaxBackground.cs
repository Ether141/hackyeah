using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private GameObject cam;
    [SerializeField] private float parallaxEffect;
    [SerializeField] private bool followCameraY = true;
    [SerializeField] private Vector3 offset;

    private float length;
    private float startpos;
    private float startposY;

    private void Start()
    {
        startpos = transform.position.x;
        startposY = transform.position.y;
        length = GetComponent<SpriteRenderer>() ? GetComponent<SpriteRenderer>().bounds.size.x : 0f;
    }
    
    private void FixedUpdate()
    {
        float tmp = cam.transform.position.x * (1 - parallaxEffect);
        float dist = cam.transform.position.x * parallaxEffect;
        float distY = cam.transform.position.y * parallaxEffect;

        transform.position = new Vector3(startpos + dist, followCameraY ? startposY + distY : transform.position.y, transform.position.z) + offset;

        if (tmp > startpos + length)
            startpos += length;
        else if (tmp < startpos - length)
            startpos -= length;
    }
}