import React, { useEffect, useRef } from 'react';
import type { PropsWithChildren } from 'react';
import styles from './Modal.module.css';
import { Button } from './Button';

interface ModalProps {
    isOpen: boolean;
    onClose: () => void;
    title: string;
}

export function Modal({ isOpen, onClose, title, children }: PropsWithChildren<ModalProps>) {
    const overlayRef = useRef<HTMLDivElement>(null);

    useEffect(() => {
        const handleEscape = (e: KeyboardEvent) => {
            if (e.key === 'Escape') onClose();
        };

        if (isOpen) {
            document.addEventListener('keydown', handleEscape);
            document.body.style.overflow = 'hidden';
        }

        return () => {
            document.removeEventListener('keydown', handleEscape);
            document.body.style.overflow = 'unset';
        };
    }, [isOpen, onClose]);

    if (!isOpen) return null;

    const handleOverlayClick = (e: React.MouseEvent) => {
        if (e.target === overlayRef.current) {
            onClose();
        }
    };

    return (
        <div className={styles.overlay} ref={overlayRef} onClick={handleOverlayClick}>
            <div className={styles.modal}>
                <div className={styles.header}>
                    <h2 className={styles.title}>{title}</h2>
                    <Button variant="secondary" onClick={onClose} style={{ padding: '0.25rem 0.5rem' }}>✕</Button>
                </div>
                <div className={styles.content}>
                    {children}
                </div>
            </div>
        </div>
    );
}
