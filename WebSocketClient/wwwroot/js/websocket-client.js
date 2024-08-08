
//let jwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoidGVzdCIsIm5iZiI6MTcwODk0NzYwNywiZXhwIjoxNzA4OTQ3NjY3LCJpYXQiOjE3MDg5NDc2MDd9.90-Cv5H9cKvovpAH-a2oRo7pu8BqThRq49VvPFNPOoo";
//let jwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoidGVzdCIsIm5iZiI6MTcwODYwMDA4OCwiZXhwIjoxNzA4NjAwMTQ4LCJpYXQiOjE3MDg2MDAwODh9.3qlY_z0F8TlnmXVo41_gyXdPS4JsLGNrYuuPGEpdsgM";

//startSignalRConnection();
//debugger
//function startSignalRConnection() {
//    debugger
//     connection = new signalR.HubConnectionBuilder()
//         .withUrl("https://localhost:7090/priceHub", {
//            //accessTokenFactory: () => jwtToken
//        })
//        .build();

//    connection.onclose(() => {
//        console.log('SignalR bağlantısı kapatıldı. Tekrar deneme...');
//        setTimeout(() => startSignalRConnection(jwtToken), 2000);
//    });

//    connection.start()
//        .then(() => {
//            console.log("SignalR Hub bağlantısı kuruldu.");
//        })
//        .catch(error => {
//            console.error("Hub bağlantısı başarısız: ", error);
//        });

//    connection.on("ReceivePriceUpdate", (symbol) => {
//        console.log(` ${symbol} `);
//    });

//}


//Tokensız bağlantı
const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:44316/cryptoHub")
    .build();
debugger
connection.on("ReceivePriceUpdate", (symbol) => {
    console.log(` ${symbol} `);
});

connection.start()
    .then(() => {
        console.log("SignalR Hub bağlantısı kuruldu.");
    })
    .catch(error => {
        console.error("Hub bağlantısı başarısız: ", error);
    });



// Abone olma işlemi
function subscribeToSymbol(symbol) {
    connection.invoke("SubscribeToSymbol", symbol)
        .then(() => console.log(`Abone olundu: ${symbol}`))
        .catch(error => console.error(`Abonelik hatası: ${error}`));
}
// Abonelikten çıkma işlemi
function unsubscribeFromSymbol(symbol) {
    connection.invoke("UnsubscribeFromSymbol", symbol)
        .then(() => console.log(`Abonelik iptal edildi: ${symbol}`))
        .catch(error => console.error(`Abonelik iptali hatası: ${error}`));
}

