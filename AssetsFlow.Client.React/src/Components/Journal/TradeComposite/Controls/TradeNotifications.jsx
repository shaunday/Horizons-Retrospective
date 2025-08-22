import React from "react";
import PropTypes from "prop-types";
import Notifications from "@components/Notifications";
import * as Constants from "@constants/journalConstants";

function TradeNotifications({ tradeComposite }) {
  const hasMissingContent = tradeComposite?.[Constants.HasMissingContent];

  if (!hasMissingContent) {
    return null;
  }

  return (
    <Notifications shortText="Missing" expandedText="Some required fields are empty or invalid." />
  );
}

TradeNotifications.propTypes = {
  tradeComposite: PropTypes.shape({
    [Constants.HasMissingContent]: PropTypes.bool,
  }),
};

export default React.memo(TradeNotifications);
