using UnityEngine;

public class MutationsTree : MonoBehaviour
{
    private bool isSkywardLeapSkillUnlocked = false;
    private bool isSwiftStrideUnlocked = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            UnlockSkywardLeapSkill();

        if (Input.GetKeyDown(KeyCode.Alpha2))
            UnlockSwiftStride();
    }

    public void UnlockSkywardLeapSkill()
    {
        if (isSkywardLeapSkillUnlocked)
        {
            return;
        }

        isSkywardLeapSkillUnlocked = true;
        PlayerController.Player.GetComponent<PlayerController>().maxJumpTime *= 1.8f;
    }

    public void UnlockSwiftStride()
    {
        if (isSwiftStrideUnlocked)
        {
            return;
        }

        isSwiftStrideUnlocked = true;
        PlayerController.Player.GetComponent<PlayerController>().walkSpeed *= 1.3f;
        PlayerController.Player.GetComponent<PlayerAnimator>().WalkAnimationMulti *= 1.3f;
    }

    private void UnlockSkill()
    {

    }
}
