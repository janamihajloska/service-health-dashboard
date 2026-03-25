import { useState, useEffect} from "react";
import type { ServiceStatus } from "../models/ServiceStatus";
import ServiceChart from "./ServiceChart";

const API_URL = 'http://localhost:5001/api/v1/health';

function getColor(status: string){
    switch(status){
            case 'Healthy':
                return 'green';
            case 'Degraded':
                return 'orange';
            case 'Down':
                return 'red';
            default:
                return 'gray';
    }
}

interface Props {
    service: ServiceStatus;
}

const ServiceCard: React.FC<Props> = ({service}) => {
    const [history, setHistory] = useState<ServiceStatus[]>([]);

    useEffect(() => {
        fetch(`${API_URL}/${service.name}/history`)
            .then(res => res.json())
            .then(setHistory)
            .catch(console.error);
    },[service.name]);

    const bgColor =
      service.status === 'Healthy' ? '#e0ffe0' :
      service.status === 'Degraded' ? '#fff0b3' :
      '#ffe0e0';

    return(
    <div style={{
      border: '1px solid #ccc',
      padding: '12px',
      marginBottom: '20px',
      borderRadius: '8px',
      backgroundColor: bgColor
      }}>
      <h3>{service.name}</h3>
      <p>{service.status} ({service.responseTimeMs} ms)</p>

      <ServiceChart history={history} />

    </div>
    );
};

export default ServiceCard;