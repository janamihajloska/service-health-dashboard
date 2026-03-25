import type { ServiceStatus } from "../models/ServiceStatus";

const API_URL = 'http://localhost:5001';

export async function fetchServiceStatuses() : Promise<ServiceStatus[]> {
    const res = await fetch(`${API_URL}/api/v1/health`);

    if(!res.ok){
        throw new Error('Failed to fetch new statuses');
    }
    return res.json();    
}