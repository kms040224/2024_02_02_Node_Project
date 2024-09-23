using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Text;
using Unity.VisualScripting;

public class UnityToNode : MonoBehaviour
{
    public Button btnPostExample;
    public Button btnGetExample;
    public Button btnResDataExample;
    public string host;
    public string Port;
    public string idUrl;
    public string postUrl;
    public string resUrl;
    public int id;
    public string data;

    public void Start()
    {

        this.btnResDataExample.onClick.AddListener(() =>
        {
            var url = string.Format("{0}:{1}/{2}", host, Port, resUrl);

            StartCoroutine(this.GetData(url, (raw)) =>
            {
                var res = JsonConvert.DeserializeObject<Protocols.Packets.res_data>(raw);

                foreach(var user in res.result)
                {
                    Debug.LogFormat("{0}:{1}", user.id, user.data);
                }
            }));
        });


        this.btnPostExample.onClick.AddListener(() =>
        {
            var url = string.Format("{0}:{1}/{2}", host, Port, postUrl);
            Debug.Log(url);
            var req = new Protocols.Packets.req_data();
            req.cmd = 1000;
            req.id = id;
            req.data = data;

            var json = JsonConvert.SerializeObject(req);                //클래스 -> Json

            StartCoroutine(this.PostData(url, json, (raw) =>
            {
                Protocols.Packets.common res = JsonConvert.DeserializeObject<Protocols.Packets.common>(raw);
                Debug.LogFormat("{0}:{1}", res.cmd, res.message);
            }));
        });


        this.btnGetExample.onClick.AddListener(() =>
        {
            var url = string.Format("{0}:{1}/{2}", host, Port, idUrl);

            Debug.Log(url);
            StartCoroutine(this.GetData(url, (raw) =>
            {
                var res = JsonConvert.DeserializeObject<Protocols.Packets.common>(raw);
                Debug.LogFormat("{0}, {1}" , res.cmd, res.message);
            }));
        });
            
    }

    private IEnumerator GetData(string url, System.Action<string> callback)
    {
        var webRequest = UnityWebRequest.Get(url);
        yield return webRequest.SendWebRequest();

        Debug.Log("Get : " + webRequest.downloadHandler.text);
        if(webRequest.result == UnityWebRequest.Result.ConnectionError
            || webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("네트워크 연결이 좋지 않아 통신 불가");
        }
    }

    private IEnumerator PostData(string url, string json,System.Action<string> callback)
    {
        var webRequest = new UnityWebRequest(url, "POST");
        var bodyRaw = Encoding.UTF8.GetBytes(json);             //직렬화

        webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        yield return webRequest.SendWebRequest();

        Debug.Log("Get : " + webRequest.downloadHandler.text);
        if (webRequest.result == UnityWebRequest.Result.ConnectionError
            || webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("네트워크 연결이 좋지 않아 통신 불가");
        }
        else
        {
            callback(webRequest.downloadHandler.text);
        }
        webRequest.Dispose();
    }
}
