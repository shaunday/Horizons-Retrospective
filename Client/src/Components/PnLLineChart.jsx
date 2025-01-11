import {
  LineChart,
  Line,
  CartesianGrid,
  XAxis,
  YAxis,
  Tooltip,
  Legend,
  ResponsiveContainer,
} from "recharts";

// Sample Data
const data = [
  { time: "2023-01-01", pnl: 200 },
  { time: "2023-01-02", pnl: 300 },
  { time: "2023-01-03", pnl: 250 },
  { time: "2023-01-04", pnl: 400 },
  { time: "2023-01-05", pnl: 350 },
];

const PnLLineChart = () => {
  return (
    <div style={{ width: "100%", height: 400 }}>
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

export default PnLLineChart;
