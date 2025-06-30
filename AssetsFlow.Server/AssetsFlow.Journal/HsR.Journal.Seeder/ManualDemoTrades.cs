using System.Collections.Generic;

namespace HsR.Journal.Seeder
{
    public static class ManualDemoTrades
    {
        // Trade 1: Just an idea
        public static readonly Dictionary<string, string> Trade1_Idea = new()
        {
            { "Ticker", "AAPL" },
            { "Direction", "Long" },
            { "Thesis", "Apple is breaking out after strong earnings and new product launches." },
            { "Aligned TA", "Price above 200MA, strong volume, breakout from base." },
            { "Sector/FA", "Tech sector leading, strong fundamentals, high ROE." },
            { "Certainty", "High" },
            { "Contras", "Market volatility, possible supply chain issues." },
            { "Invalidation", "Breaks below 180 support." },
            { "Target", "210" },
            { "Time Frame", "2-4 weeks" }
        };

        // Trade 2: Idea, 2 adds, evaluate
        public static readonly Dictionary<string, string> Trade2_Idea = new()
        {
            { "Ticker", "MSFT" },
            { "Direction", "Long" },
            { "Thesis", "Microsoft cloud growth and AI leadership." },
            { "Aligned TA", "Ascending triangle breakout." },
            { "Sector/FA", "Tech, strong cash flow." },
            { "Certainty", "Mid" },
            { "Contras", "Regulatory risk." },
            { "Invalidation", "Breaks below 320." },
            { "Target", "370" },
            { "Time Frame", "1-2 months" }
        };
        public static readonly Dictionary<string, string> Trade2_Add1 = new()
        {
            { "Reasoning", "Strong earnings beat, added to position." },
            { "Emotions", "Confident" },
            { "Price", "330" },
            { "Amount", "10" },
            { "Total Cost", "3300" },
            { "Time Frame", "2 weeks" },
            { "Target", "350" },
            { "SL", "320" },
            { "R:R", "2:1" }
        };
        public static readonly Dictionary<string, string> Trade2_Add2 = new()
        {
            { "Reasoning", "Market momentum continues, added more." },
            { "Emotions", "Optimistic" },
            { "Price", "340" },
            { "Amount", "5" },
            { "Total Cost", "1700" },
            { "Time Frame", "1 week" },
            { "Target", "355" },
            { "SL", "330" },
            { "R:R", "1.5:1" }
        };
        public static readonly Dictionary<string, string> Trade2_Evaluate = new()
        {
            { "General", "Review after earnings." },
            { "D/W/M str?", "Weekly uptrend intact." },
            { "Momentum", "Strong" },
            { "Fta reached?", "No" },
            { "NTA", "Hold position." }
        };

        // Trade 3: Idea, add, evaluate, close
        public static readonly Dictionary<string, string> Trade3_Idea = new()
        {
            { "Ticker", "TSLA" },
            { "Direction", "Short" },
            { "Thesis", "Overextended after parabolic run, signs of reversal." },
            { "Aligned TA", "Bearish engulfing on daily." },
            { "Sector/FA", "Auto sector lagging." },
            { "Certainty", "Low" },
            { "Contras", "Short squeeze risk." },
            { "Invalidation", "Breaks above 900." },
            { "Target", "700" },
            { "Time Frame", "3 weeks" }
        };
        public static readonly Dictionary<string, string> Trade3_Add = new()
        {
            { "Reasoning", "Breakdown confirmed, entered short." },
            { "Emotions", "Cautious" },
            { "Price", "850" },
            { "Amount", "5" },
            { "Total Cost", "4250" },
            { "Time Frame", "2 weeks" },
            { "Target", "750" },
            { "SL", "900" },
            { "R:R", "2:1" }
        };
        public static readonly Dictionary<string, string> Trade3_Evaluate = new()
        {
            { "General", "Mid-trade review." },
            { "D/W/M str?", "Daily reversal pattern." },
            { "Momentum", "Weakening" },
            { "Fta reached?", "Yes" },
            { "NTA", "Consider partial close." }
        };
        public static readonly Dictionary<string, string> Trade3_Close = new()
        {
            { "W/L", "W" },
            { "Lessons", "Patience paid off, but risk was high." }
        };
    }
} 