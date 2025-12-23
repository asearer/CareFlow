import { useEffect, useState } from 'react';
import { AuditLogService } from '../services/api';
import type { AuditLog } from '../services/api';
import { Card } from '../components/Card';

export const AuditLogs = () => {
    const [logs, setLogs] = useState<AuditLog[]>([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchLogs = async () => {
            try {
                const data = await AuditLogService.getAll();
                setLogs(data);
            } catch (error) {
                console.error("Failed to fetch audit logs", error);
            } finally {
                setLoading(false);
            }
        };

        fetchLogs();
    }, []);

    return (
        <div style={{ padding: '2rem' }}>
            <h1 style={{ marginBottom: '1.5rem', fontSize: '1.8rem', fontWeight: 'bold' }}>System Audit Logs</h1>
            <Card>
                {loading ? (
                    <p>Loading logs...</p>
                ) : logs.length === 0 ? (
                    <p>No audit logs found.</p>
                ) : (
                    <div style={{ overflowX: 'auto' }}>
                        <table style={{ width: '100%', borderCollapse: 'collapse', textAlign: 'left' }}>
                            <thead>
                                <tr style={{ borderBottom: '1px solid var(--color-border)' }}>
                                    <th style={{ padding: '1rem' }}>Timestamp</th>
                                    <th style={{ padding: '1rem' }}>Action</th>
                                    <th style={{ padding: '1rem' }}>Entity</th>
                                    <th style={{ padding: '1rem' }}>User / System</th>
                                    <th style={{ padding: '1rem' }}>Details</th>
                                </tr>
                            </thead>
                            <tbody>
                                {logs.map(log => (
                                    <tr key={log.id} style={{ borderBottom: '1px solid var(--color-border)' }}>
                                        <td style={{ padding: '1rem' }}>{new Date(log.timestamp).toLocaleString()}</td>
                                        <td style={{ padding: '1rem', fontWeight: 'bold', color: 'var(--color-primary)' }}>{log.action}</td>
                                        <td style={{ padding: '1rem' }}>{log.entityName}</td>
                                        <td style={{ padding: '1rem' }}>{log.userId || 'System'}</td>
                                        <td style={{ padding: '1rem', color: 'var(--color-text-muted)' }}>{log.details}</td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                )}
            </Card>
        </div>
    );
};
