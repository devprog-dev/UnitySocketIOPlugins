using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using SocketIOClient;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

//using Google.Protobuf;

public class Client : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(" Game Start Client ");
        var uri = new Uri("http://localhost:3000/");

        var socket = new SocketIO(uri, new SocketIOOptions
        {
            Query = new Dictionary<string, string>
                {
                    {"token", "V3" }
                },
        });


        socket.OnConnected += Socket_OnConnected;
        socket.OnPing += Socket_OnPing;
        socket.OnPong += Socket_OnPong;
        socket.OnDisconnected += Socket_OnDisconnected;
        socket.OnReconnectAttempt += Socket_OnReconnecting;
        socket.OnAny((name, response) =>
        {
            Debug.Log(name);
            Debug.Log(response);
        });
        //socket.On("Hell", response =>
        //{
        //    // Console.WriteLine(response.ToString());
        //    Console.WriteLine(response.GetValue<string>());
        //});

        socket.On("all_message", response =>
        {
            // Console.WriteLine(response.ToString());
            Debug.Log(response.GetValue<string>());
        });

        socket.ConnectAsync();
        
    }

    private  void Socket_OnReconnecting(object sender, int e)
    {        
        Debug.Log($"{DateTime.Now} Reconnecting: attempt = {e}");
    }

    private  void Socket_OnDisconnected(object sender, string e)
    {
        Debug.Log("disconnect: " + e);
    }

    private  async void Socket_OnConnected(object sender, EventArgs e)
    {
        Debug.Log("Socket_OnConnected");
        var socket = sender as SocketIO;
        Debug.Log("Socket.Id:" + socket.Id);

        //while (true)
        //{
        //    await Task.Delay(1000);
        await socket.EmitAsync("hi", DateTime.Now.ToString());
        //await socket.EmitAsync("welcome");
        //}
        //byte[] bytes = Encoding.UTF8.GetBytes("ClientCallsServerCallback_1Params_0");
        //await socket.EmitAsync("client calls the server's callback 1", bytes);
        //await socket.EmitAsync("1 params", Encoding.UTF8.GetBytes("hello world"));
    }

    private  void Socket_OnPing(object sender, EventArgs e)
    {
        Debug.Log("Ping");
    }

    private  void Socket_OnPong(object sender, TimeSpan e)
    {
        Debug.Log("Pong: " + e.TotalMilliseconds);
    }

    
}
