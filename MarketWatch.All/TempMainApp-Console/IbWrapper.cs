using IBApi;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;

public class IbWrapper : EWrapper
{
    public EClientSocket ClientSocket { get; private set; }
    public string CurrentSymbol { get; set; }
    private int requestId = 1;
    private Dictionary<string, List<string>> dataStore = new Dictionary<string, List<string>>();

    public IbWrapper() => ClientSocket = new EClientSocket(this);

    public int NextRequestId() => requestId++;

    public void historicalData(int reqId, Bar bar)
    {
        if (string.IsNullOrEmpty(CurrentSymbol)) return;

        if (!dataStore.ContainsKey(CurrentSymbol))
            dataStore[CurrentSymbol] = new List<string>();

        dataStore[CurrentSymbol].Add($"{bar.Time},{bar.Open},{bar.High},{bar.Low},{bar.Close},{bar.Volume}");
    }

    public void historicalDataEnd(int reqId, string start, string end)
    {
        if (string.IsNullOrEmpty(CurrentSymbol)) return;

        Directory.CreateDirectory("data");
        File.WriteAllLines(Path.Combine("data", $"{CurrentSymbol}.csv"), dataStore[CurrentSymbol]);
        Console.WriteLine($"Saved data for {CurrentSymbol}");
    }

    public void error(Exception e) => Console.WriteLine(e.Message);
    public void error(string str) => Console.WriteLine(str);
    public void error(int id, int code, string msg) => Console.WriteLine($"Error {id} {code}: {msg}");
    public void connectionClosed() => Console.WriteLine("Connection closed");

    // Other EWrapper methods can remain empty
    public void tickPrice(int tickerId, int field, double price, TickAttrib attrib) { }
    public void tickSize(int tickerId, int field, int size) { }
    public void tickOptionComputation(int tickerId, int field, double impliedVol, double delta,
        double optPrice, double pvDividend, double gamma, double vega, double theta, double undPrice)
    { }
    public void tickGeneric(int tickerId, int field, double value) { }
    public void tickString(int tickerId, int field, string value) { }
    public void tickEFP(int tickerId, int tickType, double basisPoints, string formattedBasisPoints,
        double totalDividends, int holdDays, string futureExpiry, double dividendImpact, double dividendsToExpiry)
    { }
    public void orderStatus(int orderId, string status, double filled, double remaining, double avgFillPrice,
        int permId, int parentId, double lastFillPrice, int clientId, string whyHeld, double mktCapPrice)
    { }
    public void openOrder(int orderId, Contract contract, Order order, OrderState orderState) { }
    public void openOrderEnd() { }
    public void updateAccountValue(string key, string val, string currency, string accountName) { }
    public void updatePortfolio(Contract contract, double position, double marketPrice, double marketValue,
        double averageCost, double unrealizedPNL, double realizedPNL, string accountName)
    { }
    public void updateAccountTime(string timestamp) { }
    public void accountDownloadEnd(string account) { }
    public void nextValidId(int orderId) { }
    public void contractDetails(int reqId, ContractDetails contractDetails) { }
    public void contractDetailsEnd(int reqId) { }
    public void bondContractDetails(int reqId, ContractDetails contractDetails) { }
    public void execDetails(int reqId, Contract contract, Execution execution) { }
    public void execDetailsEnd(int reqId) { }
    public void commissionReport(CommissionReport commissionReport) { }
    public void fundamentalData(int reqId, string data) { }
    public void historicalDataUpdate(int reqId, Bar bar) { }
    public void marketDataType(int reqId, int marketDataType) { }
    public void tickSnapshotEnd(int reqId) { }
    public void realtimeBar(int reqId, long time, double open, double high, double low, double close,
        long volume, double wap, int count)
    { }
    public void currentTime(long time) { }
    public void managedAccounts(string accountsList) { }
    public void receiveFA(int faDataType, string faXmlData) { }
    public void scannerParameters(string xml) { }
    public void scannerData(int reqId, int rank, ContractDetails contractDetails, string distance,
        string benchmark, string projection, string legsStr)
    { }
    public void scannerDataEnd(int reqId) { }
    public void connectAck() { }
    public void position(string account, Contract contract, double pos, double avgCost) { }
    public void positionEnd() { }
    public void accountSummary(int reqId, string account, string tag, string value, string currency) { }
    public void accountSummaryEnd(int reqId) { }
}
