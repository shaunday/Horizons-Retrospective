function useAllTrades() {
    const { data: query } = useQuery({
        queryKey: [Constants.ALL_TRADES_KEY],
        queryFn: async () => await TradesApiAccess.getAllTrades().data
      })
  
    return query
  }