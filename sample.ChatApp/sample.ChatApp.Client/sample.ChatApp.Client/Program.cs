using Grpc.Net.Client;
using MagicOnion.Client;
using sample.ChatApp.Client;

// argsからmemberIdを取得
string[] arguments = Environment.GetCommandLineArgs();
string memberId = arguments[1];
Console.WriteLine($"memberId=\"{memberId}\"");

// client
ChatComponent client = new ChatComponent();

// connect
await client.ReconnectInitializedServer(memberId);

// join to room
// - RoomName:"SampleRoom", UserName:上記指定のmemberId
await client.JoinOrLeave();

// chat loop
while (true)
{
    string? line = Console.ReadLine();

    if(line == "exit")
    {
        break;
    }
    else if (line == "report")
    {
        await client.SendReport($"this is report from {memberId}.");
    }
    else if (line == "exception")
    {
        await client.GenerateException();
    }
    else if (line is not null)
    {
        await client.SendMessage(line);
    }
}

// disconnect
// - 内部でJoinOrLeaveが呼ばれている
await client.DisconnectServer();