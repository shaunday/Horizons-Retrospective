import { useState } from "react";
import TradeElement from "./TradeElement";
import CompositeControls from "./CompositeControls";
import { useEntryUpdate } from "@hooks/useTradeUpdate";
import * as Constants from "@constants/journalConstants";

export default function TradeComposite({ tradeComposite }) {
  const initialTradeSummaryValue =
    tradeComposite[Constants.TRADE_SUMMARY_STRING];
  const initialElementsValue = tradeComposite[Constants.TRADE_ELEMENTS_STRING];

  const [tradeSummary, setTradeSummary] = useState(initialTradeSummaryValue);

  const entryUpdate = useEntryUpdate(tradeComposite);

  const processEntryUpdate = ({ updatedEntry, newSummary }) => {
    entryUpdate(updatedEntry);
    setTradeSummary(newSummary);
  };

  const processElementAdded = ({ newElement, newSummary }) => {
    //todo add to composite
    setTradeSummary(newSummary);
  };

  return (
    <div id="tradeComposite">
      <ul>
        {initialElementsValue.map((ele) => (
          <li key={ele.id}>
            <TradeElement
              tradeElement={ele}
              onElementContentUpdate={processEntryUpdate}
            />
          </li>
        ))}
      </ul>
      {tradeSummary ? <TradeElement tradeStep={tradeSummary} /> : ""}
      <CompositeControls
        tradeComposite={tradeComposite}
        onElementAddition={processElementAdded}
      />
    </div>
  );
}
