import { useEffect, useState } from 'react';

export default function useAllTrades() {
  const [allTrades, setAllTrades] = useState([]);
  const [filter, setFilter] = useStFilter([]);
  const [filteredTrades, setFilteredTrades] = useFilteredState([]);

// Update filteredTrades whenever allTrades changes
useEffect(() => {
  // Filter trades with amount greater than 100
  const filtered = allTrades.filter(trade => trade.amount > 100);
  setFilteredTrades(filtered);
}, [allTrades]);

  return { allTrades, setAllTrades };
}
