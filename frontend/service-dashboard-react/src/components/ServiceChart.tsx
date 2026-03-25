import React from "react";
import { LineChart, Line, XAxis, YAxis,Tooltip,ResponsiveContainer, ReferenceLine } from "recharts";
import type { ServiceStatus } from "../models/ServiceStatus";

interface Props {
    history: ServiceStatus[];
}

const DEGRADED_THRESHOLD = 2000;

const ServiceChart: React.FC<Props> = ({history}) => {
  const data = history.map(item => ({
    time: new Date(item.checkedAt).toLocaleTimeString(),
    responseTime: item.responseTimeMs
  }));
    return(
      <ResponsiveContainer width="100%" height={200}>
      <LineChart data={data}>
        <XAxis
          dataKey="time"
        />
        <YAxis />
        <Tooltip
          formatter={(value) => {
            if (typeof value === 'number') {
              return `${value} ms`;
            }
          return value;
          }}
          labelFormatter={(label) => `Time: ${label}`}
          />
          <ReferenceLine
          y={DEGRADED_THRESHOLD}
          label="Degraded threshold"
          stroke="red"
          strokeDasharray="3 3"
          />
          <Line
            type="monotone"
            dataKey="responseTime"
            stroke="#8884d8"
            dot={false}
          />
      </LineChart>
    </ResponsiveContainer>
 );   
};

export default ServiceChart;

