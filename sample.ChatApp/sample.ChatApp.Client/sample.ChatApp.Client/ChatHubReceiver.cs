using System;
using sample.ChatApp.Shared.Hubs;
using sample.ChatApp.Shared.MessagePackObjects;
using sample.ChatApp.Shared.Services;
using Grpc.Core;
using MagicOnion.Client;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MagicOnion;
using Grpc.Net.Client;

namespace sample.ChatApp.Client
{
    public class ChatComponent : IChatHubReceiver
    {
        private CancellationTokenSource shutdownCancellation = new CancellationTokenSource();
        private ChannelBase channel;
        private IChatHub streamingClient;
        private IChatService client;

        private bool isJoin;
        private bool isSelfDisConnected;

        public string ChatName { get; private set; }


        public async Task Destroy()
        {
            // Clean up Hub and channel
            shutdownCancellation.Cancel();

            if (this.streamingClient != null) await this.streamingClient.DisposeAsync();
            if (this.channel != null) await this.channel.ShutdownAsync();
        }


        private async Task InitializeClientAsync()
        {
            // Initialize the Hub
            // NOTE: If you want to use SSL/TLS connection, see InitialSettings.OnRuntimeInitialize method.
            this.channel = GrpcChannel.ForAddress("https://localhost:7077");

            while (!shutdownCancellation.IsCancellationRequested)
            {
                try
                {
                    Console.WriteLine($"[Receiver][InitializeClientAsync] Connecting to the server...");
                    this.streamingClient = await StreamingHubClient.ConnectAsync<IChatHub, IChatHubReceiver>(this.channel, this, cancellationToken: shutdownCancellation.Token);
                    this.RegisterDisconnectEvent(streamingClient);
                    Console.WriteLine($"[Receiver][InitializeClientAsync] Connection is established.");
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                Console.WriteLine($"[Receiver][InitializeClientAsync] Failed to connect to the server. Retry after 5 seconds...");
                await Task.Delay(5 * 1000);
            }

            this.client = MagicOnionClient.Create<IChatService>(this.channel);
        }

        private async void RegisterDisconnectEvent(IChatHub streamingClient)
        {
            try
            {
                // you can wait disconnected event
                await streamingClient.WaitForDisconnect();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                // try-to-reconnect? logging event? close? etc...
                Console.WriteLine($"[Receiver][RegisterDisconnectEvent] disconnected from the server.");

                if (this.isSelfDisConnected)
                {
                    // there is no particular meaning
                    await Task.Delay(2000);

                    // reconnect
                    await this.ReconnectServerAsync();
                }
            }
        }


        public async Task DisconnectServer()
        {
            this.isSelfDisConnected = true;

            if (this.isJoin)
                await this.JoinOrLeave();

            await this.streamingClient.DisposeAsync();
        }

        public async Task ReconnectInitializedServer(string chatName)
        {
            this.ChatName = chatName;

            if (this.channel != null)
            {
                var chan = this.channel;
                if (chan == Interlocked.CompareExchange(ref this.channel, null, chan))
                {
                    await chan.ShutdownAsync();
                    this.channel = null;
                }
            }
            if (this.streamingClient != null)
            {
                var streamClient = this.streamingClient;
                if (streamClient == Interlocked.CompareExchange(ref this.streamingClient, null, streamClient))
                {
                    await streamClient.DisposeAsync();
                    this.streamingClient = null;
                }
            }

            if (this.channel == null && this.streamingClient == null)
            {
                await this.InitializeClientAsync();
            }
        }


        private async Task ReconnectServerAsync()
        {
            Console.WriteLine($"[Receiver][ReconnectServerAsync] Reconnecting to the server...");
            this.streamingClient = await StreamingHubClient.ConnectAsync<IChatHub, IChatHubReceiver>(this.channel, this);
            this.RegisterDisconnectEvent(streamingClient);
            Console.WriteLine("[Receiver][ReconnectServerAsync] Reconnected.");

            this.isSelfDisConnected = false;
        }


        #region Client -> Server (Streaming)
        public async Task JoinOrLeave()
        {
            if (this.isJoin)
            {
                await this.streamingClient.LeaveAsync();
            }
            else
            {
                var request = new JoinRequest { RoomName = "SampleRoom", UserName = ChatName };
                await this.streamingClient.JoinAsync(request);

                this.isJoin = true;
            }
        }


        public async Task SendMessage(string message)
        {
            if (!this.isJoin)
                return;

            await this.streamingClient.SendMessageAsync(message);
        }

        public async Task GenerateException()
        {
            // hub
            if (!this.isJoin) return;
            await this.streamingClient.GenerateException("[Receiver][GenerateException] client exception(streaminghub)!");
        }

        public void SampleMethod()
        {
            throw new System.NotImplementedException();
        }
        #endregion


        #region Server -> Client (Streaming)
        public void OnJoin(string name)
        {
            Console.WriteLine($"[Receiver][OnJoin] {name} entered the room.");
        }


        public void OnLeave(string name)
        {
            Console.WriteLine($"[Receiver][OnLeave] {name} left the room.");
        }

        public void OnSendMessage(MessageResponse message)
        {
            Console.WriteLine($"[Receiver][OnSendMessage] {message.UserName}：{message.Message}");
        }
        #endregion


        #region Client -> Server (Unary)
        public async Task SendReport(string reportMessage)
        {
            await this.client.SendReportAsync(reportMessage);
        }

        public async void UnaryGenerateException()
        {
            // unary
            await this.client.GenerateException("[Receiver][UnaryGenerateException] client exception(unary)！");
        }
        #endregion
    }
}
