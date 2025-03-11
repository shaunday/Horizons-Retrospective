import { Group, Title, Text } from "@mantine/core";

function Header() {
  return (
    <Group position="apart" p="sm" style={{ borderBottom: "1px solid #ccc" }}>
      <Title order={2}>Horizons / Retrospective</Title>
      <Text mt={5} size="lg" color="dimmed">Assets Flow: Overview & Journal</Text>
    </Group>
  );
}

export default Header;
