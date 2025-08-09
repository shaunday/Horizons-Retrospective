import React, { useRef, useState } from "react";
import { Popover } from "@mantine/core";
import { dataElementContentParser } from "@services/dataElementContentParser";
import ValueDisplay from "./ValueDisplay";
import ValuePopover from "./ValuePopover";

function ValueWrapper({ cellInfo, onValueChangeInitiated }) {
  const isOverview = onValueChangeInitiated === undefined;
  const { contentValue, textRestrictions } = dataElementContentParser(cellInfo);

  const [popoverOpened, setPopoverOpened] = useState(false);
  const preventCloseRef = useRef(false);

  const handleSubmit = (value, changeDetails) => {
    onValueChangeInitiated(value, changeDetails);
    setPopoverOpened(false);
  };

  return (
    <Popover
      opened={popoverOpened}
      onChange={(open) => {
        if (!open && preventCloseRef.current) {
          preventCloseRef.current = false;
          return;
        }
        setPopoverOpened(open);
      }}
      position="bottom"
      trapFocus={false}
      closeOnEscape={true}
      withArrow
      arrowSize={12}
      shadow="md"
    >
      <Popover.Target>
        <ValueDisplay
          contentValue={contentValue}
          isOverview={isOverview}
          onClick={() => !isOverview && setPopoverOpened(true)}
        />
      </Popover.Target>

      <Popover.Dropdown
        onMouseDown={() => {
          preventCloseRef.current = true;
        }}
        className="bg-neutral-50 border border-blue-50 rounded-lg p-3 shadow-md"
      >
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
