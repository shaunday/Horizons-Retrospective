using IBApi;
using System;
using System.Diagnostics.Contracts;
using System.Threading;

public class IbHistoricalFetcher : IDisposable
{
    private EClientSocket client;
    private IbWrapper wrapper;

    public IbHistoricalFetcher()
    {
        wrapper = new IbWrapper();
        client = wrapper.ClientSocket;
    }

    public void Connect()
    {
        client.eConnect("127.0.0.1", 7496, 0);
        Thread.Sleep(1000); // wait for connection
    }

    public void Disconnect() => client.eDisconnect();

    public void RequestHistoricalData(string symbol, string duration, string barSize)
    {
        wrapper.CurrentSymbol = symbol;

        var contract = new Contract
        {
            Symbol = symbol,
            SecType = "STK",
            Exchange = "SMART",
            Currency = "USD"
        };

        client.reqHistoricalData(
            wrapper.NextRequestId(),
            contract,
            "",
            duration,
            barSize,
            "TRADES",
            1,
            1,
            false,
            null
        );

        Thread.Sleep(1000); // avoid throttling
    }

    public void Dispose() => Disconnect();
}
