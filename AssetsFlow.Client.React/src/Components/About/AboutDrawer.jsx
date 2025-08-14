import { Drawer, Text, Stack, Divider, Image } from "@mantine/core";

const AboutDrawer = ({ opened, onClose }) => {
  return (
    <Drawer
      opened={opened}
      onClose={onClose}
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
      <Stack gap="sm">
        <Text size="xl" weight={700} mb="md">
          About this App
        </Text>

        <Text>
          This is an online trading journal that allows you to record trades from the idea stage
          through building or reducing a position, performing evaluations, and finally closing the
          trade.
        </Text>

        <Divider my="sm" />

        <Text>
          To add a new trade, click on <b>&quot;Add a Trade&quot;</b>. This creates a Trade
          Composition with an initial idea.
        </Text>

        <Text>Once all fields in the idea are filled, you can initiate an actual position.</Text>

        <Text>
          After an initial position is created, you can <b>create</b> evaluations (your thoughts on
          the market or trade), reduce or add to the position, and ultimately close all positions.
        </Text>

        <Image
          src="/tradeActionsImg.png"
          alt="Trade Actions Illustration"
          radius="md"
          fit="contain"
          className="max-h-20 mt-2"
        />

        <Divider my="sm" />

        <Text>
          A trade component or element becomes active once all essential information is entered.
        </Text>

        <Text>
          Currently, trade actions (expanding the trade) are only allowed once all elements are
          active.
        </Text>

        <Divider my="sm" />

        <Text>
          Components are collapsed by default. Click on the component name to expand it and view its
          details.
        </Text>

        <Text>
          Once a trade component (idea, evaluation, position) is expanded, you can click on the
          value to <b>edit</b> it. This opens a popover where you can change the value and add
          change notes.
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
