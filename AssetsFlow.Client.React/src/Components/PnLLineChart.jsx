import React from 'react';
import { LineChart, CartesianGrid, XAxis, YAxis, Tooltip, Legend, Line, ResponsiveContainer } from 'recharts';

const PnLLineChart = () => {
  // Sample Data
  const data = [
    { time: "2023-01-01", pnl: 200 },
    { time: "2023-01-02", pnl: 300 },
    { time: "2023-01-03", pnl: 250 },
    { time: "2023-01-04", pnl: 400 },
    { time: "2023-01-05", pnl: 350 },
  ];

  return (
    <div style={{ width: "100%", height: 200 }}>
      <ResponsiveContainer>
        <LineChart data={data}>
          <CartesianGrid strokeDasharray="3 3" />
          <XAxis
            dataKey="time"
            label={{ value: "Time", position: "insideBottom", offset: -10 }}
          />
          <YAxis
            label={{ value: "PnL", angle: -90, position: "insideLeft" }}
          />
          <Tooltip />
          <Legend />
          <Line
            type="monotone"
            dataKey="pnl"
            stroke="#82ca9d"
            strokeWidth={2}
            dot={{ r: 6 }}
            activeDot={{ r: 8 }}
          />
        </LineChart>
      </ResponsiveContainer>
    </div>
  );
};

export default React.memo(PnLLineChart);