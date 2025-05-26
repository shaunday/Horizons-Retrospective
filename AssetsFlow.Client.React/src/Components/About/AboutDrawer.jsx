import { useState } from 'react';
import { Drawer, Text, Group, Divider, Loader, Stack } from '@mantine/core';

const AboutDrawer = ({ opened, onClose }) => {
  const [backendVersion, setBackendVersion] = useState(null);
  const [loading, setLoading] = useState(true);

  return (
    <Drawer
      opened={opened}
      onClose={onClose}
      title="About this App"
      padding="md"
      size="md"
    >
      <Stack spacing="xs">
        <Text>Frontend Version: <b>{__APP_VERSION__}</b></Text>
        <Text>Build Date: <b>{new Date(__APP_BUILD_DATE__).toLocaleString()}</b></Text>
        <Text>Commit: <b>{__GIT_COMMIT__}</b></Text>

        <Divider my="sm" />

        <Group>
          <Text>Backend Version:</Text>
          {loading ? (
            <Loader size="xs" />
          ) : (
            <Text weight={500}>{backendVersion}</Text>
          )}
        </Group>

        <Divider my="sm" />

        <Text size="sm" color="dimmed" mt="md">
          &copy; 2025 Your Company Name
        </Text>
      </Stack>
    </Drawer>
  );
};

export default AboutDrawer;
