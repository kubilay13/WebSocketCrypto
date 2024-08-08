using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;

namespace WebSocketAPI.SignalR
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CryptoHub : Hub
    {
        public async Task SubscribeToSymbol(string symbol)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, symbol);
            Console.WriteLine($"Client with connection ID {Context.ConnectionId} subscribed to symbol {symbol}");
        }  
        public async Task UnsubscribeFromSymbol(string symbol)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, symbol);
            Console.WriteLine($"Client with connection ID {Context.ConnectionId} unsubscribed to symbol {symbol}");
        }
        public async Task BroadcastPriceUpdate(string symbol, decimal price)
        {
            await Clients.Group(symbol).SendAsync("ReceivePriceUpdate", symbol, price);
        }
       // await _hubContext.Clients.All.SendAsync("ReceivePriceUpdate", "BTCUSDT", dateTime.ToString(), price);

    }
}
