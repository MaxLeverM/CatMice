using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Lever.UI.Windows.Behaviours
{
    public class PlayerInListBehaviour : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerNickField;

        public void LoadData(Player playerData)
        {
            playerNickField.text = playerData.NickName;
        }

    }
}