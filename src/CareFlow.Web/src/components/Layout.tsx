
import { Outlet, Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { Button } from './Button';
import styles from './Layout.module.css';

export const Layout = () => {
    const { user, logout } = useAuth();
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate('/login');
    };

    return (
        <div className={styles.layout}>
            <aside className={styles.sidebar}>
                <div className={styles.brand}>CareFlow</div>
                <nav className={styles.nav}>
                    <Link to="/dashboard" className={styles.navLink}>Dashboard</Link>
                    <Link to="/appointments" className={styles.navLink}>Appointments</Link>
                    <Link to="/patients" className={styles.navLink}>Patients</Link>
                </nav>
                <div className={styles.userSection}>
                    <div className={styles.userInfo}>
                        {user?.firstName} {user?.lastName}
                        <br />
                        <span className={styles.role}>{user?.role}</span>
                    </div>
                    <Button variant="secondary" onClick={handleLogout} className={styles.logoutBtn}>
                        Logout
                    </Button>
                </div>
            </aside>
            <main className={styles.main}>
                <header className={styles.header}>
                    <h2>Dashboard</h2>
                    {/* Add header actions if needed */}
                </header>
                <div className={styles.content}>
                    <Outlet />
                </div>
            </main>
        </div>
    );
};
