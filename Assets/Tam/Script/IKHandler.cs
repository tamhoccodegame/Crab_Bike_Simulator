using UnityEngine;

public class IKHandler : MonoBehaviour
{
	public Animator animator;

	// IK Targets
	public Transform leftHandTarget;
	public Transform rightHandTarget;

	void OnAnimatorIK(int layerIndex)
	{
		if (animator)
		{
			// Enable IK for hands
			animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
			animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);

			animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
			animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);

			// Set positions and rotations for hands
			if (leftHandTarget != null)
			{
				animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
				animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);
			}

			if (rightHandTarget != null)
			{
				animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);
				animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);
			}
		}
	}

}
