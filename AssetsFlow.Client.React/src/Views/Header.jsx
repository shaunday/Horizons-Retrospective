import { Title, Text, Button, Flex, Box } from "@mantine/core";
import { TbHelp } from "react-icons/tb";
import { useDisclosure } from '@mantine/hooks';
import AboutDrawer from "@components/About/AboutDrawer";

function Header() {
  const [opened, { open, close }] = useDisclosure(false);

  return (
    <>
      <Flex p="sm" justify="space-between" align="center" className="border-b border-gray-300">
        <Box>
          <Title order={2}>Horizons / Retrospective</Title>
          <Text mt={5} size="lg" c="dimmed">Assets Flow: Overview & Journal</Text>
        </Box>

        <Button
          variant="light"
          size="xs"
          leftSection={<TbHelp size={16} />}
          onClick={open}
        >
          Help
        </Button>
      </Flex>

      <AboutDrawer opened={opened} onClose={close} />
    </>
  );
}

export default Header;
