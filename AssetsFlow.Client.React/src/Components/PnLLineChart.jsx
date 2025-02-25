import React, { useState, useEffect } from "react";

const PnLLineChart = () => {
  const [Chart, setChart] = useState(null);

  useEffect(() => {
    // Dynamically import Recharts only when the component is loaded
    import('recharts').then((module) => {
      setChart(() => module.LineChart); // Set the component you need dynamically
    });
  }, []);

  // Sample Data
  const data = [
    { time: "2023-01-01", pnl: 200 },
    { time: "2023-01-02", pnl: 300 },
    { time: "2023-01-03", pnl: 250 },
    { time: "2023-01-04", pnl: 400 },
    { time: "2023-01-05", pnl: 350 },
  ];

  return Chart ? (
    <div style={{ width: "100%", height: 400 }}>
      <Chart data={data}>
        <ResponsiveContainer>
          <Chart data={data}>
            <module.CartesianGrid strokeDasharray="3 3" />
            <module.XAxis
              dataKey="time"
              label={{ value: "Time", position: "insideBottom", offset: -10 }}
            />
            <module.YAxis
              label={{ value: "PnL", angle: -90, position: "insideLeft" }}
            />
            <module.Tooltip />
            <module.Legend />
            <module.Line
              type="monotone"
              dataKey="pnl"
              stroke="#82ca9d"
              strokeWidth={2}
              dot={{ r: 6 }}
              activeDot={{ r: 8 }}
            />
          </Chart>
        </ResponsiveContainer>
      </Chart>
    </div>
  ) : (
    <div>Loading...</div>
  );
};

export default React.memo(PnLLineChart);