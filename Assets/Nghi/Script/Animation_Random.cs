using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Random : MonoBehaviour
{
    public List<Animator> people; // Danh sách animator 
    private List<int> usedAnimations = new List<int>(); // Danh sách animation đang được sử dụng
    [SerializeField] private int totalAnimations; // Giả sử có 5 animation khác nhau


    void OnEnable()
    {
        AssignRandomDance(); // Khởi tạo lần đầu tiên
    }

    void AssignRandomDance()
    {
        usedAnimations.Clear(); // Xóa danh sách các animation đã sử dụng

        foreach (Animator person in people)
        {
            StartCoroutine(AssignDance(person));
        }
    }

    IEnumerator AssignDance(Animator people)
    {
        while (true)
        {
            int randomAnimation;

            // Chọn animation sao cho không bị trùng lặp với dancer khác
            do
            {
                randomAnimation = Random.Range(1, totalAnimations+1);
            } while (usedAnimations.Contains(randomAnimation));

            usedAnimations.Add(randomAnimation);

            // Gán animation cho dancer
            people.SetInteger("Index", randomAnimation);

            // Chờ animation hiện tại hoàn thành
            yield return new WaitForSeconds(GetAnimationLength(people, randomAnimation));

            // Xóa animation này khỏi danh sách để dancer khác có thể sử dụng
            usedAnimations.Remove(randomAnimation);
        }
    }

    float GetAnimationLength(Animator animator, int index)
    {
        // Lấy thông tin clip dựa trên danceIndex
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.length;
    }

    private void OnDisable()
    {
        GetComponent<Animator>().SetInteger("index", 0);
        StopAllCoroutines();
    }
}
