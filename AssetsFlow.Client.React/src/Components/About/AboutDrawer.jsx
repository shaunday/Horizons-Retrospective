import { Drawer, Text, Stack, Divider } from "@mantine/core";

const AboutDrawer = ({ opened, onClose }) => {
  return (
    <Drawer
      opened={opened}
      onClose={onClose}
      title="About this App"
      position="right"
      size="md"
      padding="md"
      radius="md"
      withCloseButton
      styles={{
        drawer: {
          marginRight: 8, // simulate offset
        },
      }}
    >
      <Stack gao="sm">
        <Text>
          This is an online trading journal that allows you to record trades
          from an <b>idea stage</b> through building or reducing a position,
          performing evaluations, and finally <b>closing</b> the trade.
        </Text>

        <Text>
          To add a new trade, click on <b>"Add a Trade"</b>. This creates a
          Trade Composition with an initial <b>Idea</b>.
        </Text>

        <Text>
          Once all fields in the Idea are filled, you can initiate an{" "}
          <b>actual position</b>.
        </Text>

        <Text>
          After an initial position is created, you can add <b>evaluations</b>{" "}
          (your thoughts on the market or trade), <b>reduce</b> or <b>add to</b>{" "}
          the position, and ultimately <b>terminate</b> it when closing the
          trade.
        </Text>

        <Divider my="sm" />

        <Text>
          A Trade Component/Element becomes <b>active</b> once all essential
          information is entered.
        </Text>

        <Text>
          Currently only allowing trade actions (expanding the trade) once all
          elements are active.
        </Text>

        <Divider my="sm" />

        <Text>
          Components are collapsed by default. Click on the component name to
          expand it and view its details.
        </Text>

        <Text>
          Once a trade component (Idea, Evaluation, Position) is expanded, hover
          your mouse at the bottom of the component to reveal the <b>Edit</b>{" "}
          icon. Click it to open a modal for editing the component.
        </Text>

        <Divider my="sm" />

        <Text size="sm" c="dimmed" mt="xl">
          &copy; 2025 Shaun Day
        </Text>
      </Stack>
    </Drawer>
  );
};

export default AboutDrawer;
