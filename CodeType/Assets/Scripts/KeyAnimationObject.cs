using UnityEngine;

public class KeyAnimatedObject : MonoBehaviour
{
    private Animator animator;
    public string triggerName = "Play";

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Play()
    {
        animator.SetTrigger(triggerName);
    }
}