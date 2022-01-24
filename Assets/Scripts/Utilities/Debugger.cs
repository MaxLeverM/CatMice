using TMPro;
using UnityEngine;

namespace Lever.Utilities
{
    public class Debugger : MonoBehaviour
    {
        [SerializeField] private TMP_Text debugText;

        public void Log()
        {
            Debug.developerConsoleVisible = true;
        }

        public void LogError()
        {
        }

        public void LogWarning()
        {
            
        }
    }
}