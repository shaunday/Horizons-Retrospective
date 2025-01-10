import TradeElement from "@journalComponents/TradeElement";
import CompositeControls from "./CompositeControls";
import { useTrade } from "@hooks/useTrade";
import { useTradeStateAndManagement } from "@hooks/Composite/useTradeStateAndManagement";
import * as Constants from "@constants/journalConstants";

function TradeComposite({ tradeId }) {
  const { cachedTradeComposite } = useTrade(tradeId);
  const { tradeSummary, processEntryUpdate, processTradeAction } =
    useTradeStateAndManagement(cachedTradeComposite);

  return (
    <>
      <ul>
        {cachedTradeComposite[Constants.TRADE_ELEMENTS_STRING].map((ele) => (
          <li key={ele.id}>
            <TradeElement
              tradeElement={ele}
              onElementContentUpdate={processEntryUpdate}
            />
          </li>
        ))}
      </ul>
      {tradeSummary && <TradeElement tradeElement={tradeSummary} />}
      <CompositeControls
        tradeComposite={cachedTradeComposite}
        onTradeActionExecuted={processTradeAction}
      />
    </>
  );
}

export default React.memo(TradeComposite);
