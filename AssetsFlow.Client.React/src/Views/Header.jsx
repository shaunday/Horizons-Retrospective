import React, { useEffect } from "react";
import {
  Title,
  Loader,
  Text,
  Button,
  Flex,
  Box,
  Group,
} from "@mantine/core";
import { TbHelp, TbChartLine, TbUser } from "react-icons/tb";
import { useDisclosure } from "@mantine/hooks";
import AboutDrawer from "@components/About/AboutDrawer";
import LoadingOverlay from "@components/LoadingOverlay";
import { useAuth } from "@hooks/Auth/useAuth";
import { useReseedDemoUser } from "@hooks/Admin/useReseedDemoUser";

function HeaderComponent({ showUserActions = true }) {
  const [opened, { open, close }] = useDisclosure(false);
  const isDev = import.meta.env.DEV;
  const { user } = useAuth();

  const { isReseeding, reseed } = useReseedDemoUser();

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
                  fz="sm"
                  className="text-slate-600 font-medium"
                  style={{ letterSpacing: "0.025em" }}
                >
                  Analyze your trades, identify patterns, evolve your trading
                  mindset.
                </Text>
              </Box>
            </Box>
          </Group>

          {/* Right side - User Actions */}
          {showUserActions && user && (
            <Group gap="sm" align="center">
              <Flex
                align="center"
                gap={8}
                className="text-slate-700 font-medium"
              >
                <TbUser size={18} />
                <Text fz="sm" fw={500} c="dark">
                  {user.firstName || user.name || "User"}
                </Text>
              </Flex>

              <Button
                variant="subtle"
                size="sm"
                onClick={reseed}
                leftSection={
                  isReseeding ? (
                    <Loader size={16} />
                  ) : (
                    <span style={{ width: 16, display: "inline-block" }} />
                  )
                }
                className="text-slate-600 hover:text-slate-800 hover:bg-slate-200 transition-colors"
                disabled={isReseeding}
              >
                {isReseeding ? "Reseeding..." : "Reseed Demo Data"}
              </Button>

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
          )}
        </Flex>
      </Box>

      <LoadingOverlay
        visible={isReseeding}
        text={"Reseed in progress ... \n(~10-15sec)"}
      />

      <AboutDrawer opened={opened} onClose={close} />
    </>
  );
}

// Memoized export to prevent unnecessary re-renders
const Header = React.memo(HeaderComponent);

export default Header;
