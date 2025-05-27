import { useState, useEffect } from 'react';
import { Text, Group, Button } from '@mantine/core';
import { getVersions } from "@services/ApiRequests/versionsApiAccess";

export default function Footer() {
    const [backendVersion, setBackendVersion] = useState('loading...');
    const [backendCommit, setBackendCommit] = useState('loading...');
    const [showCommits, setShowCommits] = useState(false);

    useEffect(() => {
        const fetchVersion = async () => {
            try {
                const response = await getVersions();
                setBackendVersion(response?.beVersion || 'unknown');
                setBackendCommit(response?.gitCommit || 'unknown');
            } catch (error) {
                setBackendVersion('error');
                setBackendCommit('error');
                console.error('Failed to fetch backend version info:', error);
            }
        };

        fetchVersion();
    }, []);

    return (
        <Group
            style={{
                width: '100%',
                padding: '8px 40px',
                display: 'flex',
                alignItems: 'center',
                borderTop: "1px solid #ccc",
                fontSize: '0.75rem',
            }}
        >
            <Text size="xs" style={{ marginRight: 'auto', flexShrink: 0, fontSize: '0.85rem' }}>
                &copy; Shaun Day 2025
            </Text>

            <Group spacing="xs" style={{ flexShrink: 0 }}>
                <Text size="xs"><b>FE:</b></Text>
                <Text size="xs">Version: <b>{__APP_VERSION__}</b></Text>
                <Text size="xs">|</Text>
                <Text size="xs">Build: <b>{new Date(__APP_BUILD_DATE__).toLocaleDateString()}</b></Text>
                {showCommits && (
                    <>
                        <Text size="xs">|</Text>
                        <Text size="xs">Commit: <b>{__GIT_COMMIT__}</b></Text>
                    </>
                )}
            </Group>

            <Text size="xs" mx="xs" style={{ flexShrink: 0 }}>
                |
            </Text>

            <Group spacing="xs" style={{ flexShrink: 0 }}>
                <Text size="xs"><b>BE:</b></Text>
                <Text size="xs">Version: <b>{backendVersion}</b></Text>
                {showCommits && (
                    <>
                        <Text size="xs">|</Text>
                        <Text size="xs">Commit: <b>{backendCommit}</b></Text>
                    </>
                )}
            </Group>

            <Button
                variant="subtle"
                size="xs"
                p="0"
                style={{ marginLeft: 'auto' }}
                onClick={() => setShowCommits((v) => !v)}
            >
                {showCommits ?  '>>' : '<<'}
            </Button>
        </Group>
    );
}
