using UnityEngine;

namespace TwinTravelers.Core.Utility
{
    /// <summary>
    /// 한번의 애니메이션 재생 뒤 자동으로 삭제되는 오브젝트를 정의하는 클래스
    /// </summary>
    public class Effect : MonoBehaviour
    {
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            float time = animator.GetCurrentAnimatorStateInfo(0).length;
            Destroy(gameObject, time);
        }
    }
}
