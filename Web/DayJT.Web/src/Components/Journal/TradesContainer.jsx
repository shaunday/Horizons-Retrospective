import TradeComposite from "@journalComponents/TradeComposite";

function TradesContainer({ trades }) {
  const queryClient = useQueryClient();

  const [tradesForDisplay, setTradesForDisplay] = useState(
    tradeIds.map((id) =>
      queryClient.getQueryData([Constants.TRADE_CLIENTID_PROP_STRING, id])
    )
  );
  return (
    <ul>
      {tradesForDisplay.map((composite) => (
        <li key={composite.id}>
          <TradeComposite tradeComposite={composite} />
        </li>
      ))}
    </ul>
  );
}

export default React.memo(TradesContainer);
