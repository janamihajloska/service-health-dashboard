import React, { useEffect, useState } from 'react';
import type { ServiceStatus } from './models/ServiceStatus';
import { fetchServiceStatuses } from './api/health';
import ServiceCard from './components/ServiceCard.tsx';
import SummaryCards from './components/SummaryCards.tsx';

const API_URL = 'http://localhost:5001/api/v1/health';

const App: React.FC = () => {
  const [services, setServices] = useState<ServiceStatus[]>([]);
  const [lastUpdate, setLastUpdate] = useState<Date | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

    const loadData = () => {
    setLoading(true);
    fetch(API_URL)
      .then(res => res.json())
      .then((data: ServiceStatus[]) => {
        setServices(data);
        setLastUpdate(new Date());
        setLoading(false);
        setError(null);
      })
      .catch(err => {
        setError(err.message);
        setLoading(false);
      });
  };

  useEffect(() => {    
   loadData();   
   const interval = setInterval(loadData, 5000);
   return () => clearInterval(interval);
  }, []);

  const sortedServices =[...services].sort((a,b) => {
    const rank = { 'Down': 0, 'Degraded': 1, 'Healthy': 2 };
    return rank[a.status] - rank[b.status];
  })

  return (
    <div style={{ padding: '20px', fontFamily: 'sans-serif' }}>
      <h1>Service Health Dashboard</h1>
      <SummaryCards services={sortedServices}/>

      <div style={{ marginBottom: '12px' }}>
        <button onClick={loadData}>Refresh Now</button>
        {lastUpdate && (
          <span style={{ marginLeft: '20px', fontSize: '0.9rem', color: '#666' }}>
            Last updated: {lastUpdate.toLocaleTimeString()}
          </span>
        )}
      </div>

      {error && <p style={{ color: 'red' }}>Error: {error}</p>}
      {loading && <p>Loading services...</p>}

      {sortedServices.map(service => (
        <ServiceCard key={service.name} service={service} />
      ))}
    </div>
  )
}

export default App