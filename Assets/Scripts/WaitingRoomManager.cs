using UnityEngine.SceneManagement;
using UnityEngine;

public class WaitingRoomManager : MonoBehaviour
{
    public void ChangeScene(int _sceneNum)
    {
        SceneManager.LoadScene(_sceneNum);
    }
}
