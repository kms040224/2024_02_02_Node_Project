using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protocols : MonoBehaviour
{
    // Start is called before the first frame update
   public class Packets
    {

        public class common
        {
            public int cmd;                     //��� ���� ǥ��
            public string message;              //�޼���
        }

        public class req_data : common
        {
            public int id;                      //id�� �޾Ƽ� �Ѵ�
            public string data;                 //���� ������
        }

        public class res_data : common
        {
            public req_data[] result;                       //list or Array ���� �޴´�
        }
    }
}
