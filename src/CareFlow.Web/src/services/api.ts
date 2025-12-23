import axios from 'axios';

const API_URL = 'http://localhost:8080/api'; // Direct to API for now, consider env var

const api = axios.create({
    baseURL: API_URL,
    headers: {
        'Content-Type': 'application/json',
    },
});

// Add a request interceptor to include the JWT token
api.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem('token');
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

// Types
export interface Patient {
    id: string;
    firstName: string;
    lastName: string;
    dateOfBirth: string;
    email: string;
    phoneNumber: string;
    address: string;
}

export interface CreatePatientRequest {
    firstName: string;
    lastName: string;
    dateOfBirth: string;
    email: string;
    phoneNumber: string;
    address: string;
}

export interface Appointment {
    id: string;
    patientId: string;
    therapistId: string;
    timeRange: {
        start: string;
        end: string;
    };
    status: number;
    patient?: Patient;
    // therapist?: User; // simplified
}

export interface CreateAppointmentRequest {
    patientId: string;
    therapistId: string;
    startTime: string;
    endTime: string;
}

// Services
export const PatientService = {
    getAll: async () => {
        const response = await api.get<Patient[]>('/Patients');
        return response.data;
    },
    create: async (data: CreatePatientRequest) => {
        const response = await api.post<string>('/Patients', data);
        return response.data;
    }
};

export const AppointmentService = {
    getAll: async () => {
        const response = await api.get<Appointment[]>('/Appointments');
        return response.data;
    },
    create: async (data: CreateAppointmentRequest) => {
        const response = await api.post<string>('/Appointments', data);
        return response.data;
    }
};

export interface AuditLog {
    id: string;
    action: string;
    entityName: string;
    entityId: string;
    userId?: string;
    timestamp: string;
    details: string;
}

export const AuditLogService = {
    getAll: async () => {
        const response = await api.get<AuditLog[]>('/AuditLogs');
        return response.data;
    }
};

export default api;
