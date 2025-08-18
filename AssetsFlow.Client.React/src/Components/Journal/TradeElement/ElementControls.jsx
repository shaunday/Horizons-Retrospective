import React from "react";
import { Menu, ActionIcon } from "@mantine/core";
import { TbDots, TbTrash } from "react-icons/tb";
import { ElementActions } from "@constants/journalConstants";
import ProcessingAndSuccessMessage from "@components/Processing/ProcessingAndSuccessMessage";
import { useElementActionMutation } from "@hooks/Journal/Element/useElementActionMutation";

function ElementControls({ tradeElement, className = "" }) {
  const { elementActionMutation, processingStatus } = useElementActionMutation(tradeElement);

  const handleAction = (action) => {
    elementActionMutation.mutate({ action });
  };

  return (
    <div className={className}>
      <div>
        <Menu shadow="md" width={120} position="bottom-end">
          <Menu.Target>
            <ActionIcon variant="subtle">
              <TbDots />
            </ActionIcon>
          </Menu.Target>
          <Menu.Dropdown>
            <Menu.Item
              leftSection={<TbTrash size={16} />}
              onClick={() => handleAction(ElementActions.DELETE)}
            >
              Delete Element
            </Menu.Item>
          </Menu.Dropdown>
        </Menu>
      </div>
      <ProcessingAndSuccessMessage status={processingStatus} />
    </div>
  );
}

export default React.memo(ElementControls);
