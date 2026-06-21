using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
    

    void Start()
    {
        Animator animator = GetComponent<Animator>();
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        Destroy(gameObject, state.length);
    }
}