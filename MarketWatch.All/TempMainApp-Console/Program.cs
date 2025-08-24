// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var symbols = File.ReadAllLines("symbols.csv").Take(10).ToList();

using var ibClient = new IbHistoricalFetcher();
ibClient.Connect();

foreach (var symbol in symbols)
{
    Console.WriteLine($"Fetching {symbol}...");
    ibClient.RequestHistoricalData(symbol, "2 Y", "1 day");
}

Console.WriteLine("Fetching data... press Enter to exit.");
Console.ReadLine();
ibClient.Disconnect();