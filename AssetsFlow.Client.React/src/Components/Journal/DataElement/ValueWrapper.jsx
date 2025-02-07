import React from "react";
import ComboxBoxThingie from "../../ComboxBoxThingie";
import InlineTextDialog from "../../InlineTextDialog";
import * as Constants from "@constants/journalConstants";
import { getContent } from "./contentGetters";

function ValueWrapper({ cellInfo, onValueChangeInitiated }) {
  const isOverview = onValueChangeInitiated === undefined;
  const { contentValue } = getContent(cellInfo);
  const shouldRenderDialog = cellInfo[Constants.DATA_RESTRICTION_STRING]?.length === 0;

  const wrapperStyle = {
    display: "flex",
    alignItems: "center", // Ensure vertical alignment of children
    height: "40px",
    width: "100%",
  };

  const textStyle = {
    display: "flex",
    justifyContent: "center", // Center horizontally
    alignItems: "center", // Center vertically
    width: "100%",
    height: "100%",
  };

  return (
    <div style={wrapperStyle}>
      {isOverview ? (
        <span style={textStyle}>{contentValue}</span> 
      ) : shouldRenderDialog ? (
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
    </div>
  );
}

export default React.memo(ValueWrapper);