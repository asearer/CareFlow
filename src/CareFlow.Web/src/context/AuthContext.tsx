import { createContext, useContext, useState, useEffect, type ReactNode } from 'react';
import { jwtDecode } from 'jwt-decode';

interface User {
    id: string;
    email: string;
    firstName: string;
    lastName: string;
    role: string;
}

interface AuthContextType {
    user: User | null;
    token: string | null;
    login: (token: string) => void;
    logout: () => void;
    isAuthenticated: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
    const [user, setUser] = useState<User | null>(null);
    const [token, setToken] = useState<string | null>(localStorage.getItem('token'));

    useEffect(() => {
        const storedToken = localStorage.getItem('token');
        if (storedToken) {
            try {
                const decoded: any = jwtDecode(storedToken);
                // Map claims to User object (adjust keys based on JWT structure)
                const userPayload: User = {
                    id: decoded.sub || decoded.nameid, // Standard sub or nameid claim
                    email: decoded.email,
                    firstName: decoded.given_name,
                    lastName: decoded.family_name,
                    role: decoded.role,
                };
                setUser(userPayload);
                setToken(storedToken);
            } catch (error) {
                console.error("Invalid token:", error);
                localStorage.removeItem('token');
            }
        }
    }, []);

    const login = (newToken: string) => {
        localStorage.setItem('token', newToken);
        setToken(newToken);
        const decoded: any = jwtDecode(newToken);
        const userPayload: User = {
            id: decoded.sub || decoded.nameid,
            email: decoded.email,
            firstName: decoded.given_name,
            lastName: decoded.family_name,
            role: decoded.role,
        };
        setUser(userPayload);
    };

    const logout = () => {
        localStorage.removeItem('token');
        setToken(null);
        setUser(null);
    };

    return (
        <AuthContext.Provider value={{ user, token, login, logout, isAuthenticated: !!user }}>
            {children}
        </AuthContext.Provider>
    );
};

export const useAuth = () => {
    const context = useContext(AuthContext);
    if (context === undefined) {
        throw new Error('useAuth must be used within an AuthProvider');
    }
    return context;
};
