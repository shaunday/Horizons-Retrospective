import React from "react";
import { Badge, Tooltip } from "@mantine/core";
import { TbProgressAlert } from "react-icons/tb";

function Notifications({ shortText, expandedText }) {
  return (
    <Tooltip label={expandedText} withArrow>
      <Badge
        size="sm"
        color="red.3"
        variant="light"
        radius="md"
        leftSection={<TbProgressAlert size={14} />}
        className="mx-1 hover:bg-red-400 hover:text-white transition-colors"
      >
        {shortText}
      </Badge>
    </Tooltip>
  );
}

export default React.memo(Notifications); 