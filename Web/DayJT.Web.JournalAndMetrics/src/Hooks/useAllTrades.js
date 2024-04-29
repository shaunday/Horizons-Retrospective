import { useEffect, useState } from 'react';

export default function useAllTrades() {
  const [allTrades, setAllTrades] = useState([]);
  const [datesFilter, setDatesFilter] = useState([]);
  const [filteredTrades, setFilteredTrades] = useState([]);

// Update filteredTrades whenever allTrades changes
useEffect(() => {
  const filtered = allTrades.filter(trade => trade.amount > 100);
  setFilteredTrades(filtered);
}, [allTrades]);

  return { allTrades, setAllTrades };
}
