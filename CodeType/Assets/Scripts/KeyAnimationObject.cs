using UnityEngine;

public class KeyAnimatedObject : MonoBehaviour
{
    private Animator animator;
    // Add this so you can type "OnQClick" or "OnWKey" in the Inspector for each button
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