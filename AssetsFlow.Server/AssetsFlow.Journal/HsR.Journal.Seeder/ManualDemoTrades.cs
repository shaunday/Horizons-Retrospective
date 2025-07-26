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
            { "Broker", "Interactive Brokers" },
            { "Sector", "Technology" },
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
            { "Broker", "Robinhood" },
            { "Sector", "Automotive" },
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

        // Trade 4: Idea, add, evaluate, close
        public static readonly Dictionary<string, string> Trade4_Idea = new()
    {
        { "Ticker", "AMZN" },
        { "Direction", "Long" },
        { "Thesis", "Strong growth in e-commerce and AWS driving revenues." },
        { "Aligned TA", "Bullish breakout above resistance at 3300." },
        { "Sector/FA", "Consumer discretionary, strong cash flow." },
        { "Certainty", "Mid" },
        { "Contras", "High valuation, competition risks." },
        { "Invalidation", "Break below 3200 support." },
        { "Target", "3600" },
        { "Time Frame", "2-3 months" }
    };
        public static readonly Dictionary<string, string> Trade4_Add1 = new()
    {
        { "Broker", "Schwab" },
        { "Sector", "Consumer Discretionary" },
        { "Reasoning", "Entered on breakout confirmation." },
        { "Emotions", "Optimistic" },
        { "Price", "3320" },
        { "Amount", "10" },
        { "Total Cost", "33200" },
        { "Time Frame", "1 month" },
        { "Target", "3500" },
        { "SL", "3200" },
        { "R:R", "3:1" }
    };
        public static readonly Dictionary<string, string> Trade4_Evaluate = new()
    {
        { "General", "Price consolidating near target." },
        { "D/W/M str?", "Daily uptrend intact." },
        { "Momentum", "Moderate" },
        { "Fta reached?", "No" },
        { "NTA", "Hold, watch volume." }
    };
        public static readonly Dictionary<string, string> Trade4_Close = new()
    {
        { "W/L", "W" },
        { "Lessons", "Patience and position sizing paid off." }
    };

        // Trade 5: Idea, adds, evaluate
        public static readonly Dictionary<string, string> Trade5_Idea = new()
    {
        { "Ticker", "NFLX" },
        { "Direction", "Short" },
        { "Thesis", "Subscriber growth slowing, increasing competition." },
        { "Aligned TA", "Bearish head and shoulders on weekly chart." },
        { "Sector/FA", "Communication services, volatile earnings." },
        { "Certainty", "Low" },
        { "Contras", "Content investments may spur growth." },
        { "Invalidation", "Breaks above 650." },
        { "Target", "520" },
        { "Time Frame", "1-2 months" }
    };
        public static readonly Dictionary<string, string> Trade5_Add1 = new()
    {
        { "Broker", "E-Trade" },
        { "Sector", "Communication Services" },
        { "Reasoning", "Entered after neckline break." },
        { "Emotions", "Cautious" },
        { "Price", "600" },
        { "Amount", "8" },
        { "Total Cost", "4800" },
        { "Time Frame", "1 month" },
        { "Target", "540" },
        { "SL", "650" },
        { "R:R", "2.5:1" }
    };
        public static readonly Dictionary<string, string> Trade5_Add2 = new()
    {
        { "Reasoning", "Added after retest of breakdown." },
        { "Emotions", "Confident" },
        { "Price", "580" },
        { "Amount", "5" },
        { "Total Cost", "2900" },
        { "Time Frame", "3 weeks" },
        { "Target", "510" },
        { "SL", "630" },
        { "R:R", "3:1" }
    };
        public static readonly Dictionary<string, string> Trade5_Evaluate = new()
    {
        { "General", "Price trending lower, but volatile." },
        { "D/W/M str?", "Weekly downtrend confirmed." },
        { "Momentum", "Strong" },
        { "Fta reached?", "No" },
        { "NTA", "Hold with tight stops." }
    };

        // Trade 6: Idea, add, evaluate, close
        public static readonly Dictionary<string, string> Trade6_Idea = new()
    {
        { "Ticker", "GOOGL" },
        { "Direction", "Long" },
        { "Thesis", "Strong ad revenue and AI initiatives." },
        { "Aligned TA", "Ascending channel, strong support." },
        { "Sector/FA", "Tech sector leader." },
        { "Certainty", "High" },
        { "Contras", "Regulatory risk." },
        { "Invalidation", "Breaks below 2700." },
        { "Target", "3000" },
        { "Time Frame", "2-4 months" }
    };
        public static readonly Dictionary<string, string> Trade6_Add = new()
    {
        { "Broker", "TD Ameritrade" },
        { "Sector", "Technology" },
        { "Reasoning", "Entered on pullback to support." },
        { "Emotions", "Confident" },
        { "Price", "2750" },
        { "Amount", "12" },
        { "Total Cost", "33000" },
        { "Time Frame", "1 month" },
        { "Target", "2900" },
        { "SL", "2700" },
        { "R:R", "3:1" }
    };
        public static readonly Dictionary<string, string> Trade6_Evaluate = new()
    {
        { "General", "Strong fundamentals maintained." },
        { "D/W/M str?", "Daily and weekly uptrend intact." },
        { "Momentum", "Strong" },
        { "Fta reached?", "No" },
        { "NTA", "Hold and add on dips." }
    };
        public static readonly Dictionary<string, string> Trade6_Close = new()
    {
        { "W/L", "W" },
        { "Lessons", "Sticking to fundamentals and stops is key." }
    };
    }

}