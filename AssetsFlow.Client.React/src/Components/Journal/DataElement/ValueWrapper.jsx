import React, { useState } from "react";
import * as Constants from "@constants/journalConstants";
import { dataParser } from "./dataParser";
import DataUpdateModal from "./DataUpdateModal";
import ComboxBoxThingie from "../../ComboxBoxThingie";
import TextWithEditTag from "../../TextWithEditTag";

function ValueWrapper({ cellInfo, onValueChangeInitiated }) {
  const isOverview = onValueChangeInitiated === undefined;
  const { contentValue } = dataParser(cellInfo);
  const shouldRenderDialog = cellInfo[Constants.DATA_RESTRICTION_STRING]?.length === 0;
  const [modalOpened, setModalOpened] = useState(false);

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

  const onEditRequested = () => {
    setModalOpened(true);
  }

  return (
    <div style={wrapperStyle}>
      {isOverview ? (
        <span style={textStyle}>{contentValue}</span>
      ) : shouldRenderDialog ? (
        <div>
          <TextWithEditTag
            valueForDisplay={contentValue}
            onEditRequested={onEditRequested}
          />
          <DataUpdateModal
            opened={modalOpened}
            onClose={() => setModalOpened(false)}
            data={cellInfo}
            onSubmit={onValueChangeInitiated} />
        </div>
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