using TMPro;
using UnityEngine;

namespace Lever.UI.GameStatistic
{
    public class StatisticObject : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        private string nickName;
        private int score;

        public string NickNmae => nickName;
        public int Score => score;
        
        public void Init(string nickName, int score)
        {            
            this.nickName = nickName;
            this.score = score;
            text.text = $"{nickName} {this.score}";
        }

        public void Write(int score)
        {
            this.score = score;

            text.text = $"{nickName} {this.score}";
        }
    }
}