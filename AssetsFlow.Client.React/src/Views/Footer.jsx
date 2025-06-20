import { useState, useEffect } from 'react';
import { Text, Group, Button } from '@mantine/core';
import { getVersions } from "@services/ApiRequests/versionsApiAccess";

export default function Footer() {
  const [backendVersion, setBackendVersion] = useState('loading...');
  const [backendCommit, setBackendCommit] = useState('loading...');
  const [backendBuildTimestamp, setBackendBuildTimestamp] = useState('loading...');
  const [showCommits, setShowCommits] = useState(false);

  useEffect(() => {
    const fetchVersion = async () => {
      try {
        const response = await getVersions();
        setBackendVersion(response?.beVersion || 'unknown');
        setBackendCommit(response?.gitCommit || 'unknown');
        setBackendBuildTimestamp(response?.buildTimeStamp || 'unknown');
      } catch (error) {
        setBackendVersion('error');
        setBackendCommit('error');
        setBackendBuildTimestamp('error');
        console.error('Failed to fetch backend version info:', error);
      }
    };

    fetchVersion();
  }, []);

  return (
    <Group
      className="w-full px-10 py-2 flex items-center mt-3 border-t border-gray-300 text-xs"
    >
      <Text size="xs" className="mr-auto flex-shrink-0 text-sm">
        &copy; Shaun Day 2025
      </Text>

      <Group spacing="xs" className="flex-shrink-0">
        <Text size="xs"><b>FE:</b></Text>
        <Text size="xs">Version: <b>{__APP_VERSION__}</b></Text>
        <Text size="xs">|</Text>
        <Text size="xs">Deployed: <b>{new Date(__APP_BUILD_DATE__).toLocaleDateString()}</b></Text>
        {showCommits && (
          <>
            <Text size="xs">|</Text>
            <Text size="xs">Commit: <b>{__GIT_COMMIT__}</b></Text>
          </>
        )}
      </Group>

      <Text size="xs" mx="xs" className="flex-shrink-0">
        ___
      </Text>

      <Group spacing="xs" className="flex-shrink-0">
        <Text size="xs"><b>BE:</b></Text>
        <Text size="xs">Version: <b>{backendVersion}</b></Text>
        <Text size="xs">|</Text>
        <Text size="xs">Deployed: <b>{new Date(backendBuildTimestamp).toLocaleDateString()}</b></Text>
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
        className="ml-auto"
        onClick={() => setShowCommits((v) => !v)}
      >
        {showCommits ? '>>' : '<<'}
      </Button>
    </Group>
  );
}