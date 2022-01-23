using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace DefaultNamespace
{
    public class CreditScene : MonoBehaviour
    {
        public void Back()
        {
            SceneManager.LoadScene("Title");
        }
    }
}