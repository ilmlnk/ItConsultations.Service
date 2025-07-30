using Microsoft.AspNetCore.SignalR;

namespace ItConsultations.WebApi.Hubs;

public class VideoSignalingHub : Hub
{
    public async Task SendOffer(string connectionId, string sdp)
    {
        await Clients.Client(connectionId).SendAsync("ReceiveOffer", Context.ConnectionId, sdp);
    }

    public async Task SendAnswer(string connectionId, string sdp)
    {
        await Clients.Client(connectionId).SendAsync("ReceiveAnswer", Context.ConnectionId, sdp);
    }

    public async Task SendIceCanditate(string connectionId, string candidate)
    {
        await Clients.Client(connectionId).SendAsync("ReceiveIceCandidate", Context.ConnectionId, candidate);
    }
}
