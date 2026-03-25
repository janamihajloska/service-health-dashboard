import React from "react";
import type { ServiceStatus } from "../models/ServiceStatus";


interface Props {
    services: ServiceStatus[]
}

const SummaryCards : React.FC<Props> = ({services}) => {
    const total = services.length;
    const healthy = services.filter(s => s.status =="Healthy").length;
    const degraded = services.filter(s => s.status =="Degraded").length;
    const down = services.filter(s => s.status =="Down").length;

    const cardStyle = (bg: string) => ({
    flex: 1,
    padding: '16px',
    borderRadius: '8px',
    backgroundColor: bg,
    textAlign: 'center' as const
    });

    return(
      <div style={{ display: 'flex', gap: '12px', marginBottom: '20px'}}>
      <div style={cardStyle('#e0ffe0')}>
        <h3>Healthy</h3>
        <p>{healthy}</p>
      </div>

      <div style={cardStyle('#fff0b3')}>
        <h3>Degraded</h3>
        <p>{degraded}</p>
      </div>

      <div style={cardStyle('#ffe0e0')}>
        <h3>Down</h3>
        <p>{down}</p>
      </div>

      <div style={cardStyle('#e0e0ff')}>
        <h3>Total</h3>
        <p>{total}</p>
      </div>
    </div>
    );
};

export default SummaryCards;