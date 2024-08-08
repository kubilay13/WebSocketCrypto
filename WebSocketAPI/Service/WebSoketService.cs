using Microsoft.AspNetCore.SignalR;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using WebSocketAPI.SignalR;

namespace WebSocketAPI.Service
{
    public class WebSoketService : BackgroundService
    {
        private readonly IHubContext<CryptoHub> _hubContext;

        public WebSoketService(IHubContext<CryptoHub> hubContext)
        {
            _hubContext = hubContext;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (ClientWebSocket clientWebSocketBTC = new ClientWebSocket())
            using (ClientWebSocket clientWebSocketETH = new ClientWebSocket())
            {
                Uri uriBTC = new Uri("wss://stream.binance.com:9443/ws/btcusdt@ticker");
                Uri uriETH = new Uri("wss://stream.binance.com:9443/ws/ethusdt@ticker");

                try
                {
                    await ConnectWebSocketAsync(clientWebSocketBTC, uriBTC, "BTC-USDT");
                    await ConnectWebSocketAsync(clientWebSocketETH, uriETH, "ETH-USDT");

                    while (!stoppingToken.IsCancellationRequested)
                    {
                        await HandleWebSocketMessageAsync(clientWebSocketBTC, "BTCUSDT");
                        await HandleWebSocketMessageAsync(clientWebSocketETH, "ETHUSDT");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Hata: " + ex.Message);
                    // Handle the exception appropriately, e.g., logging, retry logic, etc.
                }
            }
        }

        private async Task ConnectWebSocketAsync(ClientWebSocket clientWebSocket, Uri uri, string symbol)
        {
            await clientWebSocket.ConnectAsync(uri, CancellationToken.None);
            Console.WriteLine($"{symbol} WebSocket bağlantısı kuruldu.");
        }

        private async Task HandleWebSocketMessageAsync(ClientWebSocket clientWebSocket, string symbol)
        {
            ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[1024]);
            WebSocketReceiveResult result = await clientWebSocket.ReceiveAsync(buffer, CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Text)
            {
                string message = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
                var json = JsonSerializer.Deserialize<PriceDto>(message);

                long timestamp = json.E;
                DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).LocalDateTime;

                string pricestr = json.c;

                IEnumerable<string> groupNames = _hubContext.Groups.ToList();


                await _hubContext.Clients.Group(symbol).SendAsync("ReceivePriceUpdate", $"Token: {symbol} - Zaman: {dateTime}, Fiyat: {pricestr}");
                Console.WriteLine($"{symbol} - Zaman: {dateTime}, Fiyat: {pricestr}");
            }
        }

    }
}

public class PriceDto
{
    public long E { get; set; }
    public string? c { get; set; }
    public string? s { get; set; }
}
