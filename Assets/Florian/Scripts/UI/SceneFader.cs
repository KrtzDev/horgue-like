using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SceneFader : MonoBehaviour
{
    private Animator _sceneFaderAnimator;

    private void Awake()
    {
        _sceneFaderAnimator = GetComponent<Animator>();
    }

    public float FadeOut()
    {
        _sceneFaderAnimator.Play("FadeOut");
        return _sceneFaderAnimator.GetCurrentAnimatorStateInfo(0).length;
    }

    public float FadeIn()
    {
        _sceneFaderAnimator.Play("FadeIn");
        return _sceneFaderAnimator.GetCurrentAnimatorStateInfo(0).length;
    }
}
