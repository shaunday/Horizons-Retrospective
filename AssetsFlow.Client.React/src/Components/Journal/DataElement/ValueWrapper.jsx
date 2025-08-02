import React, { useState } from "react";
import { Popover } from "@mantine/core";
import { dataElementContentParser } from "@services/dataElementContentParser";
import ValueDisplay from "./ValueDisplay";
import ValuePopover from "./ValuePopover";

function ValueWrapper({ cellInfo, onValueChangeInitiated }) {
  const isOverview = onValueChangeInitiated === undefined;
  const { contentValue, textRestrictions } = dataElementContentParser(cellInfo);

  const [popoverOpened, setPopoverOpened] = useState(false);

  const handleSubmit = (value, changeDetails) => {
    onValueChangeInitiated(value, changeDetails);
    setPopoverOpened(false);
  };

  return (
    <Popover
      opened={popoverOpened}
      onChange={setPopoverOpened}
      position="bottom"
      withArrow
      shadow="md"
    >
      <Popover.Target>
        <ValueDisplay
          contentValue={contentValue}
          isOverview={isOverview}
          onClick={() => !isOverview && setPopoverOpened(true)}
        />
      </Popover.Target>

      <Popover.Dropdown>
        <ValuePopover
          contentValue={contentValue}
          textRestrictions={textRestrictions}
          onSubmit={handleSubmit}
        />
      </Popover.Dropdown>
    </Popover>
  );
}

export default React.memo(ValueWrapper);
