import { Title, Text, Button, Flex, Box, Group } from "@mantine/core";
import { TbHelp, TbChartLine, TbUser } from "react-icons/tb";
import { useDisclosure } from '@mantine/hooks';
import { useEffect } from 'react';
import AboutDrawer from "@components/About/AboutDrawer";
import { useAuth } from "@hooks/Auth/useAuth";  // import your auth hook

function Header() {
  const [opened, { open, close }] = useDisclosure(false);
  const isDev = import.meta.env.DEV;
  const { user } = useAuth();

  useEffect(() => {
    document.title = "HsR" + (isDev ? " [DEV]" : "");
  }, [isDev]);

  return (
    <>
      <Box 
        className="bg-gradient-to-r from-slate-50 to-slate-100 border-b border-slate-200 shadow-sm"
        py="md"
        px="lg"
      >
        <Flex justify="space-between" align="center">
          {/* Left side - Branding */}
          <Group gap="md" align="center">
            <Box className="flex items-center gap-4">
              <Box className="bg-slate-200 p-1.5 rounded-md">
                <TbChartLine size={18} className="text-slate-600" />
              </Box>
              <Box>
                <Title 
                  order={2} 
                  className="text-slate-800 font-bold tracking-tight"
                  mb={2}
                >
                  Horizons Retrospective{isDev && " [DEV]"}
                </Title>
                <Text 
                  size="sm" 
                  className="text-slate-600 font-medium"
                  style={{ letterSpacing: '0.025em' }}
                >
                  Analyze your trades, identify patterns, evolve your trading mindset.
                </Text>
              </Box>
            </Box>
          </Group>

          {/* Right side - Actions */}
          <Group gap="sm" align="center">
            {user ? (
              <Flex align="center" gap={8} className="text-slate-700 font-medium">
                <TbUser size={18} />
                <Text size="sm" fw={500} c="dark">
                  {user.firstName || user.name || "User"}
                </Text>
              </Flex>
            ) : null}

            <Button
              variant="subtle"
              size="sm"
              leftSection={<TbHelp size={16} />}
              onClick={open}
              className="text-slate-600 hover:text-slate-800 hover:bg-slate-200 transition-colors"
            >
              Help & Info
            </Button>
          </Group>
        </Flex>
      </Box>

      <AboutDrawer opened={opened} onClose={close} />
    </>
  );
}

export default Header;
