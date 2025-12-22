import { useEffect, useState } from 'react';
import { Card } from '../components/Card';
import { useAuth } from '../context/AuthContext';
import { AppointmentService } from '../services/api';
import type { Appointment } from '../services/api';

export const Dashboard = () => {
    const { user } = useAuth();
    const [appointments, setAppointments] = useState<Appointment[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchAppointments = async () => {
            try {
                const data = await AppointmentService.getAll();
                // Sort by start time ascending
                const sorted = data.sort((a, b) => new Date(a.timeRange.start).getTime() - new Date(b.timeRange.start).getTime());
                setAppointments(sorted);
            } catch (error) {
                console.error("Failed to fetch appointments", error);
            } finally {
                setLoading(false);
            }
        };

        fetchAppointments();
    }, []);

    const upcomingAppointments = appointments
        .filter(a => new Date(a.timeRange.start) > new Date())
        .slice(0, 5);

    const todayCount = appointments.filter(a => {
        const d = new Date(a.timeRange.start);
        const today = new Date();
        return d.getDate() === today.getDate() &&
            d.getMonth() === today.getMonth() &&
            d.getFullYear() === today.getFullYear();
    }).length;

    const weekCount = appointments.filter(a => {
        const d = new Date(a.timeRange.start);
        const today = new Date();
        const nextWeek = new Date(today.getTime() + 7 * 24 * 60 * 60 * 1000);
        return d >= today && d <= nextWeek;
    }).length;

    return (
        <div>
            <h1 style={{ marginBottom: '1.5rem' }}>Welcome back, {user?.firstName}!</h1>
            <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(300px, 1fr))', gap: '1.5rem' }}>
                <Card title="Upcoming Appointments">
                    {loading ? (
                        <p>Loading...</p>
                    ) : upcomingAppointments.length === 0 ? (
                        <p>No upcoming appointments.</p>
                    ) : (
                        <div style={{ display: 'flex', flexDirection: 'column', gap: '0.75rem' }}>
                            {upcomingAppointments.map(apt => (
                                <div key={apt.id} style={{ padding: '0.75rem', backgroundColor: 'var(--color-bg)', borderRadius: '0.5rem', border: '1px solid var(--color-border)' }}>
                                    <div style={{ fontWeight: '600' }}>{new Date(apt.timeRange.start).toLocaleString()}</div>
                                    <div style={{ fontSize: '0.9rem', color: 'var(--color-text-muted)' }}>
                                        {apt.patient ? `${apt.patient.firstName} ${apt.patient.lastName}` : 'Unknown Patient'}
                                    </div>
                                </div>
                            ))}
                        </div>
                    )}
                </Card>
                <Card title="Stats">
                    <div style={{ display: 'flex', gap: '2rem' }}>
                        <div>
                            <div style={{ fontSize: '2rem', fontWeight: 'bold', color: 'var(--color-primary)' }}>{todayCount}</div>
                            <div style={{ color: 'var(--color-text-muted)' }}>Today</div>
                        </div>
                        <div>
                            <div style={{ fontSize: '2rem', fontWeight: 'bold', color: 'var(--color-primary)' }}>{weekCount}</div>
                            <div style={{ color: 'var(--color-text-muted)' }}>This Week</div>
                        </div>
                    </div>
                </Card>
            </div>
        </div>
    );
};
