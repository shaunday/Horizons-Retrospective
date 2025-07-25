import { useState, useEffect } from 'react';
import { Text, Group, Button, Box, Divider } from '@mantine/core';
import { TbGitBranch, TbCalendar, TbCode } from 'react-icons/tb';
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
    <Box 
      className="bg-gradient-to-r from-slate-50 to-slate-100 border-t border-slate-200 shadow-sm"
      py="xs"
      px="lg"
    >
      <Group justify="space-between" align="center" className="text-xs">
        {/* Copyright */}
        <Text fz="xs" className="text-slate-600 font-medium">
          &copy; Shaun Day 2025
        </Text>

        {/* Version Information */}
        <Group gap="md" align="center">
          {/* Frontend Info */}
          <Group gap="xs" align="center">
            <TbCode size={12} className="text-slate-500" />
            <Text fz="xs" className="text-slate-600 font-medium">FE:</Text>
            <Text fz="xs" className="text-slate-700">v{__APP_VERSION__}</Text>
            <TbCalendar size={12} className="text-slate-500" />
            <Text fz="xs" className="text-slate-700">
              {new Date(__APP_BUILD_DATE__).toLocaleDateString()}
            </Text>
            {showCommits && (
              <>
                <TbGitBranch size={12} className="text-slate-500" />
                <Text fz="xs" className="text-slate-700 font-mono">
                  {__GIT_COMMIT__?.substring(0, 7)}
                </Text>
              </>
            )}
          </Group>

          <Divider orientation="vertical" className="h-4" />

          {/* Backend Info */}
          <Group gap="xs" align="center">
            <TbCode size={12} className="text-slate-500" />
            <Text fz="xs" className="text-slate-600 font-medium">BE:</Text>
            <Text fz="xs" className="text-slate-700">{backendVersion}</Text>
            <TbCalendar size={12} className="text-slate-500" />
            <Text fz="xs" className="text-slate-700">
              {new Date(backendBuildTimestamp).toLocaleDateString()}
            </Text>
            {showCommits && (
              <>
                <TbGitBranch size={12} className="text-slate-500" />
                <Text fz="xs" className="text-slate-700 font-mono">
                  {backendCommit?.substring(0, 7)}
                </Text>
              </>
            )}
          </Group>
        </Group>

        {/* Toggle Button */}
        <Button
          variant="subtle"
          size="xs"
          onClick={() => setShowCommits((v) => !v)}
          className="text-slate-500 hover:text-slate-700 hover:bg-slate-200 transition-colors"
        >
          {showCommits ? 'Hide Details' : 'Show Details'}
        </Button>
      </Group>
    </Box>
  );
}