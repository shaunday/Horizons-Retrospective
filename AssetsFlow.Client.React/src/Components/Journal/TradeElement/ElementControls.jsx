import React from "react";
import { Menu, ActionIcon } from '@mantine/core';
import { IconDots, IconTrash } from '@tabler/icons-react'; // Importing icons
import * as Constants from "@constants/journalConstants";
import { ElementActions } from "@constants/journalConstants";
import ProcessingAndSuccessMessage from "@components/Processing/ProcessingAndSuccessMessage";
import { useElementActionMutation } from "@hooks/Journal/Element/useElementActionMutation"
import { useRemoveElementFromTrade } from "@hooks/Journal/Element/useRemoveElementFromTrade"

const buttonContainerStyle = {
  display: "flex",
  justifyContent: "center", // Center horizontally
  alignItems: "center", // Center vertically
  marginLeft: "5px",
  height: "100%", // Ensure full height to center vertically
};

function ElementControls({ tradeElement, onActionSuccess }) {
  const tradeId = tradeElement[Constants.ELEMENT_COMPOSITEFK_STING];
  const { removeElement } = useRemoveElementFromTrade(tradeId);

  const processTradeAction = ({ action, response }) => {
    if (action === ElementActions.DELETE) {
      removeElement(tradeElement.id);
    }
    onActionSuccess(response);
  }
  const { elementActionMutation, processingStatus } = useElementActionMutation(tradeElement, processTradeAction);

  const handleAction = (action) => {
    elementActionMutation.mutate({ action });
  };

  return (
    <>
      <div style={buttonContainerStyle}>
        <Menu shadow="md" width={150} position="bottom-end">
          <Menu.Target>
            <ActionIcon variant="subtle">
              <IconDots />
            </ActionIcon>
          </Menu.Target>
          <Menu.Dropdown>
            <Menu.Item leftSection={<IconTrash size={16} />}
              onClick={() => handleAction(ElementActions.DELETE)}
            >
              Delete Element
            </Menu.Item>
          </Menu.Dropdown>
        </Menu>
      </div>
      <ProcessingAndSuccessMessage status={processingStatus} />
    </>
  );
}

export default React.memo(ElementControls);