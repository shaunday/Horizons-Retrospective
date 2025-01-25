import React from "react";
import ComboxBoxThingie from "../../ComboxBoxThingie";
import InlineTextDialog from "../../InlineTextDialog";
import * as Constants from "@constants/journalConstants";
import { getContent } from "./contentGetters";

function ValueWrapper({ cellInfo, onValueChangeInitiated }) {
  const { contentValue } = getContent(cellInfo);
  const shouldRenderDialog  = cellInfo[Constants.DATA_RESTRICTION_STRING]?.length === 0;

  return (
    <>
      {shouldRenderDialog  ? (
        <InlineTextDialog
          valueForDisplay={contentValue}
          onApplyChanges={onValueChangeInitiated}
        />
      ) : (
        <ComboxBoxThingie
          selected={contentValue}
          options={cellInfo[Constants.DATA_RESTRICTION_STRING]}
          onSelect={onValueChangeInitiated}
        />
      )}
    </>
  );
}

export default ValueWrapper;