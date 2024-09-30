using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using Newtonsoft.Json;      //Json ���̺귯�� �߰�
using System;           //Action<> ����� ���� ���ӽ����̽� �߰�


public class GameAPI : MonoBehaviour
{
    private string baseUrl = "https://lacalhost:4000/api";      //Node.Js ������ url

    //�÷��̾� ��� �ż���
    public IEnumerator RigisterPlayer(string playerName, string password)
    {
        var requestData = new { name = playerName, password = password };
        string jsonData = JsonConvert.SerializeObject(requestData);
        Debug.Log($"Registering Player:{jsonData})");

        using (UnityWebRequest request = new UnityWebRequest($"{baseUrl}/register", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log($"Error register player: {request.result}");
            }
            else
            {
                Debug.Log("Player registered successsfully");
            }
        }
    }

    //�÷��̾� �α��� �޼���
    public IEnumerator LoginPlayer(string playerName, string password, Action<PlayerModel> onSuccess)
    {
        var requestData = new { name = playerName, password = password };
        string jsonData = JsonConvert.SerializeObject(requestData);

        using (UnityWebRequest request = new UnityWebRequest($"{baseUrl}/login", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if(request.result != UnityWebRequest.Result.Success)            //���� ����
            {
                Debug.Log($"Error loging in : {request.error}");        //���� �α�
            }
            else
            {
                //������ ó���Ͽ� PlayerModel ����
                string responseBody = request.downloadHandler.text;

                try
                {
                    var responseData = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBody);

                    //���� ���信�� playerModel ����
                    PlayerModel playerModel = new PlayerModel(responseData["playerName"].ToString())
                    {
                        metal = Convert.ToInt32(responseData["metal"]),
                        crystal = Convert.ToInt32(responseData["crystal"]),
                        deuterium = Convert.ToInt32(responseData["deuterium"]),
                    };
                    onSuccess?.Invoke(playerModel); //playerModel ��ȯ
                    Debug.Log("Login successful");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error processing login response: {ex.Message}");
                }
            }
        }

    }
}
