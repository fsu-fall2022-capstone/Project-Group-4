using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

class UIScrollViewController : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private GameObject leftButton;
    [SerializeField] private GameObject rightButton;
    [SerializeField] private Text text;
    [SerializeField] private float scrollSpeed = 0.1f;
    [SerializeField] private string shopName;
    [SerializeField] private float distanceY = 0.1f;
    private bool isHidden = false;

    public void LeftButtonClick()
    {
        if (scrollRect.horizontalNormalizedPosition >= 0.05f)
        {
            scrollRect.horizontalNormalizedPosition -= scrollSpeed;
        }
    }

    public void RightButtonClick()
    {
        if (scrollRect.horizontalNormalizedPosition <= 0.95f)
        {
            scrollRect.horizontalNormalizedPosition += scrollSpeed;
        }
    }

    public void DisplayButtonClick()
    {
        if (isHidden)
        {
            text.text = $"Hide {shopName}";
            leftButton.SetActive(true);
            rightButton.SetActive(true);
            gameObject.transform.position += new Vector3(0, distanceY, 0);
            isHidden = false;
        }
        else
        {
            text.text = $"Show {shopName}";
            leftButton.SetActive(false);
            rightButton.SetActive(false);
            gameObject.transform.position -= new Vector3(0, distanceY, 0);
            isHidden = true;
        }
    }

}