import { useEffect, useState } from 'react';
import type { ChangeEvent } from 'react';
import { AppointmentService, PatientService } from '../services/api';
import type { Appointment, Patient } from '../services/api';
import { Card } from '../components/Card';
import { Button } from '../components/Button';
import styles from './Patients.module.css'; // Reusing styles for now
import { Modal } from '../components/Modal';
import { Input } from '../components/Input';
import { useAuth } from '../context/AuthContext';

const Appointments = () => {
    const { user } = useAuth();
    const [appointments, setAppointments] = useState<Appointment[]>([]);
    const [patients, setPatients] = useState<Patient[]>([]);
    const [loading, setLoading] = useState(true);
    const [createLoading, setCreateLoading] = useState(false);
    const [error, setError] = useState('');
    const [isModalOpen, setIsModalOpen] = useState(false);

    // Form Stats
    const [patientId, setPatientId] = useState('');
    const [date, setDate] = useState('');
    const [time, setTime] = useState('');
    const [duration] = useState(60); // minutes

    const fetchData = async () => {
        try {
            const [apptData, patientData] = await Promise.all([
                AppointmentService.getAll(),
                PatientService.getAll()
            ]);
            setAppointments(apptData);
            setPatients(patientData);
        } catch (err) {
            console.error(err);
            setError('Failed to load data.');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchData();
    }, []);

    const handleCreate = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!user?.id) {
            alert('User not authenticated');
            return;
        }

        setCreateLoading(true);
        try {
            const startDateTime = new Date(`${date}T${time}`);
            const endDateTime = new Date(startDateTime.getTime() + duration * 60000);

            await AppointmentService.create({
                patientId,
                therapistId: user.id,
                startTime: startDateTime.toISOString(),
                endTime: endDateTime.toISOString()
            });
            setIsModalOpen(false);
            fetchData();
            // Reset
            setPatientId('');
            setDate('');
            setTime('');
        } catch (err) {
            console.error(err);
            alert('Failed to create appointment. Ensure no overlap.');
        } finally {
            setCreateLoading(false);
        }
    };

    return (
        <div className={styles.container}>
            <div className={styles.actions}>
                <Button onClick={() => setIsModalOpen(true)}>+ New Appointment</Button>
            </div>

            {error && <div style={{ color: 'red', marginBottom: '1rem', padding: '0 1rem' }}>{error}</div>}

            <div className={styles.grid}>
                {loading ? (
                    <p>Loading...</p>
                ) : appointments.length === 0 ? (
                    <Card className={styles.empty}>
                        <p>No appointments found.</p>
                    </Card>
                ) : (
                    appointments.map(apt => (
                        <Card key={apt.id} title={`${new Date(apt.timeRange.start).toLocaleString()}`} className={styles.patientInfo}>
                            <p><strong>Patient:</strong> {apt.patient?.firstName} {apt.patient?.lastName}</p>
                            <p><strong>Status:</strong> {apt.status === 0 ? 'Scheduled' : 'Completed'}</p>
                        </Card>
                    ))
                )}
            </div>

            <Modal
                isOpen={isModalOpen}
                onClose={() => setIsModalOpen(false)}
                title="Schedule Appointment"
            >
                <form onSubmit={handleCreate} className={styles.form}>
                    <div className={styles.inputGroup}>
                        <label className={styles.label}>Patient</label>
                        <select
                            className={styles.select}
                            value={patientId}
                            onChange={(e: ChangeEvent<HTMLSelectElement>) => setPatientId(e.target.value)}
                            required
                        >
                            <option value="">Select a Patient</option>
                            {patients.map(p => (
                                <option key={p.id} value={p.id}>
                                    {p.firstName} {p.lastName}
                                </option>
                            ))}
                        </select>
                    </div>

                    <div className={styles.row}>
                        <Input label="Date" type="date" value={date} onChange={(e: ChangeEvent<HTMLInputElement>) => setDate(e.target.value)} required />
                        <Input label="Time" type="time" value={time} onChange={(e: ChangeEvent<HTMLInputElement>) => setTime(e.target.value)} required />
                    </div>

                    <div className={styles.formActions}>
                        <Button type="button" variant="secondary" onClick={() => setIsModalOpen(false)}>Cancel</Button>
                        <Button type="submit" isLoading={createLoading}>Schedule</Button>
                    </div>
                </form>
            </Modal>
        </div>
    );
};

export default Appointments;
