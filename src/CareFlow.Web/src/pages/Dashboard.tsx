
import { Card } from '../components/Card';
import { useAuth } from '../context/AuthContext';

export const Dashboard = () => {
    const { user } = useAuth();

    return (
        <div>
            <h1 style={{ marginBottom: '1.5rem' }}>Welcome back, {user?.firstName}!</h1>
            <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(300px, 1fr))', gap: '1.5rem' }}>
                <Card title="Upcoming Appointments">
                    <p>No appointments scheduled for today.</p>
                    {/* Add appointment list logic here later */}
                </Card>
                <Card title="Stats">
                    <div style={{ display: 'flex', gap: '2rem' }}>
                        <div>
                            <div style={{ fontSize: '2rem', fontWeight: 'bold', color: 'var(--color-primary)' }}>0</div>
                            <div style={{ color: 'var(--color-text-muted)' }}>Today</div>
                        </div>
                        <div>
                            <div style={{ fontSize: '2rem', fontWeight: 'bold', color: 'var(--color-primary)' }}>0</div>
                            <div style={{ color: 'var(--color-text-muted)' }}>Week</div>
                        </div>
                    </div>
                </Card>
            </div>
        </div>
    );
};
