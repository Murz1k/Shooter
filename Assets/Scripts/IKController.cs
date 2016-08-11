using UnityEngine;
using System.Collections;

public class IKController : MonoBehaviour
{
    public Transform Head;
    public Transform Left;
    public Transform Right;

    public Transform Target;

    public bool isIkActive;
    public bool isGroup;

    Animator _anim;
    // Use this for initialization
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnAnimatorIK(int layerIndex)
    {
        if (!_anim)
            return;

        if (isIkActive)
        {
            if (!Head)
                return;

            _anim.SetLookAtWeight(1);
            _anim.SetLookAtPosition(Target.position);
            Head.LookAt(Target);

            if (!Left)
                return;
            _anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            _anim.SetIKPosition(AvatarIKGoal.RightHand, Right.position);

            _anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0.3f);
            _anim.SetIKPosition(AvatarIKGoal.LeftHand, Left.position);
        }

        if (isGroup)
        {
            Left.SetParent(Head);
            Right.SetParent(Head);
        }
    }
}
