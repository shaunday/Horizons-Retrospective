import PropTypes from "prop-types";
import { Overlay, Loader, Center, Text, Stack } from "@mantine/core";

function LoadingOverlay({ visible, text }) {
  if (!visible) return null;

  return (
    <>
      <Overlay opacity={0.6} color="#000" zIndex={9999} />
      <Center
        style={{
          position: "fixed",
          top: 0,
          left: 0,
          width: "100vw",
          height: "100vh",
          zIndex: 10000,
        }}
      >
        <Stack align="center" spacing="xs">
          <Loader size="xl" color="white" />
          {text && (
            <Text c="white" size="md" style={{ whiteSpace: "pre-line", textAlign: "center" }}>
              {text}
            </Text>
          )}
        </Stack>
      </Center>
    </>
  );
}

LoadingOverlay.propTypes = {
  visible: PropTypes.bool.isRequired,
  text: PropTypes.string,
};

export default LoadingOverlay;
