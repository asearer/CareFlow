import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { Button } from '../components/Button';
import { Input } from '../components/Input';
import { Card } from '../components/Card';
import { useAuth } from '../context/AuthContext';
import styles from './Auth.module.css';
import api from '../services/api';

export const Login = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const { login } = useAuth();
    const navigate = useNavigate();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError('');
        setLoading(true);

        try {
            const response = await api.post('/Auth/login', { email, password });
            login(response.data.token);
            navigate('/dashboard');
        } catch (err: any) {
            setError(err.response?.data?.message || 'Failed to login');
            // For demo, if API fails (404/405), we handle gracefully or let it fail
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className={styles.container}>
            <Card title="Sign In" className={styles.authCard}>
                <form onSubmit={handleSubmit} className={styles.form}>
                    <Input
                        label="Email"
                        type="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        required
                    />
                    <Input
                        label="Password"
                        type="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                    />
                    {error && <div className={styles.error}>{error}</div>}
                    <Button type="submit" isLoading={loading} className={styles.submitBtn}>
                        Sign In
                    </Button>
                    <div style={{ marginTop: '1rem', borderTop: '1px solid var(--color-border)', paddingTop: '1rem' }}>
                        <Button
                            type="button"
                            variant="secondary"
                            className={styles.submitBtn}
                            onClick={(e) => {
                                e.preventDefault();
                                setEmail('admin@demo.com');
                                setPassword('DemoPassword123!');
                                // Auto-submit after state update
                                setTimeout(() => {
                                    const submitButton = document.querySelector('button[type="submit"]') as HTMLButtonElement;
                                    submitButton?.click();
                                }, 100);
                            }}
                        >
                            Demo Login (Admin)
                        </Button>
                    </div>
                </form>
                <div className={styles.footer}>
                    Don't have an account? <Link to="/register">Sign Up</Link>
                </div>
            </Card>
        </div>
    );
};
