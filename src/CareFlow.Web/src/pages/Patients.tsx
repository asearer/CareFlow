import { useEffect, useState } from 'react';
import type { ChangeEvent } from 'react';
import { PatientService } from '../services/api';
import type { Patient } from '../services/api';
import { Card } from '../components/Card';
import { Button } from '../components/Button';
import styles from './Patients.module.css';
import { Modal } from '../components/Modal';
import { Input } from '../components/Input';

const Patients = () => {
    const [patients, setPatients] = useState<Patient[]>([]);
    const [loading, setLoading] = useState(true);
    const [createLoading, setCreateLoading] = useState(false);
    const [error, setError] = useState('');
    const [isModalOpen, setIsModalOpen] = useState(false);

    // Form Stats
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [email, setEmail] = useState('');
    const [phone, setPhone] = useState('');
    const [dob, setDob] = useState('');
    const [address, setAddress] = useState('');

    const fetchPatients = async () => {
        try {
            const data = await PatientService.getAll();
            setPatients(data);
        } catch (err) {
            console.error(err);
            setError('Failed to load patients.');
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchPatients();
    }, []);

    const handleCreate = async (e: React.FormEvent) => {
        e.preventDefault();
        setCreateLoading(true);
        try {
            await PatientService.create({
                firstName,
                lastName,
                email,
                phoneNumber: phone,
                dateOfBirth: new Date(dob).toISOString(),
                address
            });
            setIsModalOpen(false);
            fetchPatients();
            // Reset form
            setFirstName('');
            setLastName('');
            setEmail('');
            setPhone('');
            setDob('');
            setAddress('');
        } catch (err) {
            console.error(err);
            alert('Failed to create patient');
        } finally {
            setCreateLoading(false);
        }
    };

    return (
        <div className={styles.container}>
            <div className={styles.actions}>
                <Button onClick={() => setIsModalOpen(true)}>+ New Patient</Button>
            </div>

            {error && <div className={styles.error}>{error}</div>}

            <div className={styles.grid}>
                {loading ? (
                    <p>Loading...</p>
                ) : patients.length === 0 ? (
                    <Card className={styles.empty}>
                        <p>No patients found. Create one to get started.</p>
                    </Card>
                ) : (
                    patients.map(patient => (
                        <Card key={patient.id} title={`${patient.firstName} ${patient.lastName}`} className={styles.patientInfo}>
                            <p><strong>DOB:</strong> {new Date(patient.dateOfBirth).toLocaleDateString()}</p>
                            <p><strong>Email:</strong> {patient.email}</p>
                            <p><strong>Phone:</strong> {patient.phoneNumber}</p>
                        </Card>
                    ))
                )}
            </div>

            <Modal
                isOpen={isModalOpen}
                onClose={() => setIsModalOpen(false)}
                title="New Patient"
            >
                <form onSubmit={handleCreate} className={styles.form}>
                    <div className={styles.row}>
                        <Input label="First Name" value={firstName} onChange={(e: ChangeEvent<HTMLInputElement>) => setFirstName(e.target.value)} required />
                        <Input label="Last Name" value={lastName} onChange={(e: ChangeEvent<HTMLInputElement>) => setLastName(e.target.value)} required />
                    </div>
                    <Input label="Email" type="email" value={email} onChange={(e: ChangeEvent<HTMLInputElement>) => setEmail(e.target.value)} required />
                    <div className={styles.row}>
                        <Input label="Phone" value={phone} onChange={(e: ChangeEvent<HTMLInputElement>) => setPhone(e.target.value)} required />
                        <Input label="Date of Birth" type="date" value={dob} onChange={(e: ChangeEvent<HTMLInputElement>) => setDob(e.target.value)} required />
                    </div>
                    <Input label="Address" value={address} onChange={(e: ChangeEvent<HTMLInputElement>) => setAddress(e.target.value)} required />

                    <div className={styles.formActions}>
                        <Button type="button" variant="secondary" onClick={() => setIsModalOpen(false)}>Cancel</Button>
                        <Button type="submit" isLoading={createLoading}>Create Patient</Button>
                    </div>
                </form>
            </Modal>
        </div>
    );
};

export default Patients;
