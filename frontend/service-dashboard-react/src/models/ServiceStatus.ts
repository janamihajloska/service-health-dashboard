export type HealthStatus = 'Healthy' | 'Degraded' | 'Down';

export interface ServiceStatus {
    name: string;
    url: string;
    status: HealthStatus;
    responseTimeMs: number;
    checkedAt: string;
}