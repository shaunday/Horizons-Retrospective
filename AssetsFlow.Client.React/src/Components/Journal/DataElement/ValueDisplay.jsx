import React, { forwardRef } from "react";
import { Tooltip, Text } from "@mantine/core";

const ValueDisplay = forwardRef(function ValueDisplay(
  { contentValue, isOverview, onClick },
  ref
) {
  return (
    <div
      ref={ref}
      className="container-with-centered-content h-10 relative min-w-0 max-w-full bg-[#fefefe] rounded-md flex-shrink overflow-hidden"
      onClick={onClick}
      style={{ cursor: isOverview ? "default" : "pointer" }}
    >
      <Tooltip
        label={contentValue}
        disabled={contentValue.length < 20}
        withinPortal
        position="bottom"
      >
        <Text className="max-w-full whitespace-nowrap overflow-hidden text-ellipsis">
          {contentValue}
        </Text>
      </Tooltip>
    </div>
  );
});

export default React.memo(ValueDisplay);