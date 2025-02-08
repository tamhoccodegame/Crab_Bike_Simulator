using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stripper : MonoBehaviour
{
    public List<Animator> dancers; // Danh sách animator của các dancer
    private List<int> usedAnimations = new List<int>(); // Danh sách animation đang được sử dụng
    private int totalAnimations = 8; // Giả sử có 5 animation nhảy múa khác nhau

    void Start()
    {
        AssignRandomDance(); // Khởi tạo lần đầu tiên
    }

    void AssignRandomDance()
    {
        usedAnimations.Clear(); // Xóa danh sách các animation đã sử dụng

        foreach (Animator dancer in dancers)
        {
            StartCoroutine(AssignDance(dancer));
        }
    }

    IEnumerator AssignDance(Animator dancer)
    {
        while (true)
        {
            int randomAnimation;

            // Chọn animation sao cho không bị trùng lặp với dancer khác
            do
            {
                randomAnimation = Random.Range(0, totalAnimations);
            } while (usedAnimations.Contains(randomAnimation));

            usedAnimations.Add(randomAnimation);

            // Gán animation cho dancer
            dancer.SetInteger("DanceIndex", randomAnimation);

            // Chờ animation hiện tại hoàn thành
            yield return new WaitForSeconds(GetAnimationLength(dancer, randomAnimation));

            // Xóa animation này khỏi danh sách để dancer khác có thể sử dụng
            usedAnimations.Remove(randomAnimation);
        }
    }

    float GetAnimationLength(Animator animator, int danceIndex)
    {
        // Lấy thông tin clip dựa trên danceIndex
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.length;
    }
    

}
